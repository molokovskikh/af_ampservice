using System;
using NUnit.Framework;

namespace Integration
{
	[TestFixture]
	public class IntegrationTests : IntegrationFixture
	{
		[Test]
		public void GetNameFromCatalogTest()
		{
			service.GetNameFromCatalogWithMnn(null, null, false, false, null, 100, 0, null, null);
			service.GetNameFromCatalogWithMnn(null, null, false, false, new uint[0], 100, 0, null, null);
			service.GetNameFromCatalogWithMnn(new[] { "*" }, new[] { "*" }, false, false, new uint[0], 100, 0, null, null);
			service.GetNameFromCatalogWithMnn(new String[0], new String[0], false, false, new uint[] { 5 }, 100, 0, null, null);
			service.GetNameFromCatalogWithMnn(new[] { "5*" }, new[] { "*" }, true, false, new uint[0], 100, 0, null, null);
			service.GetNameFromCatalogWithMnn(null, null, false, true, null, 100, 0, null, null);
			service.GetNameFromCatalog(null, null, false, true, new uint[0], 100, 0);
			service.GetNameFromCatalog(new[] { "5*" }, new[] { "*" }, false, true, new uint[0], 100, 0);
			service.GetNameFromCatalog(new String[0], new String[0], false, true, new uint[] { 5 }, 100, 0);
			service.GetNameFromCatalog(new[] { "5*" }, new[] { "*" }, true, true, new uint[0], 100, 0);
		}

		[Test]
		public void GetPriceCodeByNameTest()
		{
			var data = service.GetPriceCodeByName(null);
			Assert.That(data.Tables[0].Rows.Count, Is.GreaterThan(0));
			service.GetPriceCodeByName(new[] { "Протек-15" });
			service.GetPriceCodeByName(new[] { "Протек-15", "Материа Медика" });
			service.GetPriceCodeByName(new[] { "Протек*" });
			service.GetPriceCodeByName(new[] { "*к*", "Ма*риа Ме*ка" });
		}

		[Test]
		public void Get_price_code_by_name_should_contains_min_req_info()
		{
			var data = service.GetPriceCodeByName(null);
			Assert.That(data.Tables[0].Columns.Contains("MinReq"), "MinReq отсутствует");
		}

		[Test]
		public void Get_categoris()
		{
			var data = service.GetCategories();
			var dataTable = data.Tables[0];
			Assert.AreEqual("Id", dataTable.Columns[0].ColumnName);
			Assert.AreEqual("Name", dataTable.Columns[1].ColumnName);
			Assert.That(dataTable.Rows.Count, Is.GreaterThan(0));
		}
	}
}