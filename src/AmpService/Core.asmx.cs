using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services;
using Common.MySql;
using ExecuteTemplate;
using MySql.Data.MySqlClient;
using MySqlHelper=Common.MySql.MySqlHelper;

namespace AmpService
{
	[WebService(Namespace = "AMPNameSpace")]
    public class AMPService : WebService
    {
        MySqlTransaction MyTrans;
        DateTime StartTime = DateTime.Now;
        private DataColumn dataColumn18;
        private DataColumn dataColumn19;

        public AMPService()
        {
            InitializeComponent();
        }

        private IContainer components;
        public MySqlCommand MySelCmd;
        public MySqlConnection MyCn;
        private MySqlDataAdapter MyDA;
        public DataSet MyDS;
        public DataTable DataTable1;
        public DataColumn DataColumn1;
        public DataColumn DataColumn2;
        public DataColumn DataColumn3;
        public DataColumn DataColumn4;
        public DataColumn DataColumn5;
        public DataColumn DataColumn6;
        public DataColumn DataColumn7;
        public DataColumn DataColumn8;
        public DataColumn DataColumn9;
        public DataColumn DataColumn10;
        public DataColumn DataColumn11;
        public DataColumn DataColumn12;
        public DataColumn DataColumn13;
        public DataColumn DataColumn14;
        public DataColumn DataColumn15;
        public DataColumn DataColumn16;
        public DataColumn DataColumn17;

        private void InitializeComponent()
        {
            MySelCmd = new MySqlCommand();
            MyCn = new MySqlConnection();
            MyDA = new MySqlDataAdapter();
            MyDS = new DataSet();
            DataTable1 = new DataTable();
            DataColumn1 = new DataColumn();
            DataColumn2 = new DataColumn();
            DataColumn3 = new DataColumn();
            DataColumn4 = new DataColumn();
            DataColumn5 = new DataColumn();
            DataColumn6 = new DataColumn();
            DataColumn7 = new DataColumn();
            DataColumn8 = new DataColumn();
            DataColumn9 = new DataColumn();
            DataColumn10 = new DataColumn();
            DataColumn11 = new DataColumn();
            DataColumn12 = new DataColumn();
            DataColumn13 = new DataColumn();
            DataColumn14 = new DataColumn();
            DataColumn15 = new DataColumn();
            DataColumn16 = new DataColumn();
            DataColumn17 = new DataColumn();
            dataColumn18 = new DataColumn();
            dataColumn19 = new DataColumn();
            MyDS.BeginInit();
            DataTable1.BeginInit();
            MySelCmd.CommandTimeout = 0;
            MySelCmd.CommandType = CommandType.Text;
            MySelCmd.Connection = MyCn;
            MySelCmd.Transaction = null;
            MySelCmd.UpdatedRowSource = UpdateRowSource.Both;
            MyCn.ConnectionString = Literals.ConnectionString;
            MyDA.DeleteCommand = null;
            MyDA.InsertCommand = null;
            MyDA.SelectCommand = MySelCmd;
            MyDA.UpdateCommand = null;
            MyDS.DataSetName = "AMPDataSet";
            MyDS.Locale = new CultureInfo("ru-RU");
            MyDS.Tables.AddRange(new DataTable[] { DataTable1 });
            DataTable1.Columns.AddRange(
                    new DataColumn[]
                                        {
                                                DataColumn1, DataColumn2, DataColumn3, DataColumn4, DataColumn5, DataColumn6,
                                                DataColumn7, DataColumn8, DataColumn9, DataColumn10, DataColumn11, DataColumn12,
                                                DataColumn13, DataColumn14, DataColumn15, DataColumn16, DataColumn17, dataColumn18,
                                                dataColumn19
                                        });
            DataTable1.TableName = "Prices";
            DataColumn1.ColumnName = "OrderID";
            DataColumn1.DataType = typeof(long);
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
            DataColumn12.DataType = typeof(bool);
            DataColumn13.ColumnName = "UpCost";
            DataColumn13.DataType = typeof(decimal);
            DataColumn14.ColumnName = "Cost";
            DataColumn14.DataType = typeof(decimal);
            DataColumn15.ColumnName = "SalerID";
            DataColumn15.DataType = typeof(uint);
            DataColumn16.ColumnName = "PriceDate";
            DataColumn17.ColumnName = "PrepCode";
            DataColumn17.DataType = typeof(uint);
            dataColumn18.ColumnName = "OrderCode1";
            dataColumn18.DataType = typeof(uint);
            dataColumn19.ColumnName = "OrderCode2";
            dataColumn19.DataType = typeof(uint);
            MyDS.EndInit();
            DataTable1.EndInit();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!((components == null)))
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
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
            MyDS.Tables.Remove("Prices");
	        Restart:
            try
            {
				using (MySqlConnection connection = new MySqlConnection(Literals.ConnectionString))
				{
					connection.Open();
					using (MySqlTransaction transaction = connection.BeginTransaction(IsolationLevel.RepeatableRead))
					{
						try
						{
							ulong clientCode = GetClientCode(MyCn);
							StringBuilder commandText = new StringBuilder();

							int i;
							bool searchByApmCode = Name != null && Name.Length > 0 && int.TryParse(Name[0], out i);

							using (CleanUp.AfterGetActivePrices(new MySqlHelper(connection, transaction)))
							{
								if (OfferOnly)
								{
									MySqlCommand command = new MySqlCommand("CALL GetActivePrices(?ClientCode);", connection, transaction);
									command.Parameters.AddWithValue("?ClientCode", clientCode);
									command.ExecuteNonQuery();

									commandText.AppendLine(
										@"
SELECT	p.id as PrepCode, 
		cn.Name, 
		cf.Form,
		ifnull(group_concat(distinct pv.Value ORDER BY prop.PropertyName, pv.Value SEPARATOR ', '), '') as Properties
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
		ifnull(group_concat(distinct pv.Value ORDER BY prop.PropertyName, pv.Value SEPARATOR ', '), '') as Properties
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

								MySqlDataAdapter adapter = new MySqlDataAdapter(commandText.ToString(), connection);
								adapter.SelectCommand.Transaction = transaction;
								adapter.SelectCommand.Parameters.AddWithValue("?ClientCode", clientCode);
								adapter.Fill(MyDS, "Catalog");

								LogQuery(adapter.SelectCommand, MyDS, false, MethodBase.GetCurrentMethod().Name,
								         new KeyValuePair<string, object>("Name", Name),
								         new KeyValuePair<string, object>("Form", Form),
								         new KeyValuePair<string, object>("NewEar", NewEar),
								         new KeyValuePair<string, object>("OfferOnly", OfferOnly),
								         new KeyValuePair<string, object>("PriceID", PriceID),
								         new KeyValuePair<string, object>("Limit", Limit),
								         new KeyValuePair<string, object>("SelStart", SelStart));
							}

							transaction.Commit();
							return MyDS;
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
            return MyDS;
        }

        [WebMethod()]
        public DataSet PostOrder(long[] OrderID, Int32[] Quantity, string[] Message, Int32[] OrderCode1, Int32[] OrderCode2,
                                 bool[] Junk)
        {
            return MethodTemplate.ExecuteMethod<PostOrderArgs, DataSet>(
				new PostOrderArgs(OrderID, Quantity, Message, OrderCode1, OrderCode2, Junk),
				InnerPostOrder, null, MyCn, GetClientCode);
        }

        private DataSet InnerPostOrder(PostOrderArgs e)
        {
            string CoreIDString;
            int Index;
            DataSet dsRes;
            DataTable dtPricesRes;

            long[] Res = new long[e.CoreIDs.Length];
            for (int i = 0; i < Res.Length; i++)
            {
                Res[i] = -1;
            }
            DataSet dsPost;
            DataTable dtSummaryOrder;
            DataTable dtOrderHead;

            //Если длины не равны, то возвращаем null
            if ((e.CoreIDs == null) || (e.Quantities == null) || (e.SynonymCodes == null) || (e.SynonymFirmCrCodes == null)
                || (e.Junks == null)
                || (e.CoreIDs.Length != e.Quantities.Length)
                || (e.CoreIDs.Length != e.SynonymCodes.Length) || (e.CoreIDs.Length != e.SynonymFirmCrCodes.Length)
                || (e.CoreIDs.Length != e.Junks.Length))
                return null;

            CoreIDString = "(";
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

            dsPost = new DataSet();

			using (CleanUp.AfterGetActivePrices(new MySqlHelper(e.DataAdapter.SelectCommand.Connection, e.DataAdapter.SelectCommand.Transaction)))
			{
				e.DataAdapter.SelectCommand.CommandText = "CALL GetActivePrices(?ClientCode);";
				e.DataAdapter.SelectCommand.Parameters.Clear();
				e.DataAdapter.SelectCommand.Parameters.AddWithValue("?ClientCode", e.ClientCode);
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
        length(c.Junk) > 0 Junk,
		length(c.Await) > 0 Await,
		c.BaseCost,
		round(if(if(ap.costtype=0, corecosts.cost, c.basecost) * ap.UpCost < c.minboundcost, c.minboundcost, if(ap.costtype=0, corecosts.cost, c.basecost) * ap.UpCost),2) as Cost
FROM    (farm.formrules fr,
        usersettings.clientsdata,
        (farm.core0 c, ActivePrices ap)
          LEFT JOIN farm.corecosts
            ON corecosts.Core_Id     = c.id
              AND corecosts.PC_CostCode = ap.CostCode),
		UserSettings.ClientsData cd
WHERE c.PriceCode                          = if(ap.costtype=0, ap.PriceCode, ap.CostCode)
    AND fr.PriceCode                       = ap.pricecode
    AND clientsdata.firmcode               = ap.firmcode
	AND if(ap.costtype = 0, corecosts.cost is not null, c.basecost is not null)
	AND cd.FirmCode							= ?ClientCode
	AND c.ID in " +
					CoreIDString;

				e.DataAdapter.SelectCommand.Parameters.Clear();
				e.DataAdapter.SelectCommand.Parameters.Add("?ClientCode", e.ClientCode);
				e.DataAdapter.Fill(dsPost, "SummaryOrder");
			}
        	dtSummaryOrder = dsPost.Tables["SummaryOrder"];

            DataRow[] drs;
            dtSummaryOrder.Columns.Add(new DataColumn("Message", typeof(string)));
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

            dtOrderHead = dtSummaryOrder.DefaultView.ToTable(true, "ClientCode", "RegionCode", "PriceCode", "PriceDate");
            dtOrderHead.Columns.Add(new DataColumn("OrderID", typeof(long)));

            DataRow[] drOrderList;
            foreach (DataRow drOH in dtOrderHead.Rows)
            {
                drOrderList = dtSummaryOrder.Select("PriceCode = " + drOH["PriceCode"].ToString());
                if (drOrderList.Length > 0)
                {
                    e.DataAdapter.SelectCommand.CommandText =
                            "insert into orders.ordershead (WriteTime, ClientCode, PriceCode, RegionCode, PriceDate, RowCount, ClientAddition, Processed)" +
                            "values(now(), ?ClientCode, ?PriceCode, ?RegionCode, ?PriceDate, ?RowCount, ?ClientAddition, 0);";
                    e.DataAdapter.SelectCommand.CommandText += "select LAST_INSERT_ID()";
                    e.DataAdapter.SelectCommand.Parameters.Clear();
                    e.DataAdapter.SelectCommand.Parameters.Add("?ClientCode", drOH["ClientCode"]);
                    e.DataAdapter.SelectCommand.Parameters.Add("?PriceCode", drOH["PriceCode"]);
                    e.DataAdapter.SelectCommand.Parameters.Add("?RegionCode", drOH["RegionCode"]);
                    e.DataAdapter.SelectCommand.Parameters.Add("?PriceDate", drOH["PriceDate"]);
                    e.DataAdapter.SelectCommand.Parameters.Add("?RowCount", drOrderList.Length);
                    e.DataAdapter.SelectCommand.Parameters.Add("?ClientAddition", drOrderList[0]["Message"]);
                    drOH["OrderID"] = Convert.ToInt64(e.DataAdapter.SelectCommand.ExecuteScalar());
                    e.DataAdapter.SelectCommand.CommandText =
                            "insert into orders.orderslist (OrderID, ProductId, CodeFirmCr, SynonymCode, SynonymFirmCrCode, Code, CodeCr, Quantity, Junk, Await, Cost) values (?OrderID, ?ProductId, ?CodeFirmCr, ?SynonymCode, ?SynonymFirmCrCode, ?Code, ?CodeCr, ?Quantity, ?Junk, ?Await, ?Cost);";
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
                        e.DataAdapter.SelectCommand.ExecuteNonQuery();
                        Index = Array.IndexOf(e.CoreIDs, Convert.ToInt64(drOL["ID"]));
                        if (Index > -1)
                            Res[Index] = Convert.ToInt64(drOH["OrderID"]);
                    }
                }
            }

            //начинаем заполнять таблицу результатов
			dtPricesRes = AmpService.PostOrder.GetPricesDataTable();
            dtPricesRes.TableName = "Prices";

            DataRow drOK;

            DataTable dtTemp;

			using (CleanUp.AfterGetActivePrices(new MySqlHelper(e.DataAdapter.SelectCommand.Connection, e.DataAdapter.SelectCommand.Transaction)))
			{
				e.DataAdapter.SelectCommand.CommandText =
					@"
CALL GetActivePrices(?ClientCode);

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
        length(c.Junk) > 0 Junk,
        ap.PublicUpCost As UpCost,
		round(if(if(ap.costtype=0, corecosts.cost, c.basecost) * ap.UpCost < c.minboundcost, c.minboundcost, if(ap.costtype=0, corecosts.cost, c.basecost) * ap.UpCost),2) as Cost,
        ap.pricecode SalerID,
        ClientsData.ShortName SalerName,
        ap.PriceDate,
        c.ProductId PrepCode,
        c.synonymcode OrderCode1,
        c.synonymfirmcrcode OrderCode2
FROM    (farm.formrules fr,
        farm.synonym s,
        usersettings.clientsdata,
        (farm.core0 c, ActivePrices ap)
          LEFT JOIN farm.corecosts
            ON corecosts.Core_Id     = c.id
              AND corecosts.PC_CostCode = ap.CostCode)
          LEFT JOIN farm.core0 ampc
      			ON ampc.ProductId            = c.ProductId
 			      	and ampc.codefirmcr      = c.codefirmcr
      				and ampc.PriceCode       = 1864
          LEFT JOIN farm.synonymfirmcr scr
            ON scr.PriceCode             = ifnull(fr.ParentSynonym, ap.pricecode)
            and c.synonymfirmcrcode     = scr.synonymfirmcrcode
WHERE c.PriceCode                          = if(ap.costtype=0, ap.PriceCode, ap.CostCode)
    AND fr.PriceCode                        = ap.pricecode
    and c.synonymcode                       = s.synonymcode
    and s.PriceCode                         = ifnull(fr.parentsynonym, ap.pricecode)
    and clientsdata.firmcode                = ap.firmcode
	and if(ap.costtype = 0, corecosts.cost is not null, c.basecost is not null)
    and c.SynonymCode = ?SynonymCode
    and c.SynonymFirmCrCode = ?SynonymFirmCrCode
    and (length(c.Junk) > 0 = ?Junk)";

				e.DataAdapter.SelectCommand.Parameters.Clear();
				e.DataAdapter.SelectCommand.Parameters.Add("?ClientCode", e.ClientCode);
				e.DataAdapter.SelectCommand.Parameters.Add("?SynonymCode", MySqlDbType.Int32);
				e.DataAdapter.SelectCommand.Parameters.Add("?SynonymFirmCrCode", MySqlDbType.Int32);
				e.DataAdapter.SelectCommand.Parameters.Add("?Junk", MySqlDbType.Bit);

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


        	//Формируем результирующий датасет
            dsRes = new DataSet();
            dsRes.Tables.Add(dtPricesRes);

            /*
             PostOrder - отправка заказов по коду позиции

            Входные данные:
            OrderID() int(32) unsigned - массив кодов(Prices.OrderID) для заказа препаратов
            Quantity() int(32) unsigned - массив количеств заказываемых препаратов

            Ответ:
            PostOrder() Int(32) unsigned - массив кодов подтверждения, коды принимают значения:
            PostOrder >=0 - код сформированного заказа(Инфорум)
            PostOrder=-1 - ошибка формирования заказа.

             */
			LogQuery(e.DataAdapter.SelectCommand, dsRes, false, "PostOrder");
            return dsRes;
        }

        private DataSet InnerGetPriceCodeByName(FirmNameArgs e)
        {
            e.DataAdapter.SelectCommand.CommandText =
                    @"
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


            e.DataAdapter.SelectCommand.CommandText += " and " + Utils.StringArrayToQuery(e.FirmNames, "ClientsData.ShortName");

            e.DataAdapter.SelectCommand.Parameters.Add("?ClientCode", e.ClientCode);

            DataSet data = new DataSet();
            e.DataAdapter.Fill(data, "PriceList");
			this.LogQuery(e.DataAdapter.SelectCommand, data, false, "GetPriceCodeByName");
            return data;
        }

        [WebMethod()]
        public DataSet GetPriceCodeByName(string[] firmName)
        {
            return MethodTemplate.ExecuteMethod<FirmNameArgs, DataSet>(
				new FirmNameArgs(firmName), InnerGetPriceCodeByName, null, GetClientCode);
        }

        private DataSet InnerGetPrices(GetPricesArgs e)
        {
            string functionName = "GetPrices";
            //словарь для валидации и трансляции имен полей для клиента в имена полей для использования в запросе
            Dictionary<string, string> validRequestFields = new Dictionary<string, string>(new CaseInsensitiveStringComparer());
            validRequestFields.Add("OrderID", "c.id");
            validRequestFields.Add("SalerCode", "c.Code");
            validRequestFields.Add("SalerName", "cd.ShortName");
            validRequestFields.Add("ItemID", "ampc.Code");
			validRequestFields.Add("OriginalCR", "sfc.Synonym");
            validRequestFields.Add("OriginalName", "s.Synonym");
            validRequestFields.Add("PriceCode", "ap.PriceCode");
            validRequestFields.Add("PrepCode", "p.Id");

            List<string> validSortFields = new List<string>();
            validSortFields.Add("OrderID");
            validSortFields.Add("SalerCode");
            validSortFields.Add("CreaterCode");
            validSortFields.Add("ItemID");
            validSortFields.Add("OriginalName");
            validSortFields.Add("OriginalCR");
            validSortFields.Add("Unit");
            validSortFields.Add("Volume");
            validSortFields.Add("Quantity");
            validSortFields.Add("Note");
            validSortFields.Add("Period");
            validSortFields.Add("Doc");
            validSortFields.Add("Junk");
            validSortFields.Add("Upcost");
            validSortFields.Add("Cost");
            validSortFields.Add("PriceCode");
            validSortFields.Add("SalerName");
            validSortFields.Add("PriceDate");
            validSortFields.Add("PrepCode");
            validSortFields.Add("OrderCode1");
            validSortFields.Add("OrderCode2");

            //проверка входящих параметров
            if ((e.RangeValue == null) || (e.RangeField == null)
                || (e.RangeField.Length != e.RangeValue.Length))
				throw new ArgumentException("Входящие параметры не валидны");
            //TODO: в принципе в этой проверке нет нужды если будет неверное название поля 
            //то будет Exception на этапе трансляции

            //проверка имен полей для фильтрации
            foreach (string fieldName in e.RangeField)
                if (!validRequestFields.ContainsKey(fieldName))
                    throw new ArgumentException(String.Format("По полю {0} не может производиться фильтрация", fieldName), fieldName);
            //проверка имен полей для сортировки
            if (e.SortField != null)
            {
                foreach (string fieldName in e.SortField)
                    if (!validSortFields.Exists(delegate(string value)
                                                    {
                                                        return String.Compare(fieldName, value, true) == 0;
                                                    }))
                        throw new ArgumentException(String.Format("По поляю {0} не может производиться сортировка", fieldName), fieldName);
            }
            //проверка направлений сортировки
            if (e.SortDirection != null)
            {
                foreach (string direction in e.SortDirection)
                    if (!((String.Compare(direction, "Asc", true) == 0) || (String.Compare(direction, "Desc", true) == 0)))
                        throw new ArgumentException(
                                String.Format("Не допустимое значение направления сортровки {0}. Допустимые значение \"Asc\" и \"Desc\"",
                                              direction), direction);
            }

            //словарь хранящий имена фильтруемых полей и значения фильтрации
            //           |            |
            //                имя поля      список значений для него
            Dictionary<string, List<string>> FiltedField = new Dictionary<string, List<string>>();
            //разбирается входящие параметры args.RangeField и args.RangeValue и одновременно клиентские имена
            //транслируются во внутренние
            for (int i = 0; i < e.RangeField.Length; i++)
            {
                //преобразовываем клиентские названия полей во внутренние
                string innerFieldName = validRequestFields[e.RangeField[i]];
                //если в словаре не такого поля 
                if (!FiltedField.ContainsKey(innerFieldName))
                    //то добавляем его и создаем массив для хранения его значений
                    FiltedField.Add(innerFieldName, new List<string>());
                //добавляем значение для фильтрации
                FiltedField[innerFieldName].Add(e.RangeValue[i]);
            }

			DataSet data = new DataSet();

			using (CleanUp.AfterGetActivePrices(new MySqlHelper(e.DataAdapter.SelectCommand.Connection, e.DataAdapter.SelectCommand.Transaction)))
			{
				e.DataAdapter.SelectCommand.CommandText = "CALL GetOffers(?ClientCode, 0);";
				e.DataAdapter.SelectCommand.Parameters.AddWithValue("?ClientCode", e.ClientCode);
				e.DataAdapter.SelectCommand.ExecuteNonQuery();

				if (!e.OnlyLeader)
				{
					e.DataAdapter.SelectCommand.CommandText = @"
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
        length(c.Junk) > 0 Junk,
        ap.PublicUpCost As UpCost,
        ap.pricecode PriceCode,
        cd.ShortName SalerName,
        ap.PriceDate,
        p.Id PrepCode,
        c.synonymcode OrderCode1,
        c.synonymfirmcrcode OrderCode2,
        offers.Cost as Cost
FROM core as offers
	JOIN farm.core0 as c on c.id = offers.id
		JOIN activeprices as ap ON ap.PriceCode = offers.PriceCode
			  JOIN UserSettings.ClientsData as cd ON ap.FirmCode = cd.FirmCode
	JOIN farm.synonym as s ON s.SynonymCode = c.SynonymCode	
	JOIN Catalogs.Products as p ON p.Id = c.ProductId
	LEFT JOIN farm.SynonymFirmCr as sfc ON sfc.SynonymFirmCrCode = c.SynonymFirmCrCode
	LEFT JOIN farm.core0 ampc ON ampc.ProductId = c.ProductId and ampc.codefirmcr = c.codefirmcr and ampc.PriceCode = 1864
";
				}
				else
				{
					e.DataAdapter.SelectCommand.CommandText = @"
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
        length(c.Junk) > 0 Junk,
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
				}

				string predicatBlock = "";

				if (e.NewEar)
					predicatBlock += " ampc.id is null ";

				foreach (string fieldName in FiltedField.Keys)
				{
					if (predicatBlock != "")
						predicatBlock += " and ";
					predicatBlock += Utils.StringArrayToQuery(FiltedField[fieldName], fieldName);
				}

				if (predicatBlock != "")
					e.DataAdapter.SelectCommand.CommandText += "WHERE " + predicatBlock;

				//группировка нужна т.к. в асортиментном прайсе амп может быть несколько записей 
				//соответствующие с одинаковым ProductId и CodeFirmCr но смысла в этом нет 
				//ни какого они просто не нужны
				e.DataAdapter.SelectCommand.CommandText += " GROUP BY c.Id";
				e.DataAdapter.SelectCommand.CommandText += Utils.FormatOrderBlock(e.SortField, e.SortDirection);
				e.DataAdapter.SelectCommand.Parameters.Clear();

				e.DataAdapter.Fill(data, "PriceList");
			}			

			LogQuery(e.DataAdapter.SelectCommand, data, true, functionName);

            return data;
        }

        /// <summary>
        /// Выбирает список список позиций прайс листов, основываясь на полях 
        /// фильтрации из RangeField и их значениях из RangeValue.
        /// </summary>
        /// <param name="OnlyLeader">Выбирать только позиции имеющие минимальную цену для заданного поставщика</param>
        /// <param name="NewEar"></param>
        /// <param name="RangeField">Список полей для которых осуществляется фильтрация. 
        /// Допустимые значения: PrepCode Int, PriceCode Int, ItemID String, OriginalName String или null
        /// </param>
        /// <param name="RangeValue">Список значений ко которым происходит фильтрация.
        /// Допустимые значения: Int, String - строки могут содержать метасимвол "%".
        /// Примечание: количество значений должно совпадать с количеством полей.
        /// </param>
        /// <param name="SortField">Поля по которым может осуществляться сортировка. 
        /// Допустимые значения: null, OrderID, SalerCode, CreaterCode, ItemID, OriginalName, 
        /// OriginalCr, Unit, Volume, Quantity, Note, Period, Doc, Junk, UpCost, Cost, PriceCode, 
        /// SalerName, PriceDate, PrepCode, OrderCode1, OrderCode2.
        /// </param>
        /// <param name="SortOrder">Порядок сортировки для полей из SortField.
        /// Допустимые значения: "ASC" - прямой порядок сортировки , "DESC" - обратный порядок сортовки.
        /// Примечание: Количество значений может совпадать или быть меньще количества полей для сортировки.
        /// Если количество значений меньше то для оставщихся полей бедут применен прямой порядок сортировки (ASC). 
        /// Пример: SortField = {"OrderID", "Junk" ,"Cost"}, SortOrder = {"DESC"} - в этом случае выборка будет 
        /// отсортированна так же как и в случае если бы  SortOrder = {"DESC", "ASC", "ASC"}
        /// </param>
        /// <param name="Limit">Количество записей в выборке. 
        /// Примечание: если Limit меньше SelStart то тогда выбираются все записи начиная с SelStart.
        /// </param>
        /// <param name="SelStart">Значение с которого начинается выбор. 
        /// Примечание: следует помнить что первым значением является 0 а не 1.
        /// </param>
        /// <returns>DataSet содержащий позиции прайс листов.</returns>
        [WebMethod()]
        public DataSet GetPrices(bool OnlyLeader, bool NewEar, string[] RangeField, string[] RangeValue, string[] SortField,
                                 string[] SortOrder, int Limit, int SelStart)
        {
            return MethodTemplate.ExecuteMethod<GetPricesArgs, DataSet>(
				new GetPricesArgs(OnlyLeader, NewEar, RangeField, RangeValue, SortField, SortOrder, Limit, SelStart),
				InnerGetPrices, null, GetClientCode);
        }

        /// <summary>
        /// Получает список заказов для клиента AMP.
        /// </summary>
        /// <param name="OrderID">Массив идентификаторов заказов
        /// Допустимые значения: 
        ///             1. "0" - все новые заказы (*)
        ///             2. "Номер заказа"
        ///             3. "!" + "Номер заказа" - все заказы у которых номер больше заданого.
        /// </param>
        /// <param name="PriceCode">
        /// Номер прайса по которому сделан заказ.
        /// Прмечание: значение равное или меньше 0 игнорируются
        /// </param>
        /// <returns>Список заказов</returns>
        [WebMethod()]
        public DataSet GetOrders(string[] OrderID, int PriceCode)
        {
            return MethodTemplate.ExecuteMethod<GetOrdersArgs, DataSet>(
				new GetOrdersArgs(OrderID, PriceCode), InnerGetOrders, null, GetClientCode);
        }

        private DataSet InnerGetOrders(ExecuteArgs e)
        {
            GetOrdersArgs args = e as GetOrdersArgs;	

            args.DataAdapter.SelectCommand.CommandText +=
                    @"
                                SELECT  
                                        i.firmclientcode as ClientCode, 
                                        i.firmclientcode2 as ClientCode2, 
                                        i.firmclientcode3 as ClientCode3, 
                                        oh.RowID as OrderID,
                                        cast(oh.PriceDate as char) as PriceDate, 
                                        cast(oh.WriteTime as char) as OrderDate, 
                                        ifnull(oh.ClientAddition, '') as Comment,
                                        ol.Code           as ItemID, 
                                        ol.Cost, 
                                        ol.Quantity, 
                                        if(length(FirmClientCode)< 1, concat(cd.shortname, '; ', cd.adress, '; ', cd.phone), '') as Addition,
                                        if(length(FirmClientCode)< 1, cd.shortname, '') as ClientName
                                FROM    UserSettings.PricesData pd 
                                INNER JOIN Orders.OrdersHead oh 
                                ON pd.PriceCode = oh.PriceCode 
                                INNER JOIN Orders.OrdersList ol 
                                ON oh.RowID = ol.OrderID 
                                INNER JOIN UserSettings.Intersection i 
                                ON i.ClientCode = oh.ClientCode 
                                and i.RegionCode= oh.RegionCode 
                                and i.PriceCode = oh.PriceCode 
                                INNER JOIN UserSettings.ClientsData cd 
                                ON cd.FirmCode  = oh.ClientCode 
                                WHERE  pd.FirmCode = 62
";
            if (args.OrderID != null)
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
                args.DataAdapter.SelectCommand.CommandText += " and oh.PriceCode = " + args.PriceCode.ToString();

            DataSet data = new DataSet();
            args.DataAdapter.Fill(data);

			LogQuery(e.DataAdapter.SelectCommand, data, false, "GetOrders");
            return data;
        }

        private string[] FormatFindStr(string InpStr, string ParameterName, string FieldName)
        {
            bool UseLike = false;
            string TmpRes = FieldName;
            string[] Result = new string[2];
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

		private static ulong GetClientCode(MySqlConnection connection)
		{
			return GetClientCode(connection, GetUserName());
		}

		private static ulong GetClientCode(MySqlConnection connection, string userName)
		{
			if (userName.IndexOf("ANALIT\\") == 0)
				userName = userName.Substring(7);

			return Convert.ToUInt64(MySql.Data.MySqlClient.MySqlHelper.ExecuteScalar(Literals.ConnectionString,
@"
SELECT 
    osuseraccessright.clientcode 
FROM clientsdata, 
    osuseraccessright 
WHERE osuseraccessright.clientcode = clientsdata.firmcode 
    AND firmstatus = 1 
    AND billingstatus = 1 
    AND OSUserName = ?UserName
",
					new MySqlParameter[] { new MySqlParameter("?UserName", userName) }));
		}


		private static string GetUserName()
		{
#if DEBUG
			return "kvasov";
#else
			return HttpContext.Current.User.Identity.Name;
#endif
		}

		private static string GetHost()
		{
#if DEBUG
			return "prg3";
#else
			return HttpContext.Current.Request.UserHostAddress;
#endif
		}

		private void LogQuery(MySqlCommand command, DataSet data, bool calculateUnique, 
			string functionName, params KeyValuePair<string, object>[] arguments)
        {
			int rowCount = 0;
			foreach (DataTable table in data.Tables)
				rowCount += table.Rows.Count;

            string oldQuery = command.CommandText;
			if (calculateUnique)
			{
				command.CommandText = @" 
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
            command.CommandText = oldQuery;
        }

		private string SerializeArguments(KeyValuePair<string, object>[] arguments)
		{			
			List<string> serializedArguments = new List<string>();
			foreach (KeyValuePair<string, object> argument in arguments)
				serializedArguments.Add(String.Format("{0} = ({1})", argument.Key, SerializeValue(argument.Value)));
			return String.Join(", ", serializedArguments.ToArray());
		}

		private string SerializeValue(object value)
		{
			if (value == null)
				return "null";
			List<string> serializedValue = new List<string>();
			if (value is IEnumerable && !(value is String))
				foreach (object item in (IEnumerable)value)
					serializedValue.Add(item.ToString());
			else
				serializedValue.Add(value.ToString());
			return String.Join(", ", serializedValue.ToArray());
		}

        private int CalculateUniqueFullCodeCount(DataTable table)
        {
            List<int> uniquePrepCodes = new List<int>();
            foreach (DataRow row in table.Rows)
            {
                if (!uniquePrepCodes.Contains(Convert.ToInt32(row["PrepCode"])))
                    uniquePrepCodes.Add(Convert.ToInt32(row["PrepCode"]));
            }
			return uniquePrepCodes.Count;
        }
    }
}