using System;
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using Test.Support;
using Test.Support.Suppliers;

namespace Integration
{
	public class GetNameFromCatalogFixture : IntegrationFixture
	{
		[SetUp]
		public void Setup()
		{
			var product = new TestProduct();

			product.CatalogProduct = new TestCatalogProduct {
				Name = "тестовый продукт из каталога"
			};
			product.CatalogProduct.CatalogForm = new TestCatalogForm {
				Form = "Тестовая форма"
			};
			product.CatalogProduct.CatalogName = new TestCatalogName {
				Name = "Тестовое наименование",
				Mnn = session.Query<TestMnn>().First(),
			};

			session.Save(product);
		}

		[Test]
		public void GetNameFromCatalogWithMnn()
		{
			var data = service.GetNameFromCatalogWithMnn(new[] { "Тестовое наименование" }, null, false, false, null, 5, 0, new[] { "*препарат" }, null);
			var table = data.Tables[0];
			Assert.That(table.Rows.Count > 0);
			Assert.That(table.Columns.Contains("MnnId"));
			Assert.That(table.Columns.Contains("VitallyImportant"), Is.True, "нет VitallyImportant");
			Assert.That(table.Columns.Contains("Mnn"), Is.True, "нет мнн");
		}

		[Test]
		public void GetNameFromCatalogWithProperty()
		{
			var data = service.GetNameFromCatalogWithMnn(null, null, false, false, null, 5, 0, null, new[] { "*та*" });
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
				Mnn = session.Query<TestMnn>().First(),
			};
			session.Save(product);

			session.Transaction.Commit();
			var data = service.GetNameFromCatalogWithMnn(new[] { "'Тестовое наименование" },
				new[] { "тестовая форма'" }, false, false, null, 5, 0, null, null);
			Assert.That(data.Tables[0].Rows.Count > 0);
		}

		[Test]
		public void Get_orders()
		{
			var supplier = TestSupplier.CreateNaked(session);
			supplier.CreateSampleCore(session);
			var offer = supplier.Prices[0].Core[0];
			testClient.MaintainIntersection();

			service.PostOrder(new[] { offer.Id },
				new uint[] { 1 },
				new[] { "" },
				new[] { offer.ProducerSynonym.Id },
				new[] { offer.ProductSynonym.Id },
				new[] { false });

			var order = session.Query<TestOrder>().First(o => o.Client == testClient);
			order.WriteTime = order.WriteTime.AddHours(-1);
			session.Save(order);
			session.Flush();

			var lines = service.GetOrderItems(DateTime.Now.AddDays(-1), DateTime.Now.AddMinutes(-30));
			var row = lines.Tables[0].Rows[0];
			Assert.AreEqual(1, lines.Tables[0].Rows.Count);
			Assert.IsInstanceOf<string>(row["WriteTime"]);
			Assert.IsInstanceOf<string>(row["WriteDate"]);
		}
	}
}