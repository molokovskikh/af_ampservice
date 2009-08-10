using System;
using System.Data;
using Common.Models;
using NUnit.Framework;

namespace Integration
{
	[TestFixture]
	public class IntegrationTests
	{
		private AmpService.AmpService service;

		[SetUp]
		public void Setup()
		{
			service = new AmpService.AmpService();
		}

		public void Execute(string command)
		{
			With.Session(s => s.CreateSQLQuery(command).ExecuteUpdate());
		}

		[Test]
		public void GetNameFromCatalogTest()
		{

			LogDataSet(service.GetNameFromCatalog(null, null, false, false, null, 100, 0));

			LogDataSet(service.GetNameFromCatalog(null, null, false, false, new uint[0], 100, 0));

			LogDataSet(service.GetNameFromCatalog(new[] { "*" }, new[] { "*" }, false, false, new uint[0], 100, 0));

			LogDataSet(service.GetNameFromCatalog(new String[0], new String[0], false, false, new uint[] { 5 }, 100, 0));

			LogDataSet(service.GetNameFromCatalog(new[] { "5*" }, new[] { "*" }, true, false, new uint[0], 100, 0));

			LogDataSet(service.GetNameFromCatalog(null, null, false, true, null, 100, 0));

			LogDataSet(service.GetNameFromCatalog(null, null, false, true, new uint[0], 100, 0));

			LogDataSet(service.GetNameFromCatalog(new[] { "5*" }, new[] { "*" }, false, true, new uint[0], 100, 0));

			LogDataSet(service.GetNameFromCatalog(new String[0], new String[0], false, true, new uint[] { 5 }, 100, 0));

			LogDataSet(service.GetNameFromCatalog(new[] { "5*" }, new[] { "*" }, true, true, new uint[0], 100, 0));
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
		public void Get_orders_older_than()
		{
			var begin = DateTime.Now;
			Execute(@"
delete from orders.ordershead
where writetime > curdate() and clientcode = 2575;

update usersettings.RetClientsSet
set ServiceClient = 0,
	InvisibleOnFirm = 0
where ClientCode = 2575");
			var offers = service.GetPrices(false,
			                               false,
			                               new[] {"OriginalName", "PriceCode"},
			                               new[] {"*папа*", "94"}, null, null, 100, 0);
			var offer = offers.Tables[0].Rows[0];
			var orderIds = service.PostOrder(new[] {Convert.ToUInt64(offer["OrderID"])},
			                                 new[] {1u},
			                                 new[] {"Тестовое сообщение"},
			                                 new[] {Convert.ToUInt32(offer["OrderCode1"])},
			                                 new[] {Convert.ToUInt32(offer["OrderCode2"])},
			                                 new[] {false});
			var orderId = Convert.ToInt64(orderIds.Tables[0].Rows[0]["OrderID"]);
			Execute(String.Format(@"
update orders.ordershead
set Submited = 1
where RowId = {0}", orderId));

			var orders = service.GetOrdersByDate(begin, 0);
			Assert.That(orders, Is.Not.Null);
			Assert.That(orders.Tables.Count, Is.GreaterThan(0));
			Assert.That(orders.Tables[0].Rows.Count, Is.EqualTo(1));
			var order = orders.Tables[0].Rows[0];
			Assert.That(Convert.ToInt64(order["OrderID"]), Is.EqualTo(orderId));
		}

		[Test]
		public void Do_not_show_orders_from_service_clients()
		{
			var begin = DateTime.Now;
			Execute(@"
delete from orders.ordershead
where writetime > curdate() and clientcode = 2575;

update usersettings.RetClientsSet 
set ServiceClient = 1,
	InvisibleOnFirm = 0
where ClientCode = 2575");

			BuildOrder();

			var orders = service.GetOrdersByDate(begin, 0);
			Assert.That(orders.Tables[0].Rows.Count, Is.EqualTo(0));
		}

		private uint BuildOrder()
		{
			var offers = service.GetPrices(false,
			                               false,
			                               new[] { "OriginalName", "PriceCode" },
			                               new[] { "*папа*", "94" }, null, null, 100, 0);
			var offer = offers.Tables[0].Rows[0];
			var orderIds = service.PostOrder(new[] { Convert.ToUInt64(offer["OrderID"]) },
			                                 new[] { 1u },
			                                 new[] { "Тестовое сообщение" },
			                                 new[] { Convert.ToUInt32(offer["OrderCode1"]) },
			                                 new[] { Convert.ToUInt32(offer["OrderCode2"]) },
			                                 new[] { false });
			var orderId = Convert.ToUInt32(orderIds.Tables[0].Rows[0]["OrderID"]);
			Execute(String.Format(@"
update orders.ordershead
set Submited = 1
where RowId = {0}", orderId));
			return orderId;
		}

		[Test]
		public void Do_not_show_orders_from_hidden_clients()
		{
			Execute(@"
delete from orders.ordershead
where writetime > curdate() and clientcode = 2575;

update usersettings.RetClientsSet
set ServiceClient = 0,
	InvisibleOnFirm = 2
where ClientCode = 2575");

			BuildOrder();

			var orders = service.GetOrdersByDate(DateTime.Now, 0);
			Assert.That(orders.Tables[0].Rows.Count, Is.EqualTo(0));
		}

		[Test]
		public void GetPriceCodeByNameTest()
		{
			LogDataSet(service.GetPriceCodeByName(null));
			LogDataSet(service.GetPriceCodeByName(new[] { "Протек-15" }));
			LogDataSet(service.GetPriceCodeByName(new[] { "Протек-15", "Материа Медика" }));
			LogDataSet(service.GetPriceCodeByName(new[] { "Протек*" }));
			LogDataSet(service.GetPriceCodeByName(new[] { "*к*", "Ма*риа Ме*ка" }));
		}


	}
}