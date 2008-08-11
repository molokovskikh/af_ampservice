using System;
using System.Collections.Generic;
using System.Text;

namespace ExecuteTemplate
{
	public class QueryBuilderUtils
	{
		/// <summary>
		/// ѕреобразовывает массив любых элументов дл€ использовани€ в запросе.
		/// “акже происходит замена символа "*" на "%".
		/// ѕример: массив {"hello", "w*rld"} разворачиваетс€ в строку 
		/// " ( someField like 'hello'  or  someField like 'w%rld') "
		/// </summary>
		/// <param name="array">јргументы поиска</param>
		/// <param name="fieldName">»м€ пол€ по которому происходит поиск</param>
		/// <returns>ќтформатированна€ строка</returns>
		public static string FormatWhereBlock<T>(IList<T> array, string fieldName)
		{
			StringBuilder builder = new StringBuilder();
			if ((array != null) && (array.Count > 0))
			{
				builder.Append(" (");
				foreach (object item in array)
				{
					//если вход€щий элемент строка
					if (item is String)
					{
						//то тогда он может сожержать мета символы и надо их обработать
						//если строка содержит мета символ
						if (item.ToString().IndexOf("*") > -1)
							//то замен€ем его и формируем блок Like
							builder.Append(fieldName + " like '" + item.ToString().Replace("*", "%") + "'");
						else
						{
							//иначе это может быть строка содержаща€ число, надо это проверить
							//если строку можна распарсить как число
							decimal test;
							if (decimal.TryParse(item.ToString(), out test))
								//то выводим ее без кавычек 
								builder.Append(fieldName + " = " + item);
							else
								//иначе выводи ее в ковычках
								builder.Append(fieldName + " = '" + item + "'");
						}
					}
					else
					{
						//иначе пишем его без кавычек
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
		/// ѕреобразовывает список строк дл€ использовани€ в блок сортировки SQL запроса.
		/// ѕример: массив orderFields = {"field1", "field2"}, orderDirection = {"ASC"}
		/// преобразуетс€ к виду " ORDER BY field1 ASC, field2 ".
		/// </summary>
		/// <param name="orderFields">—писок полей из которых формируетс€ блок сортировки.</param>
		/// <param name="orderDirection">—писок направлений сортировки дл€ orderFields.
		/// ƒопустимые значени€: null, "ASC", "DESC".
		/// ѕримечание: длинна списка orderDirection может быть меньше или равна длинне orderFields.
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
		/// ‘ормирует блок LIMIT дл€ SQL запроса. ѕример: "LIMIT 1 20"
		/// </summary>
		/// <param name="offset">Ќачина€ с какого элемента</param>
		/// <param name="count"> оличество элементов</param>
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
