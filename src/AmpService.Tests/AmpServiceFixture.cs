using System;
using System.Linq;
using System.Data;
using System.Reflection;
using Castle.Windsor;
using Common.Models;
using Common.Models.Repositories;
using log4net.Config;
using MySql.Data.MySqlClient;
using NHibernate.Mapping.Attributes;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace AmpService.Tests
{
	[TestFixture]
	public class AmpServiceFixture
	{
		private AMPService _service;

		static AmpServiceFixture()
		{
			XmlConfigurator.Configure();
		}

		[SetUp]
		public void Setup()
		{
			_service = new AMPService
			           	{
			           		GetHost = () => "localhost", 
							GetUserName = () => "kvasov"
			           	};

			var container = new WindsorContainer();
			container.AddComponent("Repository", typeof(IRepository<>), typeof(Repository<>));
			container.AddComponent<IOfferRepository, OfferRepository>();
			container.AddComponent("RepositoryInterceptor", typeof(RepositoryInterceptor));
			var holder = new SessionFactoryHolder();
			holder
				.Configuration
				.Configure()
				.AddInputStream(HbmSerializer.Default.Serialize(Assembly.Load("Common.Models")));
			holder.BuildSessionFactory();
			container.Kernel.AddComponentInstance<ISessionFactoryHolder>(holder);

			IoC.Initialize(container);
		}

		[Test]
		public void PostOrderTest()
		{
			var orderRepository = IoC.Resolve<IRepository<Order>>();
			var offerRepository = IoC.Resolve<IOfferRepository>();

			var data = _service.GetPrices(false, false,
			                             new[] {"OriginalName"},
			                             new[] {"*папа*"},
			                             new string[] {},
			                             new string[] {},
			                             100,
			                             0);
			Assert.That(data.Tables[0].Rows.Count, Is.GreaterThan(0), "предложений нет");
			var coreId = Convert.ToUInt64(data.Tables[0].Rows[0]["OrderID"]);

			using (var connection = new MySqlConnection(Literals.ConnectionString))
			{
				connection.Open();
				var command = new MySqlCommand(@"
update farm.core0
set RequestRatio = 5,
	OrderCost = 10.5,
	MinOrderCount = 10
where id = ?CoreId
", connection);
				command.Parameters.AddWithValue("?CoreId", coreId);
				command.ExecuteNonQuery();
			}


			var result = _service.PostOrder(new[] { (long)coreId },
			                                new[] {1},
			                                new[] {"это тестовый заказ"},
											new[] { Convert.ToInt32(data.Tables[0].Rows[0]["OrderCode1"]) },
											new[] { Convert.ToInt32(data.Tables[0].Rows[0]["OrderCode2"]) },
			                                new[] {false});

			using (var unitOfWork = new UnitOfWork())
			using (var transaction = unitOfWork.CurrentSession.BeginTransaction(IsolationLevel.RepeatableRead))
			{
				Assert.That(result.Tables[0].Rows.Count, Is.EqualTo(1), "ни вернули ни одной записи");
				Assert.That(result.Tables[0].Rows[0]["OriginalOrderID"], Is.EqualTo(coreId),
				            "заказали что то не то, идентификатор из core не совпал");
				var orderid = Convert.ToUInt32(result.Tables[0].Rows[0]["OrderId"]);

				var offer = offerRepository.GetById(new Client { FirmCode = 2575 }, coreId);
				var order = orderRepository.Get(orderid);

				var orderLine = (from orderItem in order.OrderItems where orderItem.CoreId == coreId select orderItem).Single();

				Assert.That(offer.Id, Is.EqualTo(orderLine.CoreId));
				Assert.That(orderLine.RequestRatio, Is.EqualTo(offer.RequestRatio));
				Assert.That(orderLine.MinOrderCount, Is.EqualTo(offer.MinOrderCount));
				Assert.That(orderLine.OrderCost, Is.EqualTo(offer.OrderCost));
			}
		}

		[Test]
		public void Every_method_should_return_null_if_user_not_have_iol_permission()
		{
			_service.HavePermission = userName => false;

			Assert.That(_service.GetOrders(new[] {"0"}, 0),
			            Is.Null);

			Assert.That(_service.GetNameFromCatalog(new[] {""},
			                                        new string[] {},
			                                        false,
			                                        false,
			                                        new uint[] {}, 100, 0),
			            Is.Null);

			Assert.That(_service.GetPriceCodeByName(new[] {"%протек%"}),
			            Is.Null);

			Assert.That(_service.GetPrices(false, false,
			                               new[] {"OriginalName"},
			                               new[] {"%папа%"},
			                               new string[] {},
			                               new string[] {}, 100, 0),
			            Is.Null);

			Assert.That(_service.PostOrder(new[] {54621354879},
			                               new[] {1},
			                               new[] {"123"},
			                               new[] {46528},
			                               new[] {544523},
			                               new[] {false}), Is.Null);
		}

		[Test]
		public void HavePermissionTest()
		{
			Execute(@"
delete ap from AssignedPermissions ap, osuseraccessright oar, userpermissions up
where ap.permissionid = up.id and oar.rowid = ap.userid and oar.osusername = 'kvasov' and up.shortcut = 'IOL';");
			Assert.That(_service.HavePermission("kvasov"), Is.False);

			Execute(@"
insert into AssignedPermissions(userid, permissionid)
select oar.rowid, up.id
from osuseraccessright oar, userpermissions up
where oar.osusername = 'kvasov' and up.shortcut = 'IOL';");
			Assert.That(_service.HavePermission("kvasov"), Is.True);
		}

		public void Execute(string commandText)
		{
			using (var connection = new MySqlConnection(Literals.ConnectionString))
			{
				connection.Open();
				var command = connection.CreateCommand();
				command.CommandText = commandText;
				command.ExecuteNonQuery();
			}
		}
	}
}
