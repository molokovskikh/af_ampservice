using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using NUnit.Framework;

namespace Integration
{
	[TestFixture]
	public class HttpFixture
	{
		public void Execute(string commandText)
		{
			using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Main"].ConnectionString))
			{
				connection.Open();
				var command = connection.CreateCommand();
				command.CommandText = commandText;
				command.ExecuteNonQuery();
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
			var offers = Request("GetPrices", false,
			                     false,
			                     new[] {"OriginalName", "PriceCode"},
			                     new[] {"*папа*", "94"}, null, null, 100, 0);
			var offer = offers.Tables[0].Rows[0];
			var orderIds = Request( "PostOrder", new[] {Convert.ToInt64(offer["OrderID"])},
			                        new[] {1},
			                        new[] {"Тестовое сообщение"},
			                        new[] {Convert.ToInt32(offer["OrderCode1"])},
			                        new[] {Convert.ToInt32(offer["OrderCode2"])},
			                        new[] {false});
			var orderId = Convert.ToInt64(orderIds.Tables[0].Rows[0]["OrderID"]);
			Execute(String.Format(@"
update orders.ordershead
set Submited = 1
where RowId = {0}", orderId));

			var orders = Request("GetOrdersByDate", begin, 0);
			Assert.That(orders, Is.Not.Null);
			Assert.That(orders.Tables.Count, Is.GreaterThan(0));
			Assert.That(orders.Tables[0].Rows.Count, Is.EqualTo(1));
			var order = orders.Tables[0].Rows[0];
			Assert.That(Convert.ToInt64(order["OrderID"]), Is.EqualTo(orderId));
		}

		public static DataSet Request(string method, params object[] args)
		{
			return null;
		}
	}
}