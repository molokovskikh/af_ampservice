using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AmpService
{
	public static class DataRowExtentions
	{
		public static void CopyTo(this DataRow source, DataRow dest)
		{
			var destTable = dest.Table;
			var sourceTable = source.Table;
			if (destTable.Columns.Count < sourceTable.Columns.Count)
			{
				foreach (DataColumn column in sourceTable.Columns)
				{
					if (!destTable.Columns.Contains(column.ColumnName))
						destTable.Columns.Add(column.ColumnName, column.DataType);
				}
			}

			foreach (DataColumn column in sourceTable.Columns)
				dest[column.ColumnName] = source[column];
		}
	}

	public class WhereBlockBuilder
	{
		private readonly StringBuilder _whereBlock;
		private bool _isAdd;

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
			if (criteriaString == null
				|| criteriaString.Trim() == "")
				return this;
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

	/// <summary>
	/// ����� �������� ��������������� ������� ��� ��������� SQL ��������
	/// </summary>
	public class Utils
	{
		/// <summary>
		/// ��������������� ������ ����� ��� ������������� � �������.
		/// ����� ���������� ������ ������� "*" �� "%".
		/// ������: ������ {"hello", "w*rld"} ��������������� � ������ 
		/// "and ( someField like 'hello'  or  someField like 'w%rld')"
		/// </summary>
		/// <param name="array">��������� ������</param>
		/// <param name="fieldName">��� ���� �� �������� ���������� �����</param>
		/// <returns>����������������� ������</returns>
		public static string StringArrayToQuery<T>(IEnumerable<T> array, string fieldName)
		{
			var builder = new StringBuilder();
			var index = 0;
			if (array != null && array.Count() > 0)
			{
				builder.Append(" (");
				foreach (var item in array)
				{
					var value = item.ToString();
					if (value.IndexOf("*") > -1)
						builder.Append(fieldName + " like '" + value.Replace("*", "%") + "'");
					else
						builder.Append(fieldName + " = '" + value + "'");
					builder.Append(" or ");
					index++;
				}
				builder.Remove(builder.Length - 4, 4);
				builder.Append(") ");
			}
			if (index > 0)
				return builder.ToString();

			return "";
		}
		/// <summary>
		/// ��������������� ������ ����� ��� ������������� � ���� ���������� SQL �������.
		/// ������: ������ orderFields = {"field1", "field2"}, orderDirection = {"ASC"}
		/// ������������� � ���� " ORDER BY field1 ASC, field2 ".
		/// </summary>
		/// <param name="orderFields">������ ����� �� ������� ����������� ���� ����������.</param>
		/// <param name="orderDirection">������ ����������� ���������� ��� orderFields.
		/// ���������� ��������: null, "ASC", "DESC".
		/// ����������: ������ ������ orderDirection ����� ���� ������ ��� ����� ������ orderFields.
		/// </param>
		/// <returns></returns>
		public static string FormatOrderBlock(IList<string> orderFields, IList<string> orderDirection)
		{
			var builder = new StringBuilder();
			if ((orderFields != null) && (orderFields.Count > 0))
			{
				builder.Append(" ORDER BY ");
				for (var i = 0; i < orderFields.Count; i++)
				{
					var direction = String.Empty;
					if ((orderDirection != null) && (i < orderDirection.Count))
						direction = orderDirection[i];
					builder.Append(orderFields[i] + " " + direction + ", ");
				}
				builder.Remove(builder.Length - 2, 2);
			}
			return builder.ToString();
		}
		
		/// <summary>
		/// ��������� ���� LIMIT ��� SQL �������. ������: "LIMIT 1 20"
		/// </summary>
		/// <param name="offset">������� � ������ ��������</param>
		/// <param name="count">���������� ���������</param>
		/// <returns></returns>
		public static string GetLimitString(uint offset, uint count)
		{
			var result = String.Empty;
			if (offset >= 0)
			{
				result = " limit " + offset;
				if (count > 0)
					result += "," + count;
			}

			return result + ";";
		}
	}
}