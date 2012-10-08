using Castle.ActiveRecord;
using NUnit.Framework;

namespace Integration
{
	[TestFixture]
	public class SecurityFixture : IntegrationFixture
	{
		[Test]
		public void Every_method_should_return_null_if_user_not_have_iol_permission()
		{
			using(new SessionScope()) {
				testUser.AssignedPermissions.Clear();
				testUser.Save();
			}

			Assert.That(service.GetNameFromCatalog(new[] {""},
				new string[] {},
				false,
				false,
				new uint[] {}, 100, 0, null),
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
				new[] {false}), 
				Is.Null);
		}
	}
}