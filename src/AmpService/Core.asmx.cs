using System;
using System.Data;
using System.Web.Services;
using System.Web.Services.Protocols;
using Common.Models;

namespace AmpService
{
	[WebService(Namespace = "AMPNameSpace")]
	public class AmpService : WebService
	{
		private readonly Service service;

		public AmpService()
		{
			service = IoC.Resolve<Service>();
		}

		[WebMethod]
		public DataSet GetNameFromCatalog(string[] name,
			string[] form,
			bool newEar,
			bool offerOnly,
			uint[] priceID,
			uint limit,
			uint selStart)
		{
			return service.GetNameFromCatalog(name, form, newEar, offerOnly, priceID, limit, selStart, null, null);
		}

		[WebMethod]
		public DataSet GetNameFromCatalogWithMnn(string[] name,
			string[] form,
			bool newEar,
			bool offerOnly,
			uint[] priceID,
			uint limit,
			uint selStart,
			string[] mnn,
			string[] property)
		{
			return service.GetNameFromCatalog(name, form, newEar, offerOnly, priceID, limit, selStart, mnn, property);
		}

		[WebMethod]
		public DataSet GetPriceCodeByName(string[] firmName)
		{
			return service.GetPriceCodeByName(firmName);
		}

		[WebMethod]
		public DataSet GetCategories()
		{
			return service.GetCategories();
		}

		/// <summary>
		/// Выбирает список список позиций прайс листов, основываясь на полях
		/// фильтрации из RangeField и их значениях из RangeValue.
		/// </summary>
		/// <param name="onlyLeader">Выбирать только позиции имеющие минимальную цену для заданного поставщика</param>
		/// <param name="newEar"></param>
		/// <param name="rangeField">Список полей для которых осуществляется фильтрация.
		/// Допустимые значения: PrepCode Int, PriceCode Int, ItemID String, OriginalName String или null
		/// </param>
		/// <param name="rangeValue">Список значений ко которым происходит фильтрация.
		/// Допустимые значения: Int, String - строки могут содержать метасимвол "%".
		/// Примечание: количество значений должно совпадать с количеством полей.
		/// </param>
		/// <param name="sortField">Поля по которым может осуществляться сортировка.
		/// Допустимые значения: null, OrderID, SalerCode, CreaterCode, ItemID, OriginalName,
		/// OriginalCr, Unit, Volume, Quantity, Note, Period, Doc, Junk, UpCost, Cost, PriceCode,
		/// SalerName, PriceDate, PrepCode, OrderCode1, OrderCode2.
		/// </param>
		/// <param name="sortOrder">Порядок сортировки для полей из SortField.
		/// Допустимые значения: "ASC" - прямой порядок сортировки , "DESC" - обратный порядок сортовки.
		/// Примечание: Количество значений может совпадать или быть меньще количества полей для сортировки.
		/// Если количество значений меньше то для оставщихся полей бедут применен прямой порядок сортировки (ASC).
		/// Пример: SortField = {"OrderID", "Junk" ,"Cost"}, SortOrder = {"DESC"} - в этом случае выборка будет
		/// отсортированна так же как и в случае если бы  SortOrder = {"DESC", "ASC", "ASC"}
		/// </param>
		/// <param name="limit">Количество записей в выборке.
		/// Примечание: если Limit меньше SelStart то тогда выбираются все записи начиная с SelStart.
		/// </param>
		/// <param name="selStart">Значение с которого начинается выбор.
		/// Примечание: следует помнить что первым значением является 0 а не 1.
		/// </param>
		/// <returns>DataSet содержащий позиции прайс листов.</returns>
		[WebMethod]
		public DataSet GetPrices(bool onlyLeader,
			bool newEar,
			string[] rangeField,
			string[] rangeValue,
			string[] sortField,
			string[] sortOrder,
			uint limit,
			uint selStart)
		{
			return service.GetPrices(onlyLeader, newEar, rangeField, rangeValue, sortField, sortOrder, limit, selStart);
		}

		[WebMethod]
		public DataSet PostOrder(ulong[] orderId,
			uint[] quantity,
			string[] message,
			uint[] orderCode1,
			uint[] orderCode2,
			bool[] junk)
		{
			return service.PostOrder(orderId, quantity, message, orderCode1, orderCode2, junk);
		}

		/// <summary>
		/// Формирует заявки по ценам клиента
		/// </summary>
		/// <param name="cost">cost - массив цен</param>
		/// <param name="priceDate">массив дат прайс-листов, получается из функции GetPrices поле PriceDate
		/// функция работает аналогично PostOrder
		/// </param>
		[WebMethod]
		public DataSet PostOrder2(ulong[] orderId,
			decimal[] cost,
			DateTime[] priceDate,
			uint[] quantity,
			string[] message,
			uint[] orderCode1,
			uint[] orderCode2,
			bool[] junk)
		{
			return service.PostOrder2(orderId, cost, quantity, priceDate, message, orderCode1, orderCode2, junk);
		}

		[WebMethod]
		public DataSet GetOrdersByDate(DateTime olderThan, int priceCode)
		{
			return service.GetOrdersByDate(olderThan, priceCode);
		}

		[WebMethod]
		public DataSet GetOrder(uint orderId)
		{
			return service.GetOrder(orderId);
		}

		[WebMethod]
		public DataSet GetOrderItems(DateTime begin, DateTime end)
		{
			return service.GetOrderItems(begin, end);
		}
	}
}