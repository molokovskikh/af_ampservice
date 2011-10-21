using System;
using System.Linq;
using Castle.ActiveRecord;
using Common.Models;
using Common.Service;
using NUnit.Framework;
using Test.Support;

namespace Integration
{
	[TestFixture]
	public class GetOrderFixture : IntegrationFixture
	{
		[Test]
		public void Get_orders_older_than()
		{
			var begin = DateTime.Now;
			var orderId = BuildOrder();

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
			using (new TransactionScope())
			{
				client.Settings.ServiceClient = true;
				client.Settings.Save();
			}
			BuildOrder();

			var orders = service.GetOrdersByDate(begin, 0);
			Assert.That(orders.Tables[0].Rows.Count, Is.EqualTo(0));
		}

		[Test]
		public void Do_not_show_orders_from_hidden_clients()
		{
			var begin = DateTime.Now;
			using (new TransactionScope())
			{
				client.Settings.InvisibleOnFirm = 2;
				client.Settings.Save();
			}
			BuildOrder();

			var orders = service.GetOrdersByDate(begin, 0);
			Assert.That(orders.Tables[0].Rows.Count, Is.EqualTo(0));
		}

		[Test]
		public void Get_order_should_return_order_by_id()
		{
			var orderId = BuildOrder();
			var data = service.GetOrder(orderId);
			Assert.That(data.Tables[0].Rows.Count, Is.EqualTo(1));
			Assert.That(data.Tables[0].Rows[0]["OrderID"], Is.EqualTo(orderId));
		}

		private uint BuildOrder()
		{
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
			var orderId = Convert.ToUInt32(orderIds.Tables[0].Rows[0]["OrderID"]);
			Execute(String.Format(@"
update orders.ordershead
set Submited = 1
where RowId = {0}", orderId));
			return orderId;
		}

		[Test]
		public void Get_order_from_future_clients()
		{
			var begin = DateTime.Now;
			var orderId = BuildOrder();

			var orders = service.GetOrdersByDate(begin, 0);
			Assert.That(orders, Is.Not.Null);
			Assert.That(orders.Tables.Count, Is.GreaterThan(0));
			Assert.That(orders.Tables[0].Rows.Count, Is.EqualTo(1));
			var order = orders.Tables[0].Rows[0];
			Assert.That(Convert.ToUInt32(order["OrderID"]), Is.EqualTo(orderId));

			var orderData = service.GetOrder(orderId);
			Assert.That(orderData.Tables[0].Rows.Count, Is.EqualTo(1));
			Assert.That(Convert.ToUInt32(orderData.Tables[0].Rows[0]["OrderID"]), Is.EqualTo(orderId));
		}
	}

	public class IntegrationFixture
	{
		protected AmpService.AmpService service;
		protected TestClient client;

		[SetUp]
		public void Setup()
		{
			client = TestClient.Create();
			var user = client.Users.First();
			ServiceContext.GetUserName = () => user.Login;
			service = new AmpService.AmpService();
		}

		public void Execute(string command)
		{
			With.Session(s => s.CreateSQLQuery(command).ExecuteUpdate());
		}
	}
}
