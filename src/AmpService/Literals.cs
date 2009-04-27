using System;
using System.Configuration;

namespace AmpService
{
	public class Literals
	{
		public static string ConnectionString
		{
			get { return Convert.ToString(ConfigurationManager.ConnectionStrings["Main"]); }
		}
	}
}
