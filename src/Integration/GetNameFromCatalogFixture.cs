﻿using System;
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
			var data = service.GetNameFromCatalog(new string[] {"Тестовое наименование"}, null, false, false, null, 5, 0);
			Assert.That(data.Tables[0].Rows.Count > 0);
			Assert.That(data.Tables[0].Columns.Contains("MnnId"));
		}
	}
}
