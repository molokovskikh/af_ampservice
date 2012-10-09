using System;
using System.Data;
using Common.MySql;
using NUnit.Framework;
using Test.Support;

namespace Integration
{
	[TestFixture]
	public class IntegrationTests : IntegrationFixture
	{
		[Test]
		public void GetNameFromCatalogWithMnn()
		{
			var catalog = new TestProduct();
			
			catalog.CatalogProduct = new TestCatalogProduct {
				Name = "тестовый продукт из каталога"
			};

		}

		[Test]
		public void GetNameFromCatalogTest()
		{
			service.GetNameFromCatalog(null, null, false, false, null, 100, 0, null, null);
			service.GetNameFromCatalog(null, null, false, false, new uint[0], 100, 0, null, null);
			service.GetNameFromCatalog(new[] { "*" }, new[] { "*" }, false, false, new uint[0], 100, 0, null, null);
			service.GetNameFromCatalog(new String[0], new String[0], false, false, new uint[] { 5 }, 100, 0, null, null);
			service.GetNameFromCatalog(new[] { "5*" }, new[] { "*" }, true, false, new uint[0], 100, 0, null, null);
			service.GetNameFromCatalog(null, null, false, true, null, 100, 0, null, null);
			service.GetNameFromCatalog(null, null, false, true, new uint[0], 100, 0, null, null);
			service.GetNameFromCatalog(new[] { "5*" }, new[] { "*" }, false, true, new uint[0], 100, 0, null, null);
			service.GetNameFromCatalog(new String[0], new String[0], false, true, new uint[] { 5 }, 100, 0, null, null);
			service.GetNameFromCatalog(new[] { "5*" }, new[] { "*" }, true, true, new uint[0], 100, 0, null, null);
		}

		[Test]
		public void GetPriceCodeByNameTest()
		{
			service.GetPriceCodeByName(null);
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
		public void Get_name_from_catalog_for_future_client()
		{
			var data = service.GetNameFromCatalog(new[] { "5*" }, new[] { "*" }, true, true, new uint[0], 100, 0, null, null);
			Assert.That(data.Tables[0].Rows.Count, Is.GreaterThan(0));
			Assert.That(data.Tables[0].Columns.Contains("VitallyImportant"), Is.True, "нет VitallyImportant");
			Assert.That(data.Tables[0].Columns.Contains("Mnn"), Is.True, "нет мнн");
		}

	}
}