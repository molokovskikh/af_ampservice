using System;
using AmpService;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework.Config;
using Common.Models;
using Common.Service;
using NUnit.Framework;
using Test.Support;

namespace Integration
{
	[SetUpFixture]
	public class Setup
	{
		public Setup()
		{
			Global.Initialize();
			ServiceContext.GetHost = () => "localhost";
			ServiceContext.GetUserName = () => {
				throw new Exception("Тесты не инициализированы");
			};
			ActiveRecordStarter.Initialize(
				new[] { typeof(TestClient).Assembly },
				ActiveRecordSectionHandler.Instance);
		}
	}
}
