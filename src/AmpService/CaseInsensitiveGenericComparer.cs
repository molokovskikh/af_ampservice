using System;
using System.Collections.Generic;

namespace AmpService
{
	public class CaseInsensitiveStringComparer : IEqualityComparer<string>
	{
		public bool Equals(string x, string y)
		{
			return String.Compare(x, y, true) == 0;
		}

		public int GetHashCode(string obj)
		{
			return base.GetHashCode();
		}
	}
}