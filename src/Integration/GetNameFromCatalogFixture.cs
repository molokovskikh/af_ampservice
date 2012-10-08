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

			var supplier = TestSupplier.Create();

			var core = new TestCore {
				Price = supplier.Prices[0],
				Product = product,
				Quantity = "2",
				Code = "12352",
				Period = "10.01.2015"
			};

			Save(core);
		}

		[Test]
		public void GetNameFromCatalogWithMnn()
		{
			var data = service.GetNameFromCatalog(null, null, false, false, null, 5, 0, new uint[] { 1 });
			Assert.That(data.Tables[0].Rows.Count > 0);
		}
	}
}
