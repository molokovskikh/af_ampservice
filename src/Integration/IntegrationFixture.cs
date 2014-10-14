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
	public class IntegrationFixture : Test.Support.IntegrationFixture
	{
		private AmpService.AmpService _service;
		protected TestClient testClient;
		protected TestUser testUser;
		public static bool OffersCreated = false;
		public static uint PriceId;

		[SetUp]
		public void Setup()
		{
			if (!OffersCreated) {
				var supplier = TestSupplier.CreateNaked(session);
				Service.SupplierIds = new[] { supplier.Id };
				PriceId = supplier.Prices[0].Id;
				supplier.CreateSampleCore();
				OffersCreated = true;
			}

			testClient = TestClient.CreateNaked(session);
			testUser = testClient.Users.First();
			ServiceContext.GetUserName = () => testUser.Login;
			_service = new AmpService.AmpService();
		}

		protected AmpService.AmpService service
		{
			get
			{
				if (session.Transaction.IsActive)
					session.Transaction.Commit();
				return _service;
			}
		}
	}
}