using System;
using System.Data;
using log4net.Config;
using MySql.Data.MySqlClient;
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
			                             new[] {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10"}, null, null, 100, 0));


			LogDataSet(_service.GetPrices(false, false, new[] {"OriginalName"}, new[] {"*а*"}, null, null, 100, 0));


			LogDataSet(_service.GetPrices(false, false, new[] {"OriginalName", "OriginalName"}, new[] {"к*", "т*"}, null,
			                             null, 100, 0));


			_service.GetPrices(false, false, new[] {"OriginalName", "OriginalName"}, new[] {"к*", "т*"},
			                  new[] {"OrderID"}, null, 100, 0);


			_service.GetPrices(false, false, new[] {"OriginalName", "OriginalName"}, new[] {"к*", "т*"},
			                  new[] {"OrderID"}, new[] {"DESC"}, 100, 0);


			_service.GetPrices(false, false, new[] {"OriginalName", "originalName"}, new[] {"к*", "т*"},
			                  new[] {"orderID", "unit", "Volume"}, new[] {"DESC"}, 100, 0);


			_service.GetPrices(true, false, new[] {"PrepCode", "PrepCode", "PrepCode"},
			                  new[] {"12999", "12998", "29652"}, null, null, -1, -1);


			_service.GetPrices(true, false, new[] {"PrepCode", "PrepCode", "PrepCode"},
			                  new[] {"12999", "12998", "29652"}, null, null, 2, 1);


			_service.GetPrices(true, false, new[] {"PrepCode", "PrepCode", "PrepCode"},
			                  new[] {"12999", "12998", "29652"}, new[] {"OrderID", "unit", "Volume"},
			                  new[] {"DESC"}, -1, -1);


			_service.GetPrices(true, false, new[] {"PrepCode", "PrepCode", "PrepCode"},
			                  new[] {"12999", "12998", "29652"}, new[] {"OrderID", "unit", "Volume"},
			                  new[] {"ASC"}, -1, 1);


			_service.GetPrices(false, false, new[] {"PrepCode"}, new[] {"5"}, new[] {"Cost"},
			                  new[] {"ASC"}, 1000, 0);
		}

		[Test]
		public void GetOrdersTest()
		{
			LogDataSet(_service.GetOrders(null, 0));
			LogDataSet(_service.GetOrders(new string[0], 0));
			LogDataSet(_service.GetOrders(new[] {"1"}, 2));
			LogDataSet(_service.GetOrders(new[] {"1", "2", "3"}, 2));
			LogDataSet(_service.GetOrders(new[] {"0"}, -1));
			LogDataSet(_service.GetOrders(new[] {"!1"}, -1));
		}

		[Test]
		public void GetPriceCodeByNameTest()
		{
			LogDataSet(_service.GetPriceCodeByName(null));
			LogDataSet(_service.GetPriceCodeByName(new[] {"Протек-15"}));
			LogDataSet(_service.GetPriceCodeByName(new[] {"Протек-15", "Материа Медика"}));
			LogDataSet(_service.GetPriceCodeByName(new[] {"Протек*"}));
			LogDataSet(_service.GetPriceCodeByName(new[] {"*к*", "Ма*риа Ме*ка"}));
		}

		[Test]
		public void PostOrderTest()
		{
			var data = _service.GetPrices(false, false,
			                             new[] {"OriginalName"},
			                             new[] {"*папа*"},
			                             new string[] {},
			                             new string[] {},
			                             100,
			                             0);
			Assert.That(data.Tables[0].Rows.Count, Is.GreaterThan(0), "предложений нет");
			LogDataSet(_service.PostOrder(new[] { Convert.ToInt64(data.Tables[0].Rows[0]["OrderID"]) },
			                                new[] {1},
			                                new[] {"это тестовый заказ"},
											new[] { Convert.ToInt32(data.Tables[0].Rows[0]["OrderCode1"]) },
											new[] { Convert.ToInt32(data.Tables[0].Rows[0]["OrderCode2"]) },
			                                new[] {false}));
		}

		[Test]
		public void GetNameFromCatalogTest()
		{
			LogDataSet(_service.GetNameFromCatalog(null, null, false, false, null, 100, 0));

			_service = new AMPService();
			LogDataSet(_service.GetNameFromCatalog(null, null, false, false, new uint[0], 100, 0));

			_service = new AMPService();
			LogDataSet(_service.GetNameFromCatalog(new[] {"*"}, new[] {"*"}, false, false, new uint[0], 100, 0));

			_service = new AMPService();
			LogDataSet(_service.GetNameFromCatalog(new String[0], new String[0], false, false, new uint[] {5}, 100, 0));

			_service = new AMPService();
			LogDataSet(_service.GetNameFromCatalog(new[] {"5*"}, new[] {"*"}, true, false, new uint[0], 100, 0));


			_service = new AMPService();
			LogDataSet(_service.GetNameFromCatalog(null, null, false, true, null, 100, 0));

			_service = new AMPService();
			LogDataSet(_service.GetNameFromCatalog(null, null, false, true, new uint[0], 100, 0));

			_service = new AMPService();
			LogDataSet(_service.GetNameFromCatalog(new[] {"5*"}, new[] {"*"}, false, true, new uint[0], 100, 0));

			_service = new AMPService();
			LogDataSet(_service.GetNameFromCatalog(new String[0], new String[0], false, true, new uint[] {5}, 100, 0));

			_service = new AMPService();
			LogDataSet(_service.GetNameFromCatalog(new[] {"5*"}, new[] {"*"}, true, true, new uint[0], 100, 0));
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
	}
}
