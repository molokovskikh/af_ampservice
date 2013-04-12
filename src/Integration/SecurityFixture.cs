using System.ServiceModel;
using Castle.ActiveRecord;
using Common.Service;
using NUnit.Framework;

namespace Integration
{
	[TestFixture]
	public class SecurityFixture : IntegrationFixture
	{
		[Test]
		public void Every_method_should_return_null_if_user_not_have_iol_permission()
		{
			using (new SessionScope()) {
				testUser.AssignedPermissions.Clear();
				testUser.Save();
			}

			Assert.Throws<FaultException<DoNotHavePermissionFault>>(() => service.GetNameFromCatalogWithMnn(new[] { "" },
				new string[] { },
				false,
				false,
				new uint[] { }, 100, 0, null, null));
		}
	}
}