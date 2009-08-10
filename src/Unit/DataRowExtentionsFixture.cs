using System.Data;
using NUnit.Framework;

namespace AmpService.Tests
{
	[TestFixture]
	public class DataRowExtentionsFixture
	{
		[Test]
		public void Copy_data_row_values()
		{
			var table = new DataTable();
			table.Columns.Add("c1", typeof (int));
			var table1 = new DataTable();
			table1.Columns.Add("c2", typeof (string));
			table1.Columns.Add("c3", typeof (int));
			var source = table1.NewRow();
			source["c2"] = "123";
			table1.Rows.Add(source);
			var dest = table.NewRow();
			table.Rows.Add(dest);
			source.CopyTo(dest);
			Assert.That(table.Rows.Count, Is.EqualTo(1));
			Assert.That(table.Rows[0]["c2"], Is.EqualTo("123"));
		}
	}
}
