using System;
using System.Data;
using System.Web.Services.Protocols;
using log4net.Config;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace AmpService.Tests
{
	public class AssertHelper
	{
		public static void ThrowException(Action action, Action<Exception> afterThrow)
		{
			try
			{
				action();
				Assert.Fail("исключение не было выброшено");
			}
			catch (Exception e)
			{
				if (e is AssertionException)
					throw;
				afterThrow(e);
			}
		}
	}

	[TestFixture]
	public class IntegrationTests
	{
		private AMPService _service;

		[SetUp]
		public void Setup()
		{
			_service = new AMPService
			{
				GetHost = () => "localhost",
				GetUserName = () => "kvasov"
			};

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


		static IntegrationTests()
		{
			XmlConfigurator.Configure();
		}

		[Test]
		public void GetNameFromCatalogTest()
		{

			LogDataSet(_service.GetNameFromCatalog(null, null, false, false, null, 100, 0));

			LogDataSet(_service.GetNameFromCatalog(null, null, false, false, new uint[0], 100, 0));

			LogDataSet(_service.GetNameFromCatalog(new[] { "*" }, new[] { "*" }, false, false, new uint[0], 100, 0));

			LogDataSet(_service.GetNameFromCatalog(new String[0], new String[0], false, false, new uint[] { 5 }, 100, 0));

			LogDataSet(_service.GetNameFromCatalog(new[] { "5*" }, new[] { "*" }, true, false, new uint[0], 100, 0));

			LogDataSet(_service.GetNameFromCatalog(null, null, false, true, null, 100, 0));

			LogDataSet(_service.GetNameFromCatalog(null, null, false, true, new uint[0], 100, 0));

			LogDataSet(_service.GetNameFromCatalog(new[] { "5*" }, new[] { "*" }, false, true, new uint[0], 100, 0));

			LogDataSet(_service.GetNameFromCatalog(new String[0], new String[0], false, true, new uint[] { 5 }, 100, 0));

			LogDataSet(_service.GetNameFromCatalog(new[] { "5*" }, new[] { "*" }, true, true, new uint[0], 100, 0));
		}

		private static void LogDataSet(DataSet dataSet)
		{
			foreach (DataTable dataTable in dataSet.Tables)
			{
				Console.WriteLine("<table>");
				foreach (DataRow dataRow in dataTable.Rows)
				{
					Console.WriteLine("\t<row>");
					Console.Write("\t");
					foreach (DataColumn column in dataTable.Columns)
					{
						Console.Write(dataRow[column] + " ");
					}
					Console.WriteLine();
					Console.WriteLine("\t</row>");
				}
				Console.WriteLine("</table>");
			}
		}


		[Test]
		public void GetOrdersTest()
		{
			LogDataSet(_service.GetOrders(null, 0));
			LogDataSet(_service.GetOrders(new string[0], 0));
			LogDataSet(_service.GetOrders(new[] { "1" }, 2));
			LogDataSet(_service.GetOrders(new[] { "1", "2", "3" }, 2));
			LogDataSet(_service.GetOrders(new[] { "0" }, -1));
			LogDataSet(_service.GetOrders(new[] { "!1" }, -1));
		}

		[Test]
		public void GetPriceCodeByNameTest()
		{
			LogDataSet(_service.GetPriceCodeByName(null));
			LogDataSet(_service.GetPriceCodeByName(new[] { "Протек-15" }));
			LogDataSet(_service.GetPriceCodeByName(new[] { "Протек-15", "Материа Медика" }));
			LogDataSet(_service.GetPriceCodeByName(new[] { "Протек*" }));
			LogDataSet(_service.GetPriceCodeByName(new[] { "*к*", "Ма*риа Ме*ка" }));
		}

		[Test]
		public void GetPricesTest()
		{
			LogDataSet(_service.GetPrices(false, false,
										 new[]
			                             	{
			                             		"prepCode", "PriceCode", "PriceCode", "PrepCode", "ItemID", "PrepCode", "PrepCode",
			                             		"OriginalName", "ItemID", "PriceCode"
			                             	},
										 new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" }, null, null, 100, 0));


			LogDataSet(_service.GetPrices(false, false, new[] { "OriginalName" }, new[] { "*а*" }, null, null, 100, 0));


			LogDataSet(_service.GetPrices(false, false, new[] { "OriginalName", "OriginalName" }, new[] { "к*", "т*" }, null,
										 null, 100, 0));


			_service.GetPrices(false, false, new[] { "OriginalName", "OriginalName" }, new[] { "к*", "т*" },
							  new[] { "OrderID" }, null, 100, 0);


			_service.GetPrices(false, false, new[] { "OriginalName", "OriginalName" }, new[] { "к*", "т*" },
							  new[] { "OrderID" }, new[] { "DESC" }, 100, 0);


			_service.GetPrices(false, false, new[] { "OriginalName", "originalName" }, new[] { "к*", "т*" },
							  new[] { "orderID", "unit", "Volume" }, new[] { "DESC" }, 100, 0);


			_service.GetPrices(true, false, new[] { "PrepCode", "PrepCode", "PrepCode" },
							  new[] { "12999", "12998", "29652" }, null, null, -1, -1);


			_service.GetPrices(true, false, new[] { "PrepCode", "PrepCode", "PrepCode" },
							  new[] { "12999", "12998", "29652" }, null, null, 2, 1);


			_service.GetPrices(true, false, new[] { "PrepCode", "PrepCode", "PrepCode" },
							  new[] { "12999", "12998", "29652" }, new[] { "OrderID", "unit", "Volume" },
							  new[] { "DESC" }, -1, -1);


			_service.GetPrices(true, false, new[] { "PrepCode", "PrepCode", "PrepCode" },
							  new[] { "12999", "12998", "29652" }, new[] { "OrderID", "unit", "Volume" },
							  new[] { "ASC" }, -1, 1);


			_service.GetPrices(false, false, new[] { "PrepCode" }, new[] { "5" }, new[] { "Cost" },
							  new[] { "ASC" }, 1000, 0);
		}

		[Test]
		public void Throw_exception_if_now_parameters_specified()
		{
			AssertHelper.ThrowException(() =>
			                            _service.GetPrices(false,
			                                               false,
			                                               new string[0],
			                                               new string[0],
			                                               new string[0],
			                                               new string[0], 0, 10),
			                            e => Assert.That(e, Is.InstanceOfType(typeof (SoapException))));

		}
	}
}
