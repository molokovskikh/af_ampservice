using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for CaseInsensitiveGenericComparer
/// </summary>
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
