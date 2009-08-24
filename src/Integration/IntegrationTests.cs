using System;
using System.Data;
using NUnit.Framework;

namespace Integration
{
	[TestFixture]
	public class IntegrationTests : IntegrationFixture
	{
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

		[Test]
		public void GetPriceCodeByNameTest()
		{
			LogDataSet(service.GetPriceCodeByName(null));
			LogDataSet(service.GetPriceCodeByName(new[] { "Протек-15" }));
			LogDataSet(service.GetPriceCodeByName(new[] { "Протек-15", "Материа Медика" }));
			LogDataSet(service.GetPriceCodeByName(new[] { "Протек*" }));
			LogDataSet(service.GetPriceCodeByName(new[] { "*к*", "Ма*риа Ме*ка" }));
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