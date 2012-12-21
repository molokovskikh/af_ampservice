using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Service;
using NUnit.Framework;
using Test.Support;
using Test.Support.Suppliers;

namespace Integration
{
	public class GetNameFromCatalogFixture : Test.Support.IntegrationFixture
	{
		protected AmpService.AmpService service;
		protected TestClient testClient;
		protected TestUser testUser;

		[SetUp]
		public void Setup()
		{
			testClient = TestClient.Create();
			testUser = testClient.Users.First();
			ServiceContext.GetUserName = () => testUser.Login;
			service = new AmpService.AmpService();

			var product = new TestProduct();

			product.CatalogProduct = new TestCatalogProduct {
				Name = "тестовый продукт из каталога"
			};
			product.CatalogProduct.CatalogForm = new TestCatalogForm {
				Form = "Тестовая форма"
			};
			product.CatalogProduct.CatalogName = new TestCatalogName {
				Name = "Тестовое наименование",
				MnnId = 1,
			};

			Save(product);
		}

		[Test]
		public void GetNameFromCatalogWithMnn()
		{
			var data = service.GetNameFromCatalogWithMnn(new string[] { "Тестовое наименование" }, null, false, false, null, 5, 0, new string[] { "*препарат" }, null);
			Assert.That(data.Tables[0].Rows.Count > 0);
			Assert.That(data.Tables[0].Columns.Contains("MnnId"));
		}

		[Test]
		public void GetNameFromCatalogWithProperty()
		{
			var data = service.GetNameFromCatalogWithMnn(null, null, false, false, null, 5, 0, null, new string[] { "*та*" });
			Assert.That(data.Tables[0].Rows.Count > 0);
		}


		[Test]
		public void GetNameFromCatalogWithEscapeChar()
		{
			var product = new TestProduct();

			product.CatalogProduct = new TestCatalogProduct {
				Name = "тестовый продукт из каталога"
			};
			product.CatalogProduct.CatalogForm = new TestCatalogForm {
				Form = "Тестовая форма'"
			};
			product.CatalogProduct.CatalogName = new TestCatalogName {
				Name = "'Тестовое наименование",
				MnnId = 1,
			};
			Save(product);
			Reopen();
			var data = service.GetNameFromCatalogWithMnn(new string[] { "'Тестовое наименование" },
				new string[] { "тестовая форма'" }, false, false, null, 5, 0, null, null);
			Assert.That(data.Tables[0].Rows.Count > 0);
		}
	}
}