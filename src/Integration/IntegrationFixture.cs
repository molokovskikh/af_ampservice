using System.Linq;
using AmpService;
using Castle.ActiveRecord;
using Common.Models;
using Common.Service;
using NUnit.Framework;
using Test.Support;
using Test.Support.Suppliers;

namespace Integration
{
	public class IntegrationFixture
	{
		protected AmpService.AmpService service;
		protected TestClient testClient;
		protected TestUser testUser;
		public static bool OffersCreated = false;
		public static uint PriceId;

		[SetUp]
		public void Setup()
		{
			if (!OffersCreated) {
				using(new SessionScope()) {
					var supplier = TestSupplier.CreateNaked();
					Service.SupplierIds = new[] { supplier.Id };
					PriceId = supplier.Prices[0].Id;
					supplier.CreateSampleCore();
				}
				OffersCreated = true;
			}

			testClient = TestClient.Create();
			testUser = testClient.Users.First();
			ServiceContext.GetUserName = () => testUser.Login;
			service = new AmpService.AmpService();
		}

		public void Execute(string command)
		{
			With.Session(s => s.CreateSQLQuery(command).ExecuteUpdate());
		}
	}
}