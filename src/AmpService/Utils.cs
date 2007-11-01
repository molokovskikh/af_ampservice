using System;
using System.Collections.Generic;
using System.Text;

namespace AmpService
{
	/// <summary>
	/// Класс содержит вспомогательные функции для генерации SQL запросов
	/// </summary>
	public class Utils
	{
		/// <summary>
		/// Форматирует массив идентификаторов прайс листа для применения в запросе.
		/// Пример: массив {1, 2} приводится к виду " and PricesData.PriceCode in (1, 2) "
		/// </summary>
		/// <param name="priceID">массив идентификаторов</param>
		/// <returns>отформатированная строка</returns>
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
			StringBuilder builder = new StringBuilder();
			int index = 0;
			if (array != null && Count(array) > 0)
			{
				builder.Append(" (");
				foreach (T item in array)
				{
					string value = item.ToString();
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
			else
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
		/// Формирует блок LIMIT для SQL запроса. Пример: "LIMIT 1 20"
		/// </summary>
		/// <param name="offset">Начиная с какого элемента</param>
		/// <param name="count">Количество элементов</param>
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

		private static int Count<T>(IEnumerable<T> enumerable)
		{
			int i = 0;
			foreach (T item in enumerable)

				i++;
			return i;
		}
	}
}