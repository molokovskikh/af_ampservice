using System.Linq;
using NUnit.Framework;

namespace AmpService.Tests
{
	[TestFixture]
	public class ServiceFixture
	{
		[Test]
		public void Check_methods()
		{
			//все методы должны быть виртуальными
			//что бы работали перехватчики вызвовов
			var methods = typeof(Service).GetMethods().Where(m => m.DeclaringType == typeof(Service));

			foreach (var methodInfo in methods)
				Assert.IsTrue(methodInfo.IsVirtual, methodInfo.Name);
		}
	}
}