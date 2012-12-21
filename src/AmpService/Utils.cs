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
	/// Класс содержит вспомогательные функции для генерации SQL запросов
	/// </summary>
	public class Utils
	{
		/// <summary>
		/// Преобразовывает массив строк для использования в запросе.
		/// Также происходит замена символа "*" на "%".
		/// Пример: массив {"hello", "w*rld"} разворачивается в строку
		/// "and ( someField like 'hello'  or  someField like 'w%rld')"
		/// </summary>
		/// <param name="array">Аргументы поиска</param>
		/// <param name="fieldName">Имя поля по которому происходит поиск</param>
		/// <returns>Отформатированная строка</returns>
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
					value = MySql.Data.MySqlClient.MySqlHelper.EscapeString(value);
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
		/// Аналогична StringArrayToQuery, но символ * в запросе преобразует в like '%' OR is null
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		public static string StringArrayToQueryNull<T>(IEnumerable<T> array, string fieldName)
		{
			var builder = new StringBuilder();
			var index = 0;
			if (array != null && array.Count() > 0)
			{
				builder.Append(" (");
				foreach (var item in array)
				{
					var value = item.ToString();
					value = MySql.Data.MySqlClient.MySqlHelper.EscapeString(value);
					if(value == "*")
						builder.Append(fieldName + " like '%' or " + fieldName + " is null");
					else if (value.IndexOf("*") > -1)
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
		/// Преобразовывает список строк для использования в блок сортировки SQL запроса.
		/// Пример: массив orderFields = {"field1", "field2"}, orderDirection = {"ASC"}
		/// преобразуется к виду " ORDER BY field1 ASC, field2 ".
		/// </summary>
		/// <param name="orderFields">Список полей из которых формируется блок сортировки.</param>
		/// <param name="orderDirection">Список направлений сортировки для orderFields.
		/// Допустимые значения: null, "ASC", "DESC".
		/// Примечание: длинна списка orderDirection может быть меньше или равна длинне orderFields.
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
		/// Формирует блок LIMIT для SQL запроса. Пример: "LIMIT 1 20"
		/// </summary>
		/// <param name="offset">Начиная с какого элемента</param>
		/// <param name="count">Количество элементов</param>
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