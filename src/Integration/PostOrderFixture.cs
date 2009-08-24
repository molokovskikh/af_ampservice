using System;
using System.Linq;
using Common.Models;
using Common.Models.Repositories;
using NUnit.Framework;

namespace Integration
{
	[TestFixture]
	public class PostOrderFixture
	{
		private AmpService.AmpService service;

		[SetUp]
		public void Setup()
		{
			service = new AmpService.AmpService();
		}

		[Test]
		public void Post_order()
		{
			var orderRepository = IoC.Resolve<IRepository<Order>>();
			var offerRepository = IoC.Resolve<IOfferRepository>();

			var data = service.GetPrices(false, false,
			                             new[] {"OriginalName"},
			                             new[] {"*папа*"},
			                             new string[] {},
			                             new string[] {},
			                             100,
			                             0);
			Assert.That(data.Tables[0].Rows.Count, Is.GreaterThan(0), "предложений нет");
			var coreId = Convert.ToUInt64(data.Tables[0].Rows[0]["OrderID"]);

			With.Session(s => s.CreateSQLQuery(@"
update farm.core0
set RequestRatio = 5,
	OrderCost = 10.5,
	MinOrderCount = 10
where id = :CoreId
").SetParameter("CoreId", coreId).ExecuteUpdate());

			var result = service.PostOrder(new[] { coreId },
			                               new[] {20u},
			                               new[] {"это тестовый заказ"},
			                               new[] { Convert.ToUInt32(data.Tables[0].Rows[0]["OrderCode1"]) },
			                               new[] { Convert.ToUInt32(data.Tables[0].Rows[0]["OrderCode2"]) },
			                               new[] {false});

			Assert.That(result.Tables[0].Rows.Count, Is.EqualTo(1), "ни вернули ни одной записи");
			Assert.That(result.Tables[0].Rows[0]["OriginalOrderID"], Is.EqualTo(coreId),
			            "заказали что то не то, идентификатор из core не совпал");
			var orderid = Convert.ToUInt32(result.Tables[0].Rows[0]["OrderId"]);

			var offer = offerRepository.GetById(new Client { FirmCode = 2575 }, coreId);
			var order = orderRepository.Get(orderid);

			var orderLine = (from orderItem in order.OrderItems
							 where orderItem.CoreId == coreId
							 select orderItem).Single();

			Assert.That(offer.Id, Is.EqualTo(orderLine.CoreId));
			Assert.That(orderLine.RequestRatio, Is.EqualTo(offer.RequestRatio));
			Assert.That(orderLine.MinOrderCount, Is.EqualTo(offer.MinOrderCount));
			Assert.That(orderLine.OrderCost, Is.EqualTo(offer.OrderCost));
		}

		[Test]
		public void Do_not_chek_order_rules()
		{
			var data = service.GetPrices(false, false,
			                             new[] {"OriginalName"},
			                             new[] {"*папа*"},
			                             null,
			                             null,
			                             100,
			                             0);
			Assert.That(data.Tables[0].Rows.Count, Is.GreaterThan(0), "предложений нет");
			var coreId = Convert.ToUInt64(data.Tables[0].Rows[0]["OrderID"]);
			With.Session(s =>s.CreateSQLQuery(@"
update farm.core0
set RequestRatio = 13
where id = :CoreId").SetParameter("CoreId", coreId).ExecuteUpdate());
			service.PostOrder(new[] {coreId}, new[] {30u}, new[] {""},
			                  new[] {Convert.ToUInt32(data.Tables[0].Rows[0]["OrderCode1"])},
			                  new[] {Convert.ToUInt32(data.Tables[0].Rows[0]["OrderCode2"])},
			                  new[] {false});
		}
	}
}
