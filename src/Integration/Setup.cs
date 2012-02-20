using System;
using AmpService;
using Common.MySql;
using Common.Service;
using NUnit.Framework;

namespace Integration
{
	[SetUpFixture]
	public class Setup
	{
		public Setup()
		{
			if (ConnectionHelper.IsIntegration())
				Global.Initialize("integration");
			else 
				Global.Initialize();
			ServiceContext.GetHost = () => "localhost";
			ServiceContext.GetUserName = () => {
				throw new Exception("Тесты не инициализированы");
			};

			Test.Support.Setup.Initialize();
		}
	}
}
