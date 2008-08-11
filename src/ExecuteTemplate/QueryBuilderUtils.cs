using System;
using System.Collections.Generic;
using System.Text;

namespace ExecuteTemplate
{
	public class QueryBuilderUtils
	{
		/// <summary>
		/// ��������������� ������ ����� ��������� ��� ������������� � �������.
		/// ����� ���������� ������ ������� "*" �� "%".
		/// ������: ������ {"hello", "w*rld"} ��������������� � ������ 
		/// " ( someField like 'hello'  or  someField like 'w%rld') "
		/// </summary>
		/// <param name="array">��������� ������</param>
		/// <param name="fieldName">��� ���� �� �������� ���������� �����</param>
		/// <returns>����������������� ������</returns>
		public static string FormatWhereBlock<T>(IList<T> array, string fieldName)
		{
			StringBuilder builder = new StringBuilder();
			if ((array != null) && (array.Count > 0))
			{
				builder.Append(" (");
				foreach (object item in array)
				{
					//���� �������� ������� ������
					if (item is String)
					{
						//�� ����� �� ����� ��������� ���� ������� � ���� �� ����������
						//���� ������ �������� ���� ������
						if (item.ToString().IndexOf("*") > -1)
							//�� �������� ��� � ��������� ���� Like
							builder.Append(fieldName + " like '" + item.ToString().Replace("*", "%") + "'");
						else
						{
							//����� ��� ����� ���� ������ ���������� �����, ���� ��� ���������
							//���� ������ ����� ���������� ��� �����
							decimal test;
							if (decimal.TryParse(item.ToString(), out test))
								//�� ������� �� ��� ������� 
								builder.Append(fieldName + " = " + item);
							else
								//����� ������ �� � ��������
								builder.Append(fieldName + " = '" + item + "'");
						}
					}
					else
					{
						//����� ����� ��� ��� �������
						builder.Append(fieldName + " = " + item);
					}
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
