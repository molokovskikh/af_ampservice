using System;
using System.Data;
using System.Web.Services;
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
			return service.GetNameFromCatalog(name, form, newEar, offerOnly, priceID, limit, selStart);
		}

		[WebMethod]
		public DataSet GetPriceCodeByName(string[] firmName)
		{
			return service.GetPriceCodeByName(firmName);
		}

		/// <summary>
		/// ¬ыбирает список список позиций прайс листов, основыва€сь на пол€х 
		/// фильтрации из RangeField и их значени€х из RangeValue.
		/// </summary>
		/// <param name="onlyLeader">¬ыбирать только позиции имеющие минимальную цену дл€ заданного поставщика</param>
		/// <param name="newEar"></param>
		/// <param name="rangeField">—писок полей дл€ которых осуществл€етс€ фильтраци€. 
		/// ƒопустимые значени€: PrepCode Int, PriceCode Int, ItemID String, OriginalName String или null
		/// </param>
		/// <param name="rangeValue">—писок значений ко которым происходит фильтраци€.
		/// ƒопустимые значени€: Int, String - строки могут содержать метасимвол "%".
		/// ѕримечание: количество значений должно совпадать с количеством полей.
		/// </param>
		/// <param name="sortField">ѕол€ по которым может осуществл€тьс€ сортировка. 
		/// ƒопустимые значени€: null, OrderID, SalerCode, CreaterCode, ItemID, OriginalName, 
		/// OriginalCr, Unit, Volume, Quantity, Note, Period, Doc, Junk, UpCost, Cost, PriceCode, 
		/// SalerName, PriceDate, PrepCode, OrderCode1, OrderCode2.
		/// </param>
		/// <param name="sortOrder">ѕор€док сортировки дл€ полей из SortField.
		/// ƒопустимые значени€: "ASC" - пр€мой пор€док сортировки , "DESC" - обратный пор€док сортовки.
		/// ѕримечание:  оличество значений может совпадать или быть меньще количества полей дл€ сортировки.
		/// ≈сли количество значений меньше то дл€ оставщихс€ полей бедут применен пр€мой пор€док сортировки (ASC). 
		/// ѕример: SortField = {"OrderID", "Junk" ,"Cost"}, SortOrder = {"DESC"} - в этом случае выборка будет 
		/// отсортированна так же как и в случае если бы  SortOrder = {"DESC", "ASC", "ASC"}
		/// </param>
		/// <param name="limit"> оличество записей в выборке. 
		/// ѕримечание: если Limit меньше SelStart то тогда выбираютс€ все записи начина€ с SelStart.
		/// </param>
		/// <param name="selStart">«начение с которого начинаетс€ выбор. 
		/// ѕримечание: следует помнить что первым значением €вл€етс€ 0 а не 1.
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

		[WebMethod]
		public DataSet GetOrdersByDate(DateTime olderThan, int priceCode)
		{
			return service.GetOrdersByDate(olderThan, priceCode);
		}
	}
}