using System;
using System.Data;
using log4net.Config;
using NUnit.Framework;

namespace AmpService.Tests
{
	[TestFixture]
	public class AmpServiceFixture
	{
		static AmpServiceFixture()
		{
			XmlConfigurator.Configure();
		}

		[Test]
		public void GetPricesTest()
		{
			var WebServ = new AMPService();
			LogDataSet(WebServ.GetPrices(false, false,
			                             new[]
			                             	{
			                             		"prepCode", "PriceCode", "PriceCode", "PrepCode", "ItemID", "PrepCode", "PrepCode",
			                             		"OriginalName", "ItemID", "PriceCode"
			                             	},
			                             new[] {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10"}, null, null, 100, 0));


			LogDataSet(WebServ.GetPrices(false, false, new[] {"OriginalName"}, new[] {"*а*"}, null, null, 100, 0));


			LogDataSet(WebServ.GetPrices(false, false, new[] {"OriginalName", "OriginalName"}, new[] {"к*", "т*"}, null,
			                             null, 100, 0));


			WebServ.GetPrices(false, false, new[] {"OriginalName", "OriginalName"}, new[] {"к*", "т*"},
			                  new[] {"OrderID"}, null, 100, 0);


			WebServ.GetPrices(false, false, new[] {"OriginalName", "OriginalName"}, new[] {"к*", "т*"},
			                  new[] {"OrderID"}, new[] {"DESC"}, 100, 0);


			WebServ.GetPrices(false, false, new[] {"OriginalName", "originalName"}, new[] {"к*", "т*"},
			                  new[] {"orderID", "unit", "Volume"}, new[] {"DESC"}, 100, 0);


			WebServ.GetPrices(true, false, new[] {"PrepCode", "PrepCode", "PrepCode"},
			                  new[] {"12999", "12998", "29652"}, null, null, -1, -1);


			WebServ.GetPrices(true, false, new[] {"PrepCode", "PrepCode", "PrepCode"},
			                  new[] {"12999", "12998", "29652"}, null, null, 2, 1);


			WebServ.GetPrices(true, false, new[] {"PrepCode", "PrepCode", "PrepCode"},
			                  new[] {"12999", "12998", "29652"}, new[] {"OrderID", "unit", "Volume"},
			                  new[] {"DESC"}, -1, -1);


			WebServ.GetPrices(true, false, new[] {"PrepCode", "PrepCode", "PrepCode"},
			                  new[] {"12999", "12998", "29652"}, new[] {"OrderID", "unit", "Volume"},
			                  new[] {"ASC"}, -1, 1);


			WebServ.GetPrices(false, false, new[] {"PrepCode"}, new[] {"5"}, new[] {"Cost"},
			                  new[] {"ASC"}, 1000, 0);
		}

		[Test]
		public void GetOrdersTest()
		{
			var WebServ = new AMPService();
			LogDataSet(WebServ.GetOrders(null, 0));
			LogDataSet(WebServ.GetOrders(new string[0], 0));
			LogDataSet(WebServ.GetOrders(new[] {"1"}, 2));
			LogDataSet(WebServ.GetOrders(new[] {"1", "2", "3"}, 2));
			LogDataSet(WebServ.GetOrders(new[] {"0"}, -1));
			LogDataSet(WebServ.GetOrders(new[] {"!1"}, -1));
		}

		[Test]
		public void GetPriceCodeByNameTest()
		{
			var WebServ = new AMPService();
			LogDataSet(WebServ.GetPriceCodeByName(null));
			LogDataSet(WebServ.GetPriceCodeByName(new[] {"Протек-15"}));
			LogDataSet(WebServ.GetPriceCodeByName(new[] {"Протек-15", "Материа Медика"}));
			LogDataSet(WebServ.GetPriceCodeByName(new[] {"Протек*"}));
			LogDataSet(WebServ.GetPriceCodeByName(new[] {"*к*", "Ма*риа Ме*ка"}));
		}

		[Test]
		public void PostOrderTest()
		{
			var ampService = new AMPService();
			LogDataSet(ampService.PostOrder(new long[] {838566976, 838566968, 838566969},
			                                new[] {1, 1, 1},
			                                new[] {"это тестовый заказ"},
			                                new[] {1503908, 1256924, 1503905},
			                                new[] {156745, 156745, 156745},
			                                new[] {false, false, false}));
		}

		[Test]
		public void GetNameFromCatalogTest()
		{
			var WebServ = new AMPService();
			LogDataSet(WebServ.GetNameFromCatalog(null, null, false, false, null, 100, 0));

			WebServ = new AMPService();
			LogDataSet(WebServ.GetNameFromCatalog(null, null, false, false, new uint[0], 100, 0));

			WebServ = new AMPService();
			LogDataSet(WebServ.GetNameFromCatalog(new[] {"*"}, new[] {"*"}, false, false, new uint[0], 100, 0));

			WebServ = new AMPService();
			LogDataSet(WebServ.GetNameFromCatalog(new String[0], new String[0], false, false, new uint[] {5}, 100, 0));

			WebServ = new AMPService();
			LogDataSet(WebServ.GetNameFromCatalog(new[] {"5*"}, new[] {"*"}, true, false, new uint[0], 100, 0));


			WebServ = new AMPService();
			LogDataSet(WebServ.GetNameFromCatalog(null, null, false, true, null, 100, 0));

			WebServ = new AMPService();
			LogDataSet(WebServ.GetNameFromCatalog(null, null, false, true, new uint[0], 100, 0));

			WebServ = new AMPService();
			LogDataSet(WebServ.GetNameFromCatalog(new[] {"5*"}, new[] {"*"}, false, true, new uint[0], 100, 0));

			WebServ = new AMPService();
			LogDataSet(WebServ.GetNameFromCatalog(new String[0], new String[0], false, true, new uint[] {5}, 100, 0));

			WebServ = new AMPService();
			LogDataSet(WebServ.GetNameFromCatalog(new[] {"5*"}, new[] {"*"}, true, true, new uint[0], 100, 0));
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
