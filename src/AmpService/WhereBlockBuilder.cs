using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

namespace AmpService
{
	public class WhereBlockBuilder
	{
		private StringBuilder _whereBlock;
		private bool _isAdd = false;

		protected WhereBlockBuilder(StringBuilder commandText)
		{
			_whereBlock = commandText;
		}

		public static WhereBlockBuilder ForCommandTest(StringBuilder commandText)
		{
			return new WhereBlockBuilder(commandText);
		}

		public WhereBlockBuilder AddCriteria(string criteriaString)
		{
			if (_isAdd)
				_whereBlock.Append(" and ");
			_whereBlock.AppendLine(criteriaString);
			_isAdd = true;
			return this;
		}

		public WhereBlockBuilder AddCriteria(string criteriaString, bool addOrNot)
		{
			if (addOrNot)
				AddCriteria(criteriaString);
			return this;
		}
	}
}
