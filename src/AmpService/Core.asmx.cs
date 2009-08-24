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
		/// <param name="onlyLeader">�������� ������ ������� ������� ����������� ���� ��� ��������� ����������</param>
		/// <param name="newEar"></param>
		/// <param name="rangeField">������ ����� ��� ������� �������������� ����������. 
		/// ���������� ��������: PrepCode Int, PriceCode Int, ItemID String, OriginalName String ��� null
		/// </param>
		/// <param name="rangeValue">������ �������� �� ������� ���������� ����������.
		/// ���������� ��������: Int, String - ������ ����� ��������� ���������� "%".
		/// ����������: ���������� �������� ������ ��������� � ����������� �����.
		/// </param>
		/// <param name="sortField">���� �� ������� ����� �������������� ����������. 
		/// ���������� ��������: null, OrderID, SalerCode, CreaterCode, ItemID, OriginalName, 
		/// OriginalCr, Unit, Volume, Quantity, Note, Period, Doc, Junk, UpCost, Cost, PriceCode, 
		/// SalerName, PriceDate, PrepCode, OrderCode1, OrderCode2.
		/// </param>
		/// <param name="sortOrder">������� ���������� ��� ����� �� SortField.
		/// ���������� ��������: "ASC" - ������ ������� ���������� , "DESC" - �������� ������� ��������.
		/// ����������: ���������� �������� ����� ��������� ��� ���� ������ ���������� ����� ��� ����������.
		/// ���� ���������� �������� ������ �� ��� ���������� ����� ����� �������� ������ ������� ���������� (ASC). 
		/// ������: SortField = {"OrderID", "Junk" ,"Cost"}, SortOrder = {"DESC"} - � ���� ������ ������� ����� 
		/// �������������� ��� �� ��� � � ������ ���� ��  SortOrder = {"DESC", "ASC", "ASC"}
		/// </param>
		/// <param name="limit">���������� ������� � �������. 
		/// ����������: ���� Limit ������ SelStart �� ����� ���������� ��� ������ ������� � SelStart.
		/// </param>
		/// <param name="selStart">�������� � �������� ���������� �����. 
		/// ����������: ������� ������� ��� ������ ��������� �������� 0 � �� 1.
		/// </param>
		/// <returns>DataSet ���������� ������� ����� ������.</returns>
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