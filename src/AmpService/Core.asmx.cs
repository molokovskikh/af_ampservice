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
		public DataSet GetNameFromCatalog(string[] Name,
		                                  string[] Form,
		                                  bool NewEar,
		                                  bool OfferOnly,
		                                  uint[] PriceID,
		                                  uint Limit,
		                                  uint SelStart)
		{
			return service.GetNameFromCatalog(Name, Form, NewEar, OfferOnly, PriceID, Limit, SelStart);
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
		/// <param name="OnlyLeader">¬ыбирать только позиции имеющие минимальную цену дл€ заданного поставщика</param>
		/// <param name="NewEar"></param>
		/// <param name="RangeField">—писок полей дл€ которых осуществл€етс€ фильтраци€. 
		/// ƒопустимые значени€: PrepCode Int, PriceCode Int, ItemID String, OriginalName String или null
		/// </param>
		/// <param name="RangeValue">—писок значений ко которым происходит фильтраци€.
		/// ƒопустимые значени€: Int, String - строки могут содержать метасимвол "%".
		/// ѕримечание: количество значений должно совпадать с количеством полей.
		/// </param>
		/// <param name="SortField">ѕол€ по которым может осуществл€тьс€ сортировка. 
		/// ƒопустимые значени€: null, OrderID, SalerCode, CreaterCode, ItemID, OriginalName, 
		/// OriginalCr, Unit, Volume, Quantity, Note, Period, Doc, Junk, UpCost, Cost, PriceCode, 
		/// SalerName, PriceDate, PrepCode, OrderCode1, OrderCode2.
		/// </param>
		/// <param name="SortOrder">ѕор€док сортировки дл€ полей из SortField.
		/// ƒопустимые значени€: "ASC" - пр€мой пор€док сортировки , "DESC" - обратный пор€док сортовки.
		/// ѕримечание:  оличество значений может совпадать или быть меньще количества полей дл€ сортировки.
		/// ≈сли количество значений меньше то дл€ оставщихс€ полей бедут применен пр€мой пор€док сортировки (ASC). 
		/// ѕример: SortField = {"OrderID", "Junk" ,"Cost"}, SortOrder = {"DESC"} - в этом случае выборка будет 
		/// отсортированна так же как и в случае если бы  SortOrder = {"DESC", "ASC", "ASC"}
		/// </param>
		/// <param name="Limit"> оличество записей в выборке. 
		/// ѕримечание: если Limit меньше SelStart то тогда выбираютс€ все записи начина€ с SelStart.
		/// </param>
		/// <param name="SelStart">«начение с которого начинаетс€ выбор. 
		/// ѕримечание: следует помнить что первым значением €вл€етс€ 0 а не 1.
		/// </param>
		/// <returns>DataSet содержащий позиции прайс листов.</returns>
		[WebMethod]
		public DataSet GetPrices(bool OnlyLeader,
								 bool NewEar,
								 string[] RangeField,
								 string[] RangeValue,
								 string[] SortField,
								 string[] SortOrder,
								 uint Limit,
								 uint SelStart)
		{
			return service.GetPrices(OnlyLeader, NewEar, RangeField, RangeValue, SortField, SortOrder, Limit, SelStart);
		}

		[WebMethod]
		public DataSet PostOrder(ulong[] PrderId, 
								 uint[] Quantity, 
								 string[] Message, 
								 uint[] OrderCode1, 
								 uint[] OrderCode2,
		                         bool[] Junk)
		{
			return service.PostOrder(PrderId, Quantity, Message, OrderCode1, OrderCode2, Junk);
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
	}
}