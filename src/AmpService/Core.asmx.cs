using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using Common.MySql;
using ExecuteTemplate;
using MySql.Data.MySqlClient;

namespace AmpService
{
	[WebService(Namespace = "AMPNameSpace")]
	public class AMPService : WebService
	{
		private readonly DateTime StartTime = DateTime.Now;

		private DataSet GetDataSet()
		{
			var MyDS = new DataSet();
			var DataTable1 = new DataTable();
			var DataColumn1 = new DataColumn();
			var DataColumn2 = new DataColumn();
			var DataColumn3 = new DataColumn();
			var DataColumn4 = new DataColumn();
			var DataColumn5 = new DataColumn();
			var DataColumn6 = new DataColumn();
			var DataColumn7 = new DataColumn();
			var DataColumn8 = new DataColumn();
			var DataColumn9 = new DataColumn();
			var DataColumn10 = new DataColumn();
			var DataColumn11 = new DataColumn();
			var DataColumn12 = new DataColumn();
			var DataColumn13 = new DataColumn();
			var DataColumn14 = new DataColumn();
			var DataColumn15 = new DataColumn();
			var DataColumn16 = new DataColumn();
			var DataColumn17 = new DataColumn();
			var dataColumn18 = new DataColumn();
			var dataColumn19 = new DataColumn();
			MyDS.BeginInit();
			DataTable1.BeginInit();
			MyDS.DataSetName = "AMPDataSet";
			MyDS.Locale = new CultureInfo("ru-RU");
			MyDS.Tables.AddRange(new[] {DataTable1});
			DataTable1.Columns.AddRange(
				new[]
					{
						DataColumn1, DataColumn2, DataColumn3, DataColumn4, DataColumn5, DataColumn6,
						DataColumn7, DataColumn8, DataColumn9, DataColumn10, DataColumn11, DataColumn12,
						DataColumn13, DataColumn14, DataColumn15, DataColumn16, DataColumn17, dataColumn18,
						dataColumn19
					});
			DataTable1.TableName = "Prices";
			DataColumn1.ColumnName = "OrderID";
			DataColumn1.DataType = typeof (long);
			DataColumn2.ColumnName = "CreaterCode";
			DataColumn3.ColumnName = "ItemID";
			DataColumn4.Caption = "OriginalName";
			DataColumn4.ColumnName = "OriginalName";
			DataColumn5.ColumnName = "OriginalCr";
			DataColumn6.ColumnName = "Unit";
			DataColumn7.ColumnName = "Volume";
			DataColumn8.ColumnName = "Quantity";
			DataColumn9.ColumnName = "Note";
			DataColumn10.ColumnName = "Period";
			DataColumn11.ColumnName = "Doc";
			DataColumn12.ColumnName = "Junk";
			DataColumn12.DataType = typeof (bool);
			DataColumn13.ColumnName = "UpCost";
			DataColumn13.DataType = typeof (decimal);
			DataColumn14.ColumnName = "Cost";
			DataColumn14.DataType = typeof (decimal);
			DataColumn15.ColumnName = "SalerID";
			DataColumn15.DataType = typeof (uint);
			DataColumn16.ColumnName = "PriceDate";
			DataColumn17.ColumnName = "PrepCode";
			DataColumn17.DataType = typeof (uint);
			dataColumn18.ColumnName = "OrderCode1";
			dataColumn18.DataType = typeof (uint);
			dataColumn19.ColumnName = "OrderCode2";
			dataColumn19.DataType = typeof (uint);
			MyDS.EndInit();
			DataTable1.EndInit();
			return MyDS;
		}

		[WebMethod]
		public DataSet GetNameFromCatalog(string[] Name,
		                                  string[] Form,
		                                  bool NewEar,
		                                  bool OfferOnly,
		                                  uint[] PriceID,
		                                  int Limit,
		                                  int SelStart)
		{
			if (!HavePermission(GetUserName()))
				return null;

			var data = GetDataSet();
			Restart:
			try
			{
				Service.UpdateLastAccessTime(GetUserName());

				using (var connection = new ConnectionManager().GetConnection())
				{
					connection.Open();
					using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
					{
						try
						{
							var clientCode = GetClientCode(connection);

							var commandText = new StringBuilder();

							int i;
							var searchByApmCode = Name != null && Name.Length > 0 && int.TryParse(Name[0], out i);

							using (CleanUp.AfterGetActivePrices(new Common.MySql.MySqlHelper(connection, transaction)))
							{
								if (OfferOnly)
								{
									var command = new MySqlCommand("CALL GetActivePrices(?ClientCode);", connection, transaction);
									command.Parameters.AddWithValue("?ClientCode", clientCode);
									command.ExecuteNonQuery();

									commandText.AppendLine(
										@"
SELECT	p.id as PrepCode, 
		cn.Name, 
		cf.Form,
		cast(ifnull(group_concat(distinct pv.Value ORDER BY prop.PropertyName, pv.Value SEPARATOR ', '), '') as CHAR) as Properties
FROM Catalogs.Catalog c
	JOIN Catalogs.CatalogNames cn on cn.id = c.nameid
	JOIN Catalogs.CatalogForms cf on cf.id = c.formid
	JOIN Catalogs.Products p on p.CatalogId = c.Id
	LEFT JOIN Catalogs.ProductProperties pp on pp.ProductId = p.Id
	LEFT JOIN Catalogs.PropertyValues pv on pv.id = pp.PropertyValueId
	LEFT JOIN Catalogs.Properties prop on prop.Id = pv.PropertyId
	JOIN Farm.Core0 c0 on c0.ProductId = p.Id
		JOIN activeprices ap on ap.PriceCode = c0.PriceCode
	LEFT JOIN farm.core0 ampc on ampc.ProductId = p.Id and ampc.codefirmcr = c0.codefirmcr and ampc.PriceCode = 1864
WHERE");
								}
								else
								{
									commandText.AppendLine(
										@"
SELECT	p.id as PrepCode,  
		cn.Name, 
		cf.Form,
		cast(ifnull(group_concat(distinct pv.Value ORDER BY prop.PropertyName, pv.Value SEPARATOR ', '), '') as CHAR) as Properties
FROM Catalogs.Catalog c 
	JOIN Catalogs.CatalogNames cn on cn.id = c.nameid
	JOIN Catalogs.CatalogForms cf on cf.id = c.formid
	JOIN Catalogs.Products p on p.CatalogId = c.Id
	LEFT JOIN Catalogs.ProductProperties pp on pp.ProductId = p.Id
	LEFT JOIN Catalogs.PropertyValues pv on pv.id = pp.PropertyValueId
	LEFT JOIN Catalogs.Properties prop on prop.Id = pv.PropertyId
	LEFT JOIN farm.core0 ampc on ampc.ProductId = p.Id and ampc.PriceCode = 1864
WHERE");
								}

								WhereBlockBuilder
									.ForCommandTest(commandText)
									.AddCriteria(Utils.StringArrayToQuery(PriceID, "ap.pricecode"),
									             OfferOnly
									             && PriceID != null
									             && !(PriceID.Length == 1
									                  && PriceID[0] == 0))
									.AddCriteria(Utils.StringArrayToQuery(Form, "cf.Form"))
									.AddCriteria(Utils.StringArrayToQuery(Name, "ampc.code"), searchByApmCode)
									.AddCriteria(Utils.StringArrayToQuery(Name, "cn.Name"), !searchByApmCode)
									.AddCriteria("ampc.id is null", NewEar)
									.AddCriteria("p.Hidden = 0")
									.AddCriteria("c.Hidden = 0");

								commandText.AppendLine("GROUP BY p.Id");
								commandText.AppendLine("ORDER BY cn.Name, cf.Form");
								commandText.AppendLine(Utils.GetLimitString(SelStart, Limit));

								var adapter = new MySqlDataAdapter(commandText.ToString(), connection);
								adapter.SelectCommand.Transaction = transaction;
								adapter.SelectCommand.Parameters.AddWithValue("?ClientCode", clientCode);
								adapter.Fill(data, "Catalog");

								LogQuery(data, false, MethodBase.GetCurrentMethod().Name,
								         new KeyValuePair<string, object>("Name", Name),
								         new KeyValuePair<string, object>("Form", Form),
								         new KeyValuePair<string, object>("NewEar", NewEar),
								         new KeyValuePair<string, object>("OfferOnly", OfferOnly),
								         new KeyValuePair<string, object>("PriceID", PriceID),
								         new KeyValuePair<string, object>("Limit", Limit),
								         new KeyValuePair<string, object>("SelStart", SelStart));
							}

							transaction.Commit();
							return data;
						}
						catch (Exception)
						{
							if (transaction != null)
								transaction.Rollback();

							throw;
						}
					}
				}
			}
			catch (MySqlException MySQLErr)
			{
				if (MySQLErr.Number == 1213 | MySQLErr.Number == 1205)
				{
					Thread.Sleep(100);
					goto Restart;
				}
				AmpService.PostOrder.MailErr(MethodBase.GetCurrentMethod().Name, MySQLErr.Message, MySQLErr.Source, GetUserName());
			}
			catch (Exception ex)
			{
				AmpService.PostOrder.MailErr(MethodBase.GetCurrentMethod().Name, ex.Message, ex.Source, GetUserName());
			}
			return data;
		}

		[WebMethod]
		public DataSet PostOrder(long[] OrderID, Int32[] Quantity, string[] Message, Int32[] OrderCode1, Int32[] OrderCode2,
		                         bool[] Junk)
		{
			return MethodTemplate.ExecuteMethod<PostOrderArgs, DataSet>(
				new PostOrderArgs(OrderID, Quantity, Message, OrderCode1, OrderCode2, Junk),
				InnerPostOrder, 
				null, 
				GetClientCode,
				false);
		}

		private DataSet InnerPostOrder(PostOrderArgs e)
		{
			if (!HavePermission(GetUserName()))
				return null;

			Service.UpdateLastAccessTime(GetUserName());

			var Res = new long[e.CoreIDs.Length];
			for (int i = 0; i < Res.Length; i++)
			{
				Res[i] = -1;
			}

			//���� ����� �� �����, �� ���������� null
			if ((e.CoreIDs == null) || (e.Quantities == null) || (e.SynonymCodes == null) || (e.SynonymFirmCrCodes == null)
			    || (e.Junks == null)
			    || (e.CoreIDs.Length != e.Quantities.Length)
			    || (e.CoreIDs.Length != e.SynonymCodes.Length) || (e.CoreIDs.Length != e.SynonymFirmCrCodes.Length)
			    || (e.CoreIDs.Length != e.Junks.Length))
				return null;

			string CoreIDString = "(";
			foreach (long ID in e.CoreIDs)
			{
				if ((CoreIDString.Length > 1) && (ID > 0))
				{
					CoreIDString += ", ";
				}
				if (ID > 0)
				{
					CoreIDString += ID.ToString();
				}
			}
			CoreIDString += ")";

			var dsPost = new DataSet();

			e.DataAdapter.SelectCommand.CommandText =
				"select SubmitOrders and AllowSubmitOrders from usersettings.RetClientsSet where clientcode = ?ClientCode";
			e.DataAdapter.SelectCommand.Parameters.AddWithValue("?ClientCode", e.ClientCode);
			bool submit = Convert.ToBoolean(e.DataAdapter.SelectCommand.ExecuteScalar());

			using (
				CleanUp.AfterGetActivePrices(new Common.MySql.MySqlHelper(e.DataAdapter.SelectCommand.Connection,
				                                                          e.DataAdapter.SelectCommand.Transaction)))
			{
				e.DataAdapter.SelectCommand.CommandText = "CALL GetActivePrices(?ClientCode);";
				e.DataAdapter.SelectCommand.ExecuteNonQuery();

				e.DataAdapter.SelectCommand.CommandText =
					@"
SELECT  cd.FirmCode as ClientCode,
		cd.RegionCode,
		ap.PriceCode,
		ap.PriceDate,
		c.Id,
        c.ProductId,
        c.CodeFirmCr,
        c.SynonymCode,
        c.SynonymFirmCrCode,
        c.Code,
		c.CodeCr,
		0 Quantity,
        c.Junk as Junk,
		c.Await as Await,
		c.RequestRatio, 
		c.MinOrderCount, 
		c.OrderCost,
		if(if(round(cc.Cost*ap.UpCost,2)<MinBoundCost, MinBoundCost, round(cc.Cost*ap.UpCost,2))>MaxBoundCost,MaxBoundCost, if(round(cc.Cost*ap.UpCost,2)<MinBoundCost, MinBoundCost, round(cc.Cost*ap.UpCost,2))) as Cost
FROM (farm.core0 c, usersettings.clientsdata cd)
  JOIN ActivePrices ap on c.PriceCode = ap.PriceCode
    JOIN farm.CoreCosts cc on cc.Core_Id = c.Id and cc.PC_CostCode = ap.CostCode
WHERE 	cd.FirmCode	= ?ClientCode
		AND c.ID in " +
					CoreIDString;

				e.DataAdapter.SelectCommand.Parameters.Clear();
				e.DataAdapter.SelectCommand.Parameters.Add("?ClientCode", e.ClientCode);
				e.DataAdapter.Fill(dsPost, "SummaryOrder");
			}
			DataTable dtSummaryOrder = dsPost.Tables["SummaryOrder"];

			DataRow[] drs;
			dtSummaryOrder.Columns.Add(new DataColumn("Message", typeof (string)));
			for (int i = 0; i < e.CoreIDs.Length; i++)
			{
				drs = dtSummaryOrder.Select(string.Format("Id = {0}", e.CoreIDs[i]));
				if (drs.Length > 0)
				{
					drs[0]["Quantity"] = e.Quantities[i];
					if ((e.Messages != null) && (e.Messages.Length > i))
						drs[0]["Message"] = e.Messages[i];
				}
			}

			DataTable dtOrderHead = dtSummaryOrder.DefaultView.ToTable(true, "ClientCode", "RegionCode", "PriceCode", "PriceDate");
			dtOrderHead.Columns.Add(new DataColumn("OrderID", typeof (long)));

			DataRow[] drOrderList;
			foreach (DataRow drOH in dtOrderHead.Rows)
			{
				drOrderList = dtSummaryOrder.Select("PriceCode = " + drOH["PriceCode"]);
				if (drOrderList.Length > 0)
				{
					e.DataAdapter.SelectCommand.CommandText =
						"insert into orders.ordershead (WriteTime, ClientCode, PriceCode, RegionCode, PriceDate, RowCount, ClientAddition, Processed, Submited, SubmitDate)" +
						"values(now(), ?ClientCode, ?PriceCode, ?RegionCode, ?PriceDate, ?RowCount, ?ClientAddition, 0, ?Submited, ?SubmitDate);";
					e.DataAdapter.SelectCommand.CommandText += "select LAST_INSERT_ID()";
					e.DataAdapter.SelectCommand.Parameters.Clear();
					e.DataAdapter.SelectCommand.Parameters.Add("?ClientCode", drOH["ClientCode"]);
					e.DataAdapter.SelectCommand.Parameters.Add("?PriceCode", drOH["PriceCode"]);
					e.DataAdapter.SelectCommand.Parameters.Add("?RegionCode", drOH["RegionCode"]);
					e.DataAdapter.SelectCommand.Parameters.Add("?PriceDate", drOH["PriceDate"]);
					e.DataAdapter.SelectCommand.Parameters.Add("?RowCount", drOrderList.Length);
					e.DataAdapter.SelectCommand.Parameters.Add("?ClientAddition", drOrderList[0]["Message"]);
					e.DataAdapter.SelectCommand.Parameters.AddWithValue("?Submited", submit ? 0 : 1);
					e.DataAdapter.SelectCommand.Parameters.AddWithValue("?SubmitDate", submit ? null : new DateTime?(DateTime.Now));
					drOH["OrderID"] = Convert.ToInt64(e.DataAdapter.SelectCommand.ExecuteScalar());
					e.DataAdapter.SelectCommand.CommandText =
@"insert into orders.orderslist (OrderID, ProductId, CodeFirmCr, SynonymCode, SynonymFirmCrCode, Code, CodeCr, Quantity, Junk, Await, Cost, CoreId, RequestRatio, MinOrderCount, OrderCost)
values (?OrderID, ?ProductId, ?CodeFirmCr, ?SynonymCode, ?SynonymFirmCrCode, ?Code, ?CodeCr, ?Quantity, ?Junk, ?Await, ?Cost, ?CoreId, ?RequestRatio, ?MinOrderCount, ?OrderCost);";
					e.DataAdapter.SelectCommand.Parameters.Clear();
					e.DataAdapter.SelectCommand.Parameters.Add("?OrderID", MySqlDbType.Int64);
					e.DataAdapter.SelectCommand.Parameters.Add("?ProductId", MySqlDbType.Int64);
					e.DataAdapter.SelectCommand.Parameters.Add("?CodeFirmCr", MySqlDbType.Int64);
					e.DataAdapter.SelectCommand.Parameters.Add("?SynonymCode", MySqlDbType.Int64);
					e.DataAdapter.SelectCommand.Parameters.Add("?SynonymFirmCrCode", MySqlDbType.Int64);
					e.DataAdapter.SelectCommand.Parameters.Add("?Code", MySqlDbType.VarString);
					e.DataAdapter.SelectCommand.Parameters.Add("?CodeCr", MySqlDbType.VarString);
					e.DataAdapter.SelectCommand.Parameters.Add("?Junk", MySqlDbType.Byte);
					e.DataAdapter.SelectCommand.Parameters.Add("?Await", MySqlDbType.Byte);
					e.DataAdapter.SelectCommand.Parameters.Add("?Cost", MySqlDbType.Decimal);
					e.DataAdapter.SelectCommand.Parameters.Add("?Quantity", MySqlDbType.Int32);
					e.DataAdapter.SelectCommand.Parameters.Add("?CoreId", MySqlDbType.Int64);
					e.DataAdapter.SelectCommand.Parameters.Add("?RequestRatio", MySqlDbType.Int32);
					e.DataAdapter.SelectCommand.Parameters.Add("?MinOrderCount", MySqlDbType.Int32);
					e.DataAdapter.SelectCommand.Parameters.Add("?OrderCost", MySqlDbType.Decimal);
					foreach (DataRow drOL in drOrderList)
					{
						e.DataAdapter.SelectCommand.Parameters["?OrderID"].Value = drOH["OrderID"];
						e.DataAdapter.SelectCommand.Parameters["?ProductId"].Value = drOL["ProductId"];
						e.DataAdapter.SelectCommand.Parameters["?CodeFirmCr"].Value = drOL["CodeFirmCr"];
						e.DataAdapter.SelectCommand.Parameters["?SynonymCode"].Value = drOL["SynonymCode"];
						e.DataAdapter.SelectCommand.Parameters["?SynonymFirmCrCode"].Value = drOL["SynonymFirmCrCode"];
						e.DataAdapter.SelectCommand.Parameters["?Code"].Value = drOL["Code"];
						e.DataAdapter.SelectCommand.Parameters["?CodeCr"].Value = drOL["CodeCr"];
						e.DataAdapter.SelectCommand.Parameters["?Junk"].Value = drOL["Junk"];
						e.DataAdapter.SelectCommand.Parameters["?Await"].Value = drOL["Await"];
						e.DataAdapter.SelectCommand.Parameters["?Cost"].Value = drOL["Cost"];
						e.DataAdapter.SelectCommand.Parameters["?Quantity"].Value = drOL["Quantity"];
						e.DataAdapter.SelectCommand.Parameters["?CoreId"].Value = drOL["ID"];
						e.DataAdapter.SelectCommand.Parameters["?RequestRatio"].Value = drOL["RequestRatio"];
						e.DataAdapter.SelectCommand.Parameters["?MinOrderCount"].Value = drOL["MinOrderCount"];
						e.DataAdapter.SelectCommand.Parameters["?OrderCost"].Value = drOL["OrderCost"];
						e.DataAdapter.SelectCommand.ExecuteNonQuery();
						int Index = Array.IndexOf(e.CoreIDs, Convert.ToInt64(drOL["ID"]));
						if (Index > -1)
							Res[Index] = Convert.ToInt64(drOH["OrderID"]);
					}
				}
			}

			//�������� ��������� ������� �����������
			DataTable dtPricesRes = AmpService.PostOrder.GetPricesDataTable();
			dtPricesRes.TableName = "Prices";

			DataRow drOK;

			DataTable dtTemp;

			using (
				CleanUp.AfterGetActivePrices(new Common.MySql.MySqlHelper(e.DataAdapter.SelectCommand.Connection,
				                                                          e.DataAdapter.SelectCommand.Transaction)))
			{
				e.DataAdapter.SelectCommand.CommandText = "CALL GetActivePrices(?ClientCode);";
				e.DataAdapter.SelectCommand.Parameters.Clear();
				e.DataAdapter.SelectCommand.Parameters.AddWithValue("?ClientCode", e.ClientCode);
				e.DataAdapter.SelectCommand.ExecuteNonQuery();

				e.DataAdapter.SelectCommand.CommandText =
					@"
SELECT  c.id OrderID,
		c.id OriginalOrderID,
        ifnull(c.Code, '') SalerCode,
        ifnull(c.CodeCr, '') CreaterCode,
        ifnull(ampc.code, '') ItemID,
        s.synonym OriginalName,
        ifnull(scr.synonym, '') OriginalCr,
        ifnull(c.Unit, '') Unit,
        ifnull(c.Volume, '') Volume,
        ifnull(c.Quantity, '') Quantity,
        ifnull(c.Note, '') Note,
        ifnull(c.Period, '') Period,
        ifnull(c.Doc, '') Doc,
        c.Junk as Junk,
        ap.PublicUpCost As UpCost,
		if(if(round(cc.Cost*ap.UpCost,2)<c.MinBoundCost, c.MinBoundCost, round(cc.Cost*ap.UpCost,2))>c.MaxBoundCost,c.MaxBoundCost, if(round(cc.Cost*ap.UpCost,2)<c.MinBoundCost, c.MinBoundCost, round(cc.Cost*ap.UpCost,2))) as Cost,
        ap.pricecode SalerID,
        cd.ShortName SalerName,
        ap.PriceDate,
        c.ProductId PrepCode,
        c.synonymcode OrderCode1,
        c.synonymfirmcrcode OrderCode2
FROM farm.core0 c
  JOIN ActivePrices ap on c.PriceCode = ap.PriceCode
		JOIN usersettings.PricesData pd on pd.PriceCode = ap.PriceCode
    JOIN farm.CoreCosts cc on cc.Core_Id = c.Id and cc.PC_CostCode = ap.CostCode
	JOIN usersettings.clientsdata cd on cd.FirmCode = ap.FirmCode
	JOIN farm.synonym s on s.PriceCode = ifnull(pd.parentsynonym, ap.pricecode)
    LEFT JOIN farm.core0 ampc ON ampc.ProductId = c.ProductId and ampc.codefirmcr = c.codefirmcr and ampc.PriceCode = 1864
	LEFT JOIN farm.synonymfirmcr scr ON scr.PriceCode = ifnull(pd.ParentSynonym, ap.pricecode) and c.synonymfirmcrcode = scr.synonymfirmcrcode
WHERE	c.SynonymCode = ?SynonymCode
		and c.SynonymFirmCrCode = ?SynonymFirmCrCode
		and c.Junk = ?Junk;
";

				e.DataAdapter.SelectCommand.Parameters.Clear();
				e.DataAdapter.SelectCommand.Parameters.AddWithValue("?ClientCode", e.ClientCode);
				e.DataAdapter.SelectCommand.Parameters.AddWithValue("?SynonymCode", MySqlDbType.Int32);
				e.DataAdapter.SelectCommand.Parameters.AddWithValue("?SynonymFirmCrCode", MySqlDbType.Int32);
				e.DataAdapter.SelectCommand.Parameters.AddWithValue("?Junk", MySqlDbType.Bit);

				for (int i = 0; i < Res.Length; i++)
				{
					if (Res[i] > -1)
					{
						drOK = dtPricesRes.NewRow();
						drOK["OrderID"] = Res[i];
						drOK["OriginalOrderID"] = e.CoreIDs[i];
						dtPricesRes.Rows.Add(drOK);
					}
					else
					{
						dtTemp = AmpService.PostOrder.GetPricesDataTable();
						e.DataAdapter.SelectCommand.Parameters["?SynonymCode"].Value = e.SynonymCodes[i];
						e.DataAdapter.SelectCommand.Parameters["?SynonymFirmCrCode"].Value = e.SynonymFirmCrCodes[i];
						e.DataAdapter.SelectCommand.Parameters["?Junk"].Value = e.Junks[i];
						e.DataAdapter.Fill(dtTemp);

						if (dtTemp.Rows.Count > 0)
						{
							dtTemp.DefaultView.Sort = "Cost";
							drOK = dtPricesRes.NewRow();
							drOK.ItemArray = dtTemp.DefaultView[0].Row.ItemArray;
							drOK["OriginalOrderID"] = e.CoreIDs[i];
							dtPricesRes.Rows.Add(drOK);
						}
						else
						{
							drOK = dtPricesRes.NewRow();
							drOK["OrderID"] = -1;
							drOK["OriginalOrderID"] = e.CoreIDs[i];
							dtPricesRes.Rows.Add(drOK);
						}
					}
				}
			}


			//��������� �������������� �������
			var dsRes = new DataSet();
			dsRes.Tables.Add(dtPricesRes);

			/*
             PostOrder - �������� ������� �� ���� �������

            ������� ������:
            OrderID() int(32) unsigned - ������ �����(Prices.OrderID) ��� ������ ����������
            Quantity() int(32) unsigned - ������ ��������� ������������ ����������

            �����:
            PostOrder() Int(32) unsigned - ������ ����� �������������, ���� ��������� ��������:
            PostOrder >=0 - ��� ��������������� ������(�������)
            PostOrder=-1 - ������ ������������ ������.

             */
			LogQuery(dsRes, false, "PostOrder");
			return dsRes;
		}

		private DataSet InnerGetPriceCodeByName(FirmNameArgs e)
		{
			if (!HavePermission(GetUserName()))
				return null;

			Service.UpdateLastAccessTime(GetUserName());

			e.DataAdapter.SelectCommand.CommandText = @"
                                SELECT  PricesData.PriceCode as PriceCode, 
                                        PricesData.PriceName as PriceName,
                                        PriceInfo,
                                        ClientsData.FirmCode,
                                        ClientsData.ShortName as FirmName, 
                                        RegionalData.ContactInfo,
                                        RegionalData.OperativeInfo
                                FROM    (intersection, clientsdata, pricesdata, pricesregionaldata, retclientsset, clientsdata as AClientsData, RegionalData) 
                                WHERE   DisabledByClient                                            = 0 
                                        and Disabledbyfirm                                          = 0 
                                        and DisabledByAgency                                        = 0 
                                        and intersection.clientcode                                 = ?ClientCode 
                                        and retclientsset.clientcode                                = intersection.clientcode 
                                        and pricesdata.pricecode                                    = intersection.pricecode 
                                        and clientsdata.firmcode                                    = pricesdata.firmcode 
                                        and clientsdata.firmstatus                                  = 1 
                                        and clientsdata.billingstatus                               = 1 
                                        and clientsdata.firmtype                                    = 0 
                                        and clientsdata.firmsegment                                 = AClientsData.firmsegment 
                                        and pricesregionaldata.regioncode                           = intersection.regioncode 
                                        and pricesregionaldata.pricecode                            = pricesdata.pricecode 
                                        and AClientsData.firmcode                                   = intersection.clientcode 
                                        and (clientsdata.maskregion & intersection.regioncode)      > 0 
                                        and (AClientsData.maskregion & intersection.regioncode)     > 0 
                                        and (retclientsset.workregionmask & intersection.regioncode)> 0 
                                        and pricesdata.agencyenabled                                = 1 
                                        and pricesdata.enabled                                      = 1 
                                        and invisibleonclient                                       = 0 
                                        and pricesdata.pricetype                                   <> 1 
                                        and pricesregionaldata.enabled                              = 1
                                        and RegionalData.FirmCode = ClientsData.FirmCode
                                        and RegionalData.RegionCode = Intersection.RegionCode
                                        ";


			if (e.FirmNames != null && e.FirmNames.Length > 0)
				e.DataAdapter.SelectCommand.CommandText += " and " + Utils.StringArrayToQuery(e.FirmNames, "ClientsData.ShortName");

			e.DataAdapter.SelectCommand.Parameters.Add("?ClientCode", e.ClientCode);

			var data = new DataSet();
			e.DataAdapter.Fill(data, "PriceList");
			LogQuery(data, false, "GetPriceCodeByName");
			return data;
		}

		[WebMethod]
		public DataSet GetPriceCodeByName(string[] firmName)
		{
			return MethodTemplate.ExecuteMethod<FirmNameArgs, DataSet>(
				new FirmNameArgs(firmName), InnerGetPriceCodeByName, null, GetClientCode);
		}

		private DataSet InnerGetPrices(GetPricesArgs e)
		{
			if (!HavePermission(GetUserName()))
				return null;

			Service.UpdateLastAccessTime(GetUserName());

			const string functionName = "GetPrices";
			//������� ��� ��������� � ���������� ���� ����� ��� ������� � ����� ����� ��� ������������� � �������
			var validRequestFields = new Dictionary<string, string>(new CaseInsensitiveStringComparer())
			                         	{
			                         		{"OrderID", "c.id"},
			                         		{"SalerCode", "c.Code"},
			                         		{"SalerName", "cd.ShortName"},
			                         		{"ItemID", "ampc.Code"},
			                         		{"OriginalCR", "sfc.Synonym"},
			                         		{"OriginalName", "s.Synonym"},
			                         		{"PriceCode", "ap.PriceCode"},
			                         		{"PrepCode", "c.ProductId"}
			                         	};

			var validSortFields = new List<string>
			                      	{
			                      		"OrderID",
			                      		"SalerCode",
			                      		"CreaterCode",
			                      		"ItemID",
			                      		"OriginalName",
			                      		"OriginalCR",
			                      		"Unit",
			                      		"Volume",
			                      		"Quantity",
			                      		"Note",
			                      		"Period",
			                      		"Doc",
			                      		"Junk",
			                      		"Upcost",
			                      		"Cost",
			                      		"PriceCode",
			                      		"SalerName",
			                      		"PriceDate",
			                      		"PrepCode",
			                      		"OrderCode1",
			                      		"OrderCode2"
			                      	};

			//�������� �������� ����������
			if ((e.RangeValue == null) || (e.RangeField == null)
			    || (e.RangeField.Length != e.RangeValue.Length))
				throw new ArgumentException("�������� ��������� �� �������");
			//TODO: � �������� � ���� �������� ��� ����� ���� ����� �������� �������� ���� 
			//�� ����� Exception �� ����� ����������

			//�������� ���� ����� ��� ����������
			foreach (string fieldName in e.RangeField)
				if (!validRequestFields.ContainsKey(fieldName))
					throw new ArgumentException(String.Format("�� ���� {0} �� ����� ������������� ����������", fieldName), fieldName);
			//�������� ���� ����� ��� ����������
			if (e.SortField != null)
			{
				foreach (string fieldName in e.SortField)
					if (!validSortFields.Exists(value => String.Compare(fieldName, value, true) == 0))
						throw new ArgumentException(String.Format("�� ����� {0} �� ����� ������������� ����������", fieldName), fieldName);
			}
			//�������� ����������� ����������
			if (e.SortDirection != null)
			{
				foreach (string direction in e.SortDirection)
					if (!((String.Compare(direction, "Asc", true) == 0) || (String.Compare(direction, "Desc", true) == 0)))
						throw new ArgumentException(
							String.Format("�� ���������� �������� ����������� ��������� {0}. ���������� �������� \"Asc\" � \"Desc\"",
							              direction), direction);
			}

			//������� �������� ����� ����������� ����� � �������� ����������
			//           |            |
			//                ��� ����      ������ �������� ��� ����
			var FiltedField = new Dictionary<string, List<string>>();
			//����������� �������� ��������� args.RangeField � args.RangeValue � ������������ ���������� �����
			//������������� �� ����������
			for (int i = 0; i < e.RangeField.Length; i++)
			{
				//��������������� ���������� �������� ����� �� ����������
				string innerFieldName = validRequestFields[e.RangeField[i]];
				//���� � ������� �� ������ ���� 
				if (!FiltedField.ContainsKey(innerFieldName))
					//�� ��������� ��� � ������� ������ ��� �������� ��� ��������
					FiltedField.Add(innerFieldName, new List<string>());
				//��������� �������� ��� ����������
				FiltedField[innerFieldName].Add(e.RangeValue[i]);
			}

			if (FiltedField.Count == 0)
				throw new SoapException("��� ��������� ��� ����������", new XmlQualifiedName("0"));

			var data = new DataSet();

			string predicatBlock = "";

			if (e.NewEar)
				predicatBlock += " ampc.id is null ";

			foreach (string fieldName in FiltedField.Keys)
			{
				if (predicatBlock != "")
					predicatBlock += " and ";
				predicatBlock += Utils.StringArrayToQuery(FiltedField[fieldName], fieldName);
			}

			using (
				CleanUp.AfterGetOffers(new Common.MySql.MySqlHelper(e.DataAdapter.SelectCommand.Connection,
				                                                    e.DataAdapter.SelectCommand.Transaction)))
			{
				if (!e.OnlyLeader)
				{
					if (predicatBlock != "")
						predicatBlock = " and " + predicatBlock;

					e.DataAdapter.SelectCommand.CommandText = "CALL GetActivePrices(?ClientCode);";
					e.DataAdapter.SelectCommand.Parameters.AddWithValue("?ClientCode", e.ClientCode);
					e.DataAdapter.SelectCommand.ExecuteNonQuery();

					e.DataAdapter.SelectCommand.CommandText =
						@"
drop temporary table if exists offers;

create temporary table offers engine memory
SELECT  c.id OrderID,
		ifnull(c.Code, '') SalerCode,
		ifnull(c.CodeCr, '') CreaterCode,
		ifnull(ampc.code, '') ItemID,
		s.synonym OriginalName,
		ifnull(sfc.synonym, '') OriginalCr,
		ifnull(c.Unit, '') Unit,
		ifnull(c.Volume, '') Volume,
		ifnull(c.Quantity, '') Quantity,
		ifnull(c.Note, '') Note,
		ifnull(c.Period, '') Period,
		ifnull(c.Doc, '') Doc,
		c.Junk as Junk,
		ap.PublicUpCost as UpCost,
		ap.PriceCode as PriceCode,
		cd.ShortName SalerName,
		ap.PriceDate,
        c.ProductId as PrepCode,
		c.synonymcode OrderCode1,
		c.synonymfirmcrcode OrderCode2,
        if(if(round(cc.Cost*ap.UpCost,2)<c.MinBoundCost, c.MinBoundCost, round(cc.Cost*ap.UpCost,2))>c.MaxBoundCost, c.MaxBoundCost, if(round(cc.Cost*ap.UpCost,2)<c.MinBoundCost, c.MinBoundCost, round(cc.Cost*ap.UpCost,2))) as Cost
FROM farm.core0 c
	JOIN ActivePrices ap on c.PriceCode = ap.PriceCode
		JOIN farm.CoreCosts cc on cc.Core_Id = c.Id and cc.PC_CostCode = ap.CostCode
		JOIN UserSettings.ClientsData as cd ON ap.FirmCode = cd.FirmCode
	JOIN farm.synonym as s ON s.SynonymCode = c.SynonymCode	
	JOIN Catalogs.Products as p ON p.Id = c.ProductId
	LEFT JOIN farm.SynonymFirmCr as sfc ON sfc.SynonymFirmCrCode = c.SynonymFirmCrCode
	LEFT JOIN farm.core0 ampc ON ampc.ProductId = c.ProductId and ampc.codefirmcr = c.codefirmcr and ampc.PriceCode = 1864
WHERE ap.pricecode != 2647
";
					e.DataAdapter.SelectCommand.CommandText += predicatBlock;
					e.DataAdapter.SelectCommand.ExecuteNonQuery();
					e.DataAdapter.SelectCommand.CommandText = @"select * from offers ";
				}
				else
				{
					e.DataAdapter.SelectCommand.CommandText = "CALL GetOffers(?ClientCode, 0);";
					e.DataAdapter.SelectCommand.Parameters.AddWithValue("?ClientCode", e.ClientCode);
					e.DataAdapter.SelectCommand.ExecuteNonQuery();

					e.DataAdapter.SelectCommand.CommandText =
						@"
SELECT  c.id OrderID,
        ifnull(c.Code, '') SalerCode,
        ifnull(c.CodeCr, '') CreaterCode,
        ifnull(ampc.code, '') ItemID,
        s.synonym OriginalName,
        ifnull(sfc.synonym, '') OriginalCr,
        ifnull(c.Unit, '') Unit,
        ifnull(c.Volume, '') Volume,
        ifnull(c.Quantity, '') Quantity,
        ifnull(c.Note, '') Note,
        ifnull(c.Period, '') Period,
        ifnull(c.Doc, '') Doc,
        c.Junk as Junk,
        ap.PublicUpCost As UpCost,
        ap.pricecode PriceCode,
        cd.ShortName SalerName,
        ap.PriceDate,
        p.Id PrepCode,
        c.synonymcode OrderCode1,
        c.synonymfirmcrcode OrderCode2,
        offers.MinCost as Cost
FROM UserSettings.MinCosts as offers
	JOIN farm.core0 as c on c.id = offers.id
		JOIN UserSettings.activeprices as ap ON ap.PriceCode = offers.PriceCode
			  JOIN UserSettings.ClientsData as cd ON ap.FirmCode = cd.FirmCode
	JOIN farm.synonym as s ON s.SynonymCode = c.SynonymCode
	JOIN Catalogs.Products as p ON p.Id = c.ProductId
	LEFT JOIN farm.SynonymFirmCr as sfc ON sfc.SynonymFirmCrCode = c.SynonymFirmCrCode
    LEFT JOIN farm.core0 ampc ON ampc.ProductId = c.ProductId and ampc.codefirmcr = c.codefirmcr and ampc.PriceCode = 1864
";

					if (predicatBlock != "")
						predicatBlock = " where " + predicatBlock;

					e.DataAdapter.SelectCommand.CommandText += predicatBlock;
				}

				//����������� ����� �.�. � ������������� ������ ��� ����� ���� ��������� ������� 
				//��������������� � ���������� ProductId � CodeFirmCr �� ������ � ���� ��� 
				//�� ������ ��� ������ �� �����
				e.DataAdapter.SelectCommand.CommandText += " GROUP BY OrderID";
				e.DataAdapter.SelectCommand.CommandText += Utils.FormatOrderBlock(e.SortField, e.SortDirection);
				e.DataAdapter.SelectCommand.CommandText += Utils.GetLimitString(e.Offset, e.Count);
				e.DataAdapter.SelectCommand.Parameters.Clear();

				e.DataAdapter.Fill(data, "PriceList");
			}

			LogQuery(data, true, functionName,
			         new[]
			         	{
			         		new KeyValuePair<string, object>("NewEar", e.NewEar),
			         		new KeyValuePair<string, object>("OnlyLeader", e.OnlyLeader),
			         		new KeyValuePair<string, object>("RangeField", e.RangeField),
			         		new KeyValuePair<string, object>("RangeValue", e.RangeValue),
			         		new KeyValuePair<string, object>("SortField", e.SortField),
			         		new KeyValuePair<string, object>("SortDirection", e.SortDirection),
			         		new KeyValuePair<string, object>("SelStart", e.Offset),
			         		new KeyValuePair<string, object>("Limit", e.Count),
			         	});

			return data;
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
		public DataSet GetPrices(bool OnlyLeader, bool NewEar, string[] RangeField, string[] RangeValue, string[] SortField,
		                         string[] SortOrder, int Limit, int SelStart)
		{
			return MethodTemplate.ExecuteMethod<GetPricesArgs, DataSet>(
				new GetPricesArgs(OnlyLeader, NewEar, RangeField, RangeValue, SortField, SortOrder, Limit, SelStart),
				InnerGetPrices, null, GetClientCode);
		}

		/// <summary>
		/// �������� ������ ������� ��� ������� AMP.
		/// </summary>
		/// <param name="OrderID">������ ��������������� �������
		/// ���������� ��������: 
		///             1. "0" - ��� ����� ������ (*)
		///             2. "����� ������"
		///             3. "!" + "����� ������" - ��� ������ � ������� ����� ������ ��������.
		/// </param>
		/// <param name="PriceCode">
		/// ����� ������ �� �������� ������ �����.
		/// ���������: �������� ������ ��� ������ 0 ������������
		/// </param>
		/// <returns>������ �������</returns>
		[WebMethod]
		public DataSet GetOrders(string[] OrderID, int PriceCode)
		{
			return MethodTemplate.ExecuteMethod<GetOrdersArgs, DataSet>(
				new GetOrdersArgs(OrderID, PriceCode), InnerGetOrders, null, GetClientCode);
		}

		private DataSet InnerGetOrders(GetOrdersArgs args)
		{
			if (!HavePermission(GetUserName()))
				return null;

			Service.UpdateLastAccessTime(GetUserName());

			args.DataAdapter.SelectCommand.CommandText +=@"
SELECT  i.firmclientcode as ClientCode, 
        i.firmclientcode2 as ClientCode2, 
        i.firmclientcode3 as ClientCode3, 
        oh.RowID as OrderID,
        cast(oh.PriceDate as char) as PriceDate, 
        cast(oh.WriteTime as char) as OrderDate, 
        ifnull(oh.ClientAddition, '') as Comment,
        ol.Code as ItemID, 
        ol.Cost, 
        ol.Quantity, 
        if(length(FirmClientCode)< 1, concat(cd.shortname, '; ', cd.adress, '; ', 
		(select c.contactText
        from contacts.contact_groups cg
          join contacts.contacts c on cg.Id = c.ContactOwnerId
        where cd.ContactGroupOwnerId = cg.ContactGroupOwnerId
              and cg.Type = 0
              and c.Type = 1
        limit 1)), '') as Addition,
        if(length(FirmClientCode)< 1, cd.shortname, '') as ClientName
FROM UserSettings.PricesData pd 
	JOIN Orders.OrdersHead oh ON pd.PriceCode = oh.PriceCode 
	JOIN Orders.OrdersList ol ON oh.RowID = ol.OrderID 
	JOIN UserSettings.Intersection i ON i.ClientCode = oh.ClientCode and i.RegionCode= oh.RegionCode and i.PriceCode = oh.PriceCode
    JOIN UserSettings.ClientsData cd ON cd.FirmCode  = oh.ClientCode 
WHERE (pd.FirmCode = 62 or pd.FirmCode = 94)
	  and oh.Deleted = 0
	  and oh.Submited = 1";
			if (args.OrderID != null && args.OrderID.Length > 0)
			{
				if (args.OrderID[0] == "0")
				{
					args.DataAdapter.SelectCommand.CommandText += " and oh.Processed = 0 ";
				}
				else
				{
					if (args.OrderID[0].IndexOf("!") < 0)
						args.DataAdapter.SelectCommand.CommandText += " and " + Utils.StringArrayToQuery(args.OrderID, "ol.OrderID");
					else
						args.DataAdapter.SelectCommand.CommandText += " and ol.OrderID > " + args.OrderID[0].Replace("!", "");
				}
			}
			if (args.PriceCode > 0)
				args.DataAdapter.SelectCommand.CommandText += " and oh.PriceCode = " + args.PriceCode;

			var data = new DataSet();
			args.DataAdapter.Fill(data);

			LogQuery(data, false, "GetOrders");
			return data;
		}

		private string[] FormatFindStr(string InpStr, string ParameterName, string FieldName)
		{
			bool UseLike = false;
			string TmpRes = FieldName;
			var Result = new string[2];
			if (InpStr.IndexOf("*") >= 0)
			{
				UseLike = true;
			}
			if (InpStr.IndexOf("!") >= 0)
			{
				InpStr = InpStr.Remove(0, 1);
				if (UseLike)
					TmpRes += " not like ";
				else
					TmpRes += " <> ";
			}
			else
			{
				if (UseLike)
					TmpRes += " like ";
				else
					TmpRes += " = ";
			}
			InpStr = InpStr.Replace("*", "%");
			TmpRes += "?" + ParameterName;
			Result[0] = TmpRes;
			Result[1] = InpStr;
			return Result;
		}

		private ulong GetClientCode(MySqlConnection connection)
		{
			return GetClientCode(connection, GetUserName());
		}

		private static ulong GetClientCode(MySqlConnection connection, string userName)
		{
			userName = NormalizeUsername(userName);

			var command = connection.CreateCommand();
			command.CommandText = @"
SELECT osuseraccessright.clientcode 
FROM clientsdata, 
    osuseraccessright 
WHERE osuseraccessright.clientcode = clientsdata.firmcode 
    AND firmstatus = 1 
    AND billingstatus = 1 
    AND OSUserName = ?UserName";
			command.Parameters.AddWithValue("?UserName", userName);

			return Convert.ToUInt64(command.ExecuteScalar());
		}

		public static string NormalizeUsername(string username)
		{
			if (username.IndexOf("ANALIT\\") == 0)
				username = username.Substring(7);

			return username;
		}


		public Func<string> GetUserName = () => NormalizeUsername(HttpContext.Current.User.Identity.Name);

		public Func<string> GetHost = () => HttpContext.Current.Request.UserHostAddress;

		public Func<string, bool> HavePermission
			= userName =>
			  	{
			  		if (userName.IndexOf("ANALIT\\") == 0)
			  			userName = userName.Substring(7);

			  		using (var connection = new MySqlConnection(Literals.ConnectionString))
			  		{
						connection.Open();
			  			var command = new MySqlCommand(@"
select up.id
from usersettings.UserPermissions up
  join usersettings.AssignedPermissions ap on up.id = ap.permissionid
    join usersettings.osuseraccessright ouar on ouar.rowid = ap.userid
where up.Shortcut = 'IOL' and ouar.osusername = ?UserName", connection);
			  			command.Parameters.AddWithValue("?UserName", userName);
			  			return command.ExecuteScalar() != null;
			  		}
			  	};

		private void LogQuery(DataSet data, 
							  bool calculateUnique,
		                      string functionName, 
							  params KeyValuePair<string, object>[] arguments)
		{
			using (var connection = new MySqlConnection(Literals.ConnectionString))
			{
				connection.Open();

				var command = connection.CreateCommand();
				var rowCount = 0;
				foreach (DataTable table in data.Tables)
					rowCount += table.Rows.Count;

				if (calculateUnique)
				{
					command.CommandText =
						@" 
insert into logs.AMPLogs(LogTime, Host, User, Function, RowCount, ProcessingTime, UniqueCount, Arguments)
values (now(), ?Host, ?UserName, ?FunctionName, ?RowCount, ?ProcessTime,  ?UniqueCount, ?Arguments);
";
					command.Parameters.Add("?UniqueCount", CalculateUniqueFullCodeCount(data.Tables[0]));
				}
				else
					command.CommandText =
						@" 
insert into logs.AMPLogs(LogTime, Host, User, Function, RowCount, ProcessingTime, Arguments) 
values (now(), ?Host, ?UserName, ?FunctionName, ?RowCount, ?ProcessTime, ?Arguments);
";
				command.Parameters.Add("?Host", GetHost());
				command.Parameters.Add("?UserName", GetUserName());
				command.Parameters.Add("?functionName", functionName);
				command.Parameters.Add("?RowCount", rowCount);
				command.Parameters.Add("?ProcessTime", DateTime.Now.Subtract(StartTime).TotalMilliseconds);
				command.Parameters.Add("?Arguments", SerializeArguments(arguments));

				command.ExecuteNonQuery();
			}
		}

		private string SerializeArguments(KeyValuePair<string, object>[] arguments)
		{
			var serializedArguments = new List<string>();
			foreach (var argument in arguments)
				serializedArguments.Add(String.Format("{0} = ({1})", argument.Key, SerializeValue(argument.Value)));
			return String.Join(", ", serializedArguments.ToArray());
		}

		private string SerializeValue(object value)
		{
			if (value == null)
				return "null";
			var serializedValue = new List<string>();
			if (value is IEnumerable && !(value is String))
				foreach (object item in (IEnumerable) value)
					serializedValue.Add(item.ToString());
			else
				serializedValue.Add(value.ToString());
			return String.Join(", ", serializedValue.ToArray());
		}

		private int CalculateUniqueFullCodeCount(DataTable table)
		{
			var uniquePrepCodes = new List<int>();
			foreach (DataRow row in table.Rows)
			{
				if (!uniquePrepCodes.Contains(Convert.ToInt32(row["PrepCode"])))
					uniquePrepCodes.Add(Convert.ToInt32(row["PrepCode"]));
			}
			return uniquePrepCodes.Count;
		}
	}
}