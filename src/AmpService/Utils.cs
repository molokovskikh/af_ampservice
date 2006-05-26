using System;
using System.Collections.Generic;
using System.Text;

namespace AMPWebService
{
	/// <summary>
	/// ����� �������� ��������������� ������� ��� ��������� SQL ��������
	/// </summary>
	public class Utils
	{
		/// <summary>
		/// ����������� ������ ��������������� ����� ����� ��� ���������� � �������.
		/// ������: ������ {1, 2} ���������� � ���� " and PricesData.PriceCode in (1, 2) "
		/// </summary>
		/// <param name="priceID">������ ���������������</param>
		/// <returns>����������������� ������</returns>
		public static string FormatPriceIDForQuery(params uint[] priceID)
		{
			string result = String.Empty;
			if (priceID != null && !(priceID.Length == 1 && priceID[0] == 0))
			{
				string ids = String.Empty;
				foreach (uint id in priceID)
					ids += Convert.ToString(id) + " , ";

				ids = ids.Remove(ids.Length - 3);
				result = String.Format(" and PricesData.PriceCode in ({0}) ", ids);
			}
			return result;
		}

		/// <summary>
		/// ��������������� ������ ����� ��� ������������� � �������.
		/// ����� ���������� ������ ������� "*" �� "%".
		/// ������: ������ {"hello", "w*rld"} ��������������� � ������ 
		/// "and ( someField like 'hello'  or  someField like 'w%rld')"
		/// </summary>
		/// <param name="array">��������� ������</param>
		/// <param name="fieldName">��� ���� �� �������� ���������� �����</param>
		/// <returns>����������������� ������</returns>
		public static string StringArrayToQuery(IList<string> array, string fieldName)
		{
			StringBuilder builder = new StringBuilder();
			if ((array != null) && (array.Count > 0))
			{
				builder.Append(" and (");
				foreach (string item in array)
				{
					if (item.IndexOf("*") > -1)
						builder.Append(fieldName + " like '" + item.Replace("*", "%") + "'");
					else
						builder.Append(fieldName + " = '" + item + "'");
					builder.Append(" or ");
				}
				builder.Remove(builder.Length - 4, 4);
				builder.Append(") ");
			}
			return builder.ToString();
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
			StringBuilder builder = new StringBuilder();
			if ((orderFields != null) && (orderFields.Count > 0))
			{
				builder.Append(" ORDER BY ");
				for (int i = 0; i < orderFields.Count; i++)
				{
					string direction = String.Empty;
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
		public static string GetLimitString(int offset, int count)
		{
			string result = String.Empty;
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