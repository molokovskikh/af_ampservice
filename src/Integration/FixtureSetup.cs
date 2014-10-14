using System;
using AmpService;
using Castle.ActiveRecord;
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
			Test.Support.Setup.BuildConfiguration("local");
			var holder = ActiveRecordMediator.GetSessionFactoryHolder();
			var cfg = holder.GetConfiguration(typeof(ActiveRecordBase));
			Global.Map(cfg);
			var factory = holder.GetSessionFactory(typeof(ActiveRecordBase));
			Global.Initialize(factory);

			ServiceContext.GetHost = () => "localhost";
			ServiceContext.GetUserName = () => { throw new Exception("Тесты не инициализированы"); };
		}

		[TearDown]
		public void TearDown()
		{
			Global.Deinitialize();
		}
	}
}