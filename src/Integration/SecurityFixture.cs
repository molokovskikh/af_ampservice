using Common.Models;
using NUnit.Framework;

namespace Integration
{
	[TestFixture]
	public class SecurityFixture
	{
		private AmpService.AmpService service;

		[SetUp]
		public void Setup()
		{
			service = new AmpService.AmpService();
		}

		[Test]
		public void Every_method_should_return_null_if_user_not_have_iol_permission()
		{
			Execute(@"
delete ap from AssignedPermissions ap, osuseraccessright oar, userpermissions up
where ap.permissionid = up.id and oar.rowid = ap.userid and oar.osusername = 'kvasov' and up.shortcut = 'IOL';");
			Assert.That(service.GetNameFromCatalog(new[] {""},
			                                       new string[] {},
			                                       false,
			                                       false,
			                                       new uint[] {}, 100, 0),
			            Is.Null);

			Assert.That(service.GetPriceCodeByName(new[] {"%протек%"}),
			            Is.Null);

			Assert.That(service.GetPrices(false, false,
			                              new[] {"OriginalName"},
			                              new[] {"%папа%"},
			                              new string[] {},
			                              new string[] {}, 100, 0),
			            Is.Null);

			Assert.That(service.PostOrder(new[] {54621354879ul},
			                              new[] {1u},
			                              new[] {"123"},
			                              new[] {46528u},
			                              new[] {544523u},
			                              new[] {false}), Is.Null);
		}

		public void Execute(string command)
		{
			With.Session(s => s.CreateSQLQuery(command).ExecuteUpdate());
		}
	}
}