using System.Linq;
using Common.Models;
using Common.Service;
using NUnit.Framework;
using Test.Support;

namespace Integration
{
	public class IntegrationFixture
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
		}

		public void Execute(string command)
		{
			With.Session(s => s.CreateSQLQuery(command).ExecuteUpdate());
		}
	}
}