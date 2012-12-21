using System;
using System.Data;
using System.Linq;
using Castle.ActiveRecord;
using Common.Models;
using Common.Models.Repositories;
using NUnit.Framework;
using Test.Support;

namespace Integration
{
	[TestFixture]
	public class PostOrderFixture : IntegrationFixture
	{
		private User user;

		[SetUp]
		public void Setup()
		{
			user = IoC.Resolve<IRepository<User>>().Get(testUser.Id);
		}

		[Test]
		public void Post_order()
		{
			var orderRepository = IoC.Resolve<IRepository<Order>>();
			var offerRepository = IoC.Resolve<IOfferRepository>();

			var data = service.GetPrices(false, false,
				new[] { "OriginalName" },
				new[] { "*папа*" },
				new string[] { },
				new string[] { },
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
")
				.SetParameter("CoreId", coreId).ExecuteUpdate());

			var result = service.PostOrder(new[] { coreId },
				new[] { 20u },
				new[] { "это тестовый заказ" },
				new[] { Convert.ToUInt32(data.Tables[0].Rows[0]["OrderCode1"]) },
				new[] { Convert.ToUInt32(data.Tables[0].Rows[0]["OrderCode2"]) },
				new[] { false });

			Assert.That(result.Tables[0].Rows.Count, Is.EqualTo(1), "ни вернули ни одной записи");
			Assert.That(result.Tables[0].Rows[0]["OriginalOrderID"], Is.EqualTo(coreId),
				"заказали что то не то, идентификатор из core не совпал");
			var orderid = Convert.ToUInt32(result.Tables[0].Rows[0]["OrderId"]);

			var offer = offerRepository.GetById(user, coreId);
			var order = orderRepository.Get(orderid);

			var orderLine = (from orderItem in order.OrderItems
				where orderItem.CoreId == coreId
				select orderItem).Single();

			Assert.That(offer.Id.CoreId, Is.EqualTo(orderLine.CoreId));
			Assert.That(orderLine.RequestRatio, Is.EqualTo(offer.RequestRatio));
			Assert.That(orderLine.MinOrderCount, Is.EqualTo(offer.MinOrderCount));
			Assert.That(orderLine.OrderCost, Is.EqualTo(offer.OrderCost));
		}

		[Test]
		public void Post_order_with_single_message()
		{
			var data = service.GetPrices(false, false,
				new[] { "OriginalName" },
				new[] { "*папа*" },
				null,
				null,
				100,
				0);
			Assert.That(data.Tables[0].Rows.Count, Is.GreaterThan(0), "предложений нет");
			var coreId1 = Convert.ToUInt64(data.Tables[0].Rows[0]["OrderID"]);
			var coreId2 = Convert.ToUInt64(data.Tables[0].Rows[1]["OrderID"]);
			service.PostOrder(
				new[] { coreId1, coreId2 }, new[] { 30u, 10u }, new[] { "" },
				new[] { Convert.ToUInt32(data.Tables[0].Rows[0]["OrderCode1"]), Convert.ToUInt32(data.Tables[0].Rows[1]["OrderCode1"]) },
				new[] { Convert.ToUInt32(data.Tables[0].Rows[0]["OrderCode2"]), Convert.ToUInt32(data.Tables[0].Rows[1]["OrderCode2"]) },
				new[] { false, false });
		}

		[Test]
		public void If_offer_does_not_exists_return_new_one()
		{
			var data = service.GetPrices(false, false,
				new[] { "OriginalName" },
				new[] { "*папа*" },
				null,
				null,
				100,
				0);
			Assert.That(data.Tables[0].Rows.Count, Is.GreaterThan(0), "предложений нет");
			var orderCode1 = 5u;
			var orderCode2 = 6u;
			var result = service.PostOrder(
				new[] { 1ul },
				new[] { 30u },
				new[] { "" },
				new[] { orderCode1 },
				new[] { orderCode2 },
				new[] { false });
			Assert.That(result.Tables[0].Rows[0]["OrderID"], Is.EqualTo(-1));
			Assert.That(result.Tables[0].Rows[0]["OriginalOrderID"], Is.EqualTo(1));
		}

		[Test]
		public void Do_not_chek_order_rules()
		{
			var data = service.GetPrices(false, false,
				new[] { "OriginalName" },
				new[] { "*папа*" },
				null,
				null,
				100,
				0);
			Assert.That(data.Tables[0].Rows.Count, Is.GreaterThan(0), "предложений нет");
			var coreId = Convert.ToUInt64(data.Tables[0].Rows[0]["OrderID"]);
			With.Session(s => s.CreateSQLQuery(@"
update farm.core0
set RequestRatio = 13
where id = :CoreId")
				.SetParameter("CoreId", coreId).ExecuteUpdate());
			PostOrder(data.Tables[0].Rows[0]);
		}

		[Test]
		public void Post_order_by_future_clients()
		{
			var data = GetPrices();

			var row = data.Tables[0].Rows[0];
			PostOrder(row);

			using (new SessionScope()) {
				var orders = TestOrder.Queryable.Where(o => o.Client.Id == testClient.Id).ToList();
				Assert.That(orders.Count, Is.EqualTo(1));
				var order = orders.First();
				Assert.That(order.Address.Id, Is.EqualTo(testClient.Addresses.First().Id));
				Assert.That(order.User.Id, Is.EqualTo(testUser.Id));
			}
		}

		[Test]
		public void Sugest_order_for_new_client()
		{
			var data = GetPrices();

			var row = data.Tables[0].Rows[0];
			var coreId = Convert.ToUInt64(row[0]);

			var result = service.PostOrder(new[] { 1UL },
				new[] { 30u },
				new[] { "" },
				new[] { Convert.ToUInt32(row["OrderCode1"]) },
				new[] { Convert.ToUInt32(row["OrderCode2"]) },
				new[] { false });

			Assert.That(result.Tables[0].Rows.Count, Is.EqualTo(1));
			Assert.That(Convert.ToInt32(result.Tables[0].Rows[0]["OrderID"]), Is.EqualTo(coreId));
			Assert.That(result.Tables[0].Columns.Contains("SalerCode"), Is.True);
		}

		private void PostOrder(DataRow row)
		{
			var result = service.PostOrder(new[] { Convert.ToUInt64(row["OrderId"]) }, new[] { 30u }, new[] { "" },
				new[] { Convert.ToUInt32(row["OrderCode1"]) },
				new[] { Convert.ToUInt32(row["OrderCode2"]) },
				new[] { false });

			Assert.That(result.Tables[0].Rows.Count, Is.EqualTo(1));
			Assert.That(Convert.ToInt32(result.Tables[0].Rows[0]["OrderID"]), Is.Not.EqualTo(-1), "ничего не заказли");
		}

		private DataSet GetPrices()
		{
			var data = service.GetPrices(
				false,
				false,
				new[] { "OriginalName" },
				new[] { "*папа*" },
				null,
				null,
				100,
				0);
			Assert.That(data.Tables[0].Rows.Count, Is.GreaterThan(0), "предложений нет");
			return data;
		}
	}
}