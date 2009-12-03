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
			Execute(@"
delete ap from AssignedPermissions ap, osuseraccessright oar, userpermissions up
where ap.permissionid = up.id and oar.rowid = ap.userid and oar.osusername = 'kvasov' and up.shortcut = 'IOL';

insert into AssignedPermissions(userid, permissionid)
select oar.rowid, up.id
from osuseraccessright oar, userpermissions up
where oar.osusername = 'kvasov' and up.shortcut = 'IOL';");
			ServiceContext.GetHost = () => "localhost";
			ServiceContext.GetUserName = () => "kvasov";
			ActiveRecordStarter.Initialize(
				new[] { typeof(TestClient).Assembly },
				ActiveRecordSectionHandler.Instance);
		}

		public void Execute(string command)
		{
			With.Session(s => s.CreateSQLQuery(command).ExecuteUpdate());
		}
	}
}
