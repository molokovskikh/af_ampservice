﻿using System;
using System.Data;
using System.Linq;
using Common.Models;
using Common.Models.Repositories;
using Common.NHibernate;
using Common.Service;
using Common.Service.Models;
using Common.Tools;
using NHibernate.Linq;
using NUnit.Framework;
using Test.Support;

namespace Integration
{
	[TestFixture]
	public class GetPricesFixture : IntegrationFixture
	{
		[Test]
		public void Get_prices_should_return_offer_check_fields()
		{
			var offers = service.GetPrices(false, false, new[] { "OriginalName" }, new[] { "*папа*" }, null, null, 100, 0);
			Assert.That(offers.Tables[0].Columns.Contains("RequestRatio"));
			Assert.That(offers.Tables[0].Columns.Contains("MinOrderSum"));
			Assert.That(offers.Tables[0].Columns.Contains("MinOrderCount"));
		}

		[Test]
		public void GetPricesTest()
		{
			var data = service.GetPrices(false, false,
				new[] {
					"prepCode", "PriceCode", "PriceCode", "PrepCode", "ItemID", "PrepCode", "PrepCode",
					"OriginalName", "ItemID", "PriceCode"
				},
				new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" }, null, null, 100, 0);
			Assert.That(data, Is.Not.Null);

			data = service.GetPrices(false, false, new[] { "OriginalName" }, new[] { "*а*" }, null, null, 100, 0);
			Assert.That(data, Is.Not.Null);

			service.GetPrices(false, false, new[] { "OriginalName", "OriginalName" }, new[] { "к*", "т*" }, null, null, 100, 0);
			Assert.That(data, Is.Not.Null);

			service.GetPrices(false, false, new[] { "OriginalName", "OriginalName" }, new[] { "к*", "т*" }, new[] { "OrderID" }, null, 100, 0);
			Assert.That(data, Is.Not.Null);

			service.GetPrices(false, false, new[] { "OriginalName", "OriginalName" }, new[] { "к*", "т*" }, new[] { "OrderID" }, new[] { "DESC" }, 100, 0);
			Assert.That(data, Is.Not.Null);

			service.GetPrices(false, false, new[] { "OriginalName", "originalName" }, new[] { "к*", "т*" }, new[] { "orderID", "unit", "Volume" }, new[] { "DESC" }, 100, 0);
			Assert.That(data, Is.Not.Null);

			service.GetPrices(true, false, new[] { "PrepCode", "PrepCode", "PrepCode" }, new[] { "12999", "12998", "29652" }, null, null, 2, 1);
			Assert.That(data, Is.Not.Null);

			service.GetPrices(false, false, new[] { "PrepCode" }, new[] { "5" }, new[] { "Cost" }, new[] { "ASC" }, 1000, 0);
			Assert.That(data, Is.Not.Null);
		}

		[Test]
		public void GetPriceWithMnn()
		{
			var data = service.GetPrices(true, false, new[] { "MnnId", "MnnId" }, new[] { "1", "2" }, null, null, 2, 1);
			Assert.That(data, Is.Not.Null);
			data = service.GetPrices(false, false, new[] { "MnnId", "MnnId" }, new[] { "1", "2" }, null, null, 2, 1);
			Assert.That(data, Is.Not.Null);
		}

		[Test]
		public void Calculate_row_count()
		{
			var toDelete = session.Query<ServiceLogEntity>()
				.Where(e => e.UserName == testUser.Login && e.MethodName == "GetPrices")
				.ToList();
			session.DeleteEach(toDelete);

			var begin = DateTime.Now;
			var data = service.GetPrices(false, false, new[] { "OriginalName" }, new[] { "*" }, new[] { "OriginalName" }, new[] { "asc" }, 1000, 0);
			Assert.That(data.Tables[0].Rows.Count, Is.GreaterThan(0));
			var productCount = data.Tables[0].Rows.Cast<DataRow>().GroupBy(r => r["PrepCode"]).Count();
			var entity = session.Query<ServiceLogEntity>()
				.First(e => e.MethodName == "GetPrices" && e.LogTime >= begin && e.UserName == testUser.Login);
			Assert.That(entity.RowCount, Is.EqualTo(productCount));
		}
	}
}