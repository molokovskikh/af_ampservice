using System;
using Common.Models;
using NUnit.Framework;

namespace Integration
{
	[TestFixture]
	public class GetOrderFixture : IntegrationFixture
	{
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
		public void Get_order_should_return_order_by_id()
		{
			Execute(@"
delete from orders.ordershead
where writetime > curdate() and clientcode = 2575;

update usersettings.RetClientsSet
set ServiceClient = 0,
	InvisibleOnFirm = 0
where ClientCode = 2575");
			var orderId = BuildOrder();
			var data = service.GetOrder(orderId);
			Assert.That(data.Tables[0].Rows.Count, Is.EqualTo(1));
			Assert.That(data.Tables[0].Rows[0]["OrderID"], Is.EqualTo(orderId));
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
	}

	public class IntegrationFixture
	{
		protected AmpService.AmpService service;

		[SetUp]
		public void Setup()
		{
			service = new AmpService.AmpService();
		}

		public void Execute(string command)
		{
			With.Session(s => s.CreateSQLQuery(command).ExecuteUpdate());
		}
	}
}
