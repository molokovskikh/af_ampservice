using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using NUnit.Framework;

namespace AmpService.Tests
{
	[TestFixture]
	public class AmpServiceFixture
	{
		[Test]
		public void GetPricesTest()
		{
			
			AMPService WebServ = new AMPService();
			WebServ.GetPrices(false, false,
			                  new String[]
			                  	{
			                  		"prepCode", "PriceCode", "PriceCode", "PrepCode", "ItemID", "PrepCode", "PrepCode",
			                  		"OriginalName", "ItemID", "PriceCode"
			                  	},
			                  new String[] {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10"}, null, null, 100, 0);


			WebServ.GetPrices(false, false, new String[] {"OriginalName"}, new String[] {"*а*"}, null, null, 100, 0);


			WebServ.GetPrices(false, false, new String[] {"OriginalName", "OriginalName"}, new String[] {"к*", "т*"}, null,
			                  null, 100, 0);


			WebServ.GetPrices(false, false, new String[] {"OriginalName", "OriginalName"}, new String[] {"к*", "т*"},
			                  new String[] {"OrderID"}, null, 100, 0);


			WebServ.GetPrices(false, false, new String[] {"OriginalName", "OriginalName"}, new String[] {"к*", "т*"},
			                  new String[] {"OrderID"}, new String[] {"DESC"}, 100, 0);


			WebServ.GetPrices(false, false, new String[] {"OriginalName", "originalName"}, new String[] {"к*", "т*"},
			                  new String[] {"orderID", "unit", "Volume"}, new String[] {"DESC"}, 100, 0);


			WebServ.GetPrices(true, false, new String[] {"PrepCode", "PrepCode", "PrepCode"},
			                  new String[] {"12999", "12998", "29652"}, null, null, -1, -1);


			WebServ.GetPrices(true, false, new String[] {"PrepCode", "PrepCode", "PrepCode"},
			                  new String[] {"12999", "12998", "29652"}, null, null, 2, 1);


			WebServ.GetPrices(true, false, new String[] {"PrepCode", "PrepCode", "PrepCode"},
			                  new String[] {"12999", "12998", "29652"}, new String[] {"OrderID", "unit", "Volume"},
			                  new String[] {"DESC"}, -1, -1);


			WebServ.GetPrices(true, false, new String[] {"PrepCode", "PrepCode", "PrepCode"},
			                  new String[] {"12999", "12998", "29652"}, new String[] {"OrderID", "unit", "Volume"},
			                  new String[] {"ASC"}, -1, 1);


			WebServ.GetPrices(false, false, new String[] {"PrepCode"}, new String[] {"5"}, new String[] {"Cost"},
			                  new String[] {"ASC"}, 1000, 0);
		}

		[Test]
		public void GetOrdersTest()
		{
			AMPService WebServ = new AMPService();
			WebServ.GetOrders(null, 0);
			WebServ.GetOrders(new string[0], 0);
			WebServ.GetOrders(new String[] {"1"}, 2);
			WebServ.GetOrders(new String[] {"1", "2", "3"}, 2);
			WebServ.GetOrders(new String[] {"0"}, -1);
			WebServ.GetOrders(new String[] {"!1"}, -1);
		}

		[Test]
		public void GetPriceCodeByNameTest()
		{
			AMPService WebServ = new AMPService();
			WebServ.GetPriceCodeByName(null);
			WebServ.GetPriceCodeByName(new String[] {"Протек-15"});
			WebServ.GetPriceCodeByName(new String[] {"Протек-15", "Материа Медика"});
			WebServ.GetPriceCodeByName(new String[] {"Протек*"});
			WebServ.GetPriceCodeByName(new String[] {"*к*", "Ма*риа Ме*ка"});
		}

		[Test]
		public void PostOrderTest()
		{
			AMPService ampService = new AMPService();
			ampService.PostOrder(new long[] { 838566976, 838566968, 838566969 },
			                     new int[] {1, 1, 1},
			                     new string[] {"это тестовый заказ"},
								 new int[] { 1503908, 1256924, 1503905 },
								 new int[] { 156745, 156745, 156745 },
			                     new bool[] {false, false, false});
		}

		[Test]
		public void GetNameFromCatalogTest()
		{
			AMPService WebServ = new AMPService();
			LogDataSet(WebServ.GetNameFromCatalog(null, null, false, false, null, 100, 0));

			WebServ = new AMPService();
			LogDataSet(WebServ.GetNameFromCatalog(null, null, false, false, new uint[0], 100, 0));

			WebServ = new AMPService();
			LogDataSet(WebServ.GetNameFromCatalog(new String[] { "*" }, new String[] { "*" }, false, false, new uint[0], 100, 0));

			WebServ = new AMPService();
			LogDataSet(WebServ.GetNameFromCatalog(new String[0], new String[0], false, false, new uint[] { 5 }, 100, 0));

			WebServ = new AMPService();
			LogDataSet(WebServ.GetNameFromCatalog(new String[] { "5*" }, new String[] { "*" }, true, false, new uint[0], 100, 0));


			WebServ = new AMPService();
			LogDataSet(WebServ.GetNameFromCatalog(null, null, false, true, null, 100, 0));

			WebServ = new AMPService();
			LogDataSet(WebServ.GetNameFromCatalog(null, null, false, true, new uint[0], 100, 0));

			WebServ = new AMPService();
			LogDataSet(WebServ.GetNameFromCatalog(new String[] { "5*" }, new String[] { "*" }, false, true, new uint[0], 100, 0));

			WebServ = new AMPService();
			LogDataSet(WebServ.GetNameFromCatalog(new String[0], new String[0], false, true, new uint[] { 5 }, 100, 0));

			WebServ = new AMPService();
			LogDataSet(WebServ.GetNameFromCatalog(new String[] { "5*" }, new String[] { "*" }, true, true, new uint[0], 100, 0));
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
