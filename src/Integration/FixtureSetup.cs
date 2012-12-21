using System;
using AmpService;
using Common.MySql;
using Common.Service;
using NUnit.Framework;

namespace Integration
{
	[SetUpFixture]
	public class FixtureSetup
	{
		[SetUp]
		public void Setup()
		{
			if (ConnectionHelper.IsIntegration())
				Global.Initialize("integration");
			else
				Global.Initialize();
			ServiceContext.GetHost = () => "localhost";
			ServiceContext.GetUserName = () => { throw new Exception("Тесты не инициализированы"); };

			Test.Support.Setup.Initialize();
		}

		[TearDown]
		public void TearDown()
		{
			Global.Deinitialize();
		}
	}
}