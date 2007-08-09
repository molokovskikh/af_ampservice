using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace AmpService
{

	public class Literals
	{
		public static string ConnectionString
		{
			get { return Convert.ToString(ConfigurationManager.ConnectionStrings["DB"]); }
		}

	}
}
