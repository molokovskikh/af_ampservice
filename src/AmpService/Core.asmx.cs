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
		/// �������� ������ ������ ������� ����� ������, ����������� �� ����� 
		/// ���������� �� RangeField � �� ��������� �� RangeValue.
		/// </summary>
		/// <param name="OnlyLeader">�������� ������ ������� ������� ����������� ���� ��� ��������� ����������</param>
		/// <param name="NewEar"></param>
		/// <param name="RangeField">������ ����� ��� ������� �������������� ����������. 
		/// ���������� ��������: PrepCode Int, PriceCode Int, ItemID String, OriginalName String ��� null
		/// </param>
		/// <param name="RangeValue">������ �������� �� ������� ���������� ����������.
		/// ���������� ��������: Int, String - ������ ����� ��������� ���������� "%".
		/// ����������: ���������� �������� ������ ��������� � ����������� �����.
		/// </param>
		/// <param name="SortField">���� �� ������� ����� �������������� ����������. 
		/// ���������� ��������: null, OrderID, SalerCode, CreaterCode, ItemID, OriginalName, 
		/// OriginalCr, Unit, Volume, Quantity, Note, Period, Doc, Junk, UpCost, Cost, PriceCode, 
		/// SalerName, PriceDate, PrepCode, OrderCode1, OrderCode2.
		/// </param>
		/// <param name="SortOrder">������� ���������� ��� ����� �� SortField.
		/// ���������� ��������: "ASC" - ������ ������� ���������� , "DESC" - �������� ������� ��������.
		/// ����������: ���������� �������� ����� ��������� ��� ���� ������ ���������� ����� ��� ����������.
		/// ���� ���������� �������� ������ �� ��� ���������� ����� ����� �������� ������ ������� ���������� (ASC). 
		/// ������: SortField = {"OrderID", "Junk" ,"Cost"}, SortOrder = {"DESC"} - � ���� ������ ������� ����� 
		/// �������������� ��� �� ��� � � ������ ���� ��  SortOrder = {"DESC", "ASC", "ASC"}
		/// </param>
		/// <param name="Limit">���������� ������� � �������. 
		/// ����������: ���� Limit ������ SelStart �� ����� ���������� ��� ������ ������� � SelStart.
		/// </param>
		/// <param name="SelStart">�������� � �������� ���������� �����. 
		/// ����������: ������� ������� ��� ������ ��������� �������� 0 � �� 1.
		/// </param>
		/// <returns>DataSet ���������� ������� ����� ������.</returns>
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