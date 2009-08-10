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