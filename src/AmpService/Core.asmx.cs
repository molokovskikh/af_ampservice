using System;
using System.Web.Services;
using MySql.Data.MySqlClient;
using System.Data;
using System.Net.Mail;
using System.Collections.Generic;

namespace AMPWebService
{
	[System.Web.Services.WebService(Namespace = "AMPNameSpace")]
	public class AMPService : System.Web.Services.WebService
	{
		MySqlTransaction MyTrans;
		string UserName;
		string FunctionName;
		DateTime StartTime = DateTime.Now;
		private System.Data.DataColumn dataColumn18;
		private System.Data.DataColumn dataColumn19;

		public AMPService() : base()
		{
			InitializeComponent();
		}

		private System.ComponentModel.IContainer components;
		public MySql.Data.MySqlClient.MySqlCommand MySelCmd;
		public MySql.Data.MySqlClient.MySqlConnection MyCn;
		private MySql.Data.MySqlClient.MySqlDataAdapter MyDA;
		public System.Data.DataSet MyDS;
		public System.Data.DataTable DataTable1;
		public System.Data.DataColumn DataColumn1;
		public System.Data.DataColumn DataColumn2;
		public System.Data.DataColumn DataColumn3;
		public System.Data.DataColumn DataColumn4;
		public System.Data.DataColumn DataColumn5;
		public System.Data.DataColumn DataColumn6;
		public System.Data.DataColumn DataColumn7;
		public System.Data.DataColumn DataColumn8;
		public System.Data.DataColumn DataColumn9;
		public System.Data.DataColumn DataColumn10;
		public System.Data.DataColumn DataColumn11;
		public System.Data.DataColumn DataColumn12;
		public System.Data.DataColumn DataColumn13;
		public System.Data.DataColumn DataColumn14;
		public System.Data.DataColumn DataColumn15;
		public System.Data.DataColumn DataColumn16;
		public System.Data.DataColumn DataColumn17;

		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.MySelCmd = new MySql.Data.MySqlClient.MySqlCommand();
			this.MyCn = new MySql.Data.MySqlClient.MySqlConnection();
			this.MyDA = new MySql.Data.MySqlClient.MySqlDataAdapter();
			this.MyDS = new System.Data.DataSet();
			this.DataTable1 = new System.Data.DataTable();
			this.DataColumn1 = new System.Data.DataColumn();
			this.DataColumn2 = new System.Data.DataColumn();
			this.DataColumn3 = new System.Data.DataColumn();
			this.DataColumn4 = new System.Data.DataColumn();
			this.DataColumn5 = new System.Data.DataColumn();
			this.DataColumn6 = new System.Data.DataColumn();
			this.DataColumn7 = new System.Data.DataColumn();
			this.DataColumn8 = new System.Data.DataColumn();
			this.DataColumn9 = new System.Data.DataColumn();
			this.DataColumn10 = new System.Data.DataColumn();
			this.DataColumn11 = new System.Data.DataColumn();
			this.DataColumn12 = new System.Data.DataColumn();
			this.DataColumn13 = new System.Data.DataColumn();
			this.DataColumn14 = new System.Data.DataColumn();
			this.DataColumn15 = new System.Data.DataColumn();
			this.DataColumn16 = new System.Data.DataColumn();
			this.DataColumn17 = new System.Data.DataColumn();
			this.dataColumn18 = new System.Data.DataColumn();
			this.dataColumn19 = new System.Data.DataColumn();
			this.MyDS.BeginInit();
			this.DataTable1.BeginInit();
			this.MySelCmd.CommandText = null;
			this.MySelCmd.CommandTimeout = 0;
			this.MySelCmd.CommandType = System.Data.CommandType.Text;
			this.MySelCmd.Connection = this.MyCn;
			this.MySelCmd.Transaction = null;
			this.MySelCmd.UpdatedRowSource = System.Data.UpdateRowSource.Both;
			this.MyCn.ConnectionString = Literals.ConnectionString;
			this.MyDA.DeleteCommand = null;
			this.MyDA.InsertCommand = null;
			this.MyDA.SelectCommand = this.MySelCmd;
			this.MyDA.UpdateCommand = null;
			this.MyDS.DataSetName = "AMPDataSet";
			this.MyDS.Locale = new System.Globalization.CultureInfo("ru-RU");
			this.MyDS.Tables.AddRange(new System.Data.DataTable[] { this.DataTable1 });
			this.DataTable1.Columns.AddRange(new System.Data.DataColumn[] { this.DataColumn1, this.DataColumn2, this.DataColumn3, this.DataColumn4, this.DataColumn5, this.DataColumn6, this.DataColumn7, this.DataColumn8, this.DataColumn9, this.DataColumn10, this.DataColumn11, this.DataColumn12, this.DataColumn13, this.DataColumn14, this.DataColumn15, this.DataColumn16, this.DataColumn17, this.dataColumn18, this.dataColumn19 });
			this.DataTable1.TableName = "Prices";
			this.DataColumn1.ColumnName = "OrderID";
			this.DataColumn1.DataType = typeof(int);
			this.DataColumn2.ColumnName = "CreaterCode";
			this.DataColumn3.ColumnName = "ItemID";
			this.DataColumn4.Caption = "OriginalName";
			this.DataColumn4.ColumnName = "OriginalName";
			this.DataColumn5.ColumnName = "OriginalCr";
			this.DataColumn6.ColumnName = "Unit";
			this.DataColumn7.ColumnName = "Volume";
			this.DataColumn8.ColumnName = "Quantity";
			this.DataColumn9.ColumnName = "Note";
			this.DataColumn10.ColumnName = "Period";
			this.DataColumn11.ColumnName = "Doc";
			this.DataColumn12.ColumnName = "Junk";
			this.DataColumn12.DataType = typeof(bool);
			this.DataColumn13.ColumnName = "UpCost";
			this.DataColumn13.DataType = typeof(decimal);
			this.DataColumn14.ColumnName = "Cost";
			this.DataColumn14.DataType = typeof(decimal);
			this.DataColumn15.ColumnName = "SalerID";
			this.DataColumn15.DataType = typeof(uint);
			this.DataColumn16.ColumnName = "PriceDate";
			this.DataColumn17.ColumnName = "PrepCode";
			this.DataColumn17.DataType = typeof(uint);
			this.dataColumn18.ColumnName = "OrderCode1";
			this.dataColumn18.DataType = typeof(uint);
			this.dataColumn19.ColumnName = "OrderCode2";
			this.dataColumn19.DataType = typeof(uint);
			this.MyDS.EndInit();
			this.DataTable1.EndInit();
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

		[WebMethod()]
		public DataSet GetNameFromCatalog(string[] Name, string[] Form, bool NewEar, bool OfferOnly, uint[] PriceID, int Limit, int SelStart)
		{
			FunctionName = "GetNameFromCatalog";
			string[] Params = new string[2];
			int Inc;
			bool AMPCode = false;
			MyDS.Tables.Remove("Prices");
			Restart:
			try
			{
				if (MyCn.State == ConnectionState.Closed)
					MyCn.Open();

				MyTrans = MyCn.BeginTransaction();
				MySelCmd.Transaction = MyTrans;
				MySelCmd.CommandText = "SET SQL_BIG_SELECTS=1; ";
				MySelCmd.CommandText += "select distinct catalog.FullCode PrepCode, catalog.Name, catalog.Form from (intersection, clientsdata, pricesdata, pricesregionaldata, retclientsset, clientsdata as AClientsData, farm.catalog)" + " left join farm.formrules on formrules.firmcode=pricesdata.pricecode" + " left join farm.core0 c on c.firmcode=if(clientsdata.OldCode=0, pricesdata.pricecode, intersection.costcode) and catalog.fullcode=c.fullcode and to_days(now())-to_days(datecurprice)<maxold" + " left join farm.core0 ampc on ampc.fullcode=catalog.fullcode and ampc.codefirmcr=c.codefirmcr and ampc.firmcode=1864" + " where DisabledByClient=0" + " and Disabledbyfirm=0" + " and DisabledByAgency=0" + " and intersection.clientcode=" + GetClientCode().ToString() + " and retclientsset.clientcode=intersection.clientcode" + " and pricesdata.pricecode=intersection.pricecode and clientsdata.firmcode=pricesdata.firmcode";
				if (!(PriceID == null))
				{
					Inc = 0;
					MySelCmd.CommandText += " and (";
					for (int i = 0; i < PriceID.Length; i++)
					{
						string PriceNameStr = Convert.ToString(PriceID[i]);
						if (!String.IsNullOrEmpty(PriceNameStr))
						{
							if (Inc > 0)
							{
								MySelCmd.CommandText += " or ";
							}
							Params = FormatFindStr(PriceNameStr, "PriceCode" + Inc, "pricesdata.pricecode");
							MySelCmd.Parameters.Add("PriceCode" + Inc, Params[1]);
							MySelCmd.CommandText += Params[0];
							Inc += 1;
						}
					}
					if (Inc < 1)
					{
						MySelCmd.CommandText += "1";
					}
					MySelCmd.CommandText += ")";
				}
				if (!(Form == null))
				{
					Inc = 0;
					MySelCmd.CommandText += " and (";
					foreach (string PriceNameStr in Form)
					{
						if (!String.IsNullOrEmpty(PriceNameStr))
						{
							if (Inc > 0)
							{
								MySelCmd.CommandText += " or ";
							}
							Params = FormatFindStr(PriceNameStr, "Form" + Inc, "catalog.form");
							MySelCmd.Parameters.Add("Form" + Inc, Params[1]);
							MySelCmd.CommandText += Params[0];
							Inc += 1;
						}
						if (Inc < 1)
						{
							MySelCmd.CommandText += "1";
						}
					}
					MySelCmd.CommandText += ")";
				}
				MySelCmd.CommandText += " and clientsdata.firmstatus=1" + " and clientsdata.billingstatus=1" + " and clientsdata.firmtype=0";
				if (NewEar)
				{
					MySelCmd.CommandText += " and ampc.id is null";
				}
				if (OfferOnly | PriceID.Length > 0)
				{
					MySelCmd.CommandText += " and c.id is not null";
				}
				MySelCmd.CommandText += " and clientsdata.firmsegment=AClientsData.firmsegment" + " and pricesregionaldata.regioncode=intersection.regioncode" + " and pricesregionaldata.pricecode=pricesdata.pricecode" + " and AClientsData.firmcode=intersection.clientcode" + " and (clientsdata.maskregion & intersection.regioncode)>0" + " and (AClientsData.maskregion & intersection.regioncode)>0" + " and (retclientsset.workregionmask & intersection.regioncode)>0" + " and pricesdata.agencyenabled=1" + " and pricesdata.enabled=1 and invisibleonclient=0" + " and pricesdata.pricetype<>1" + " and pricesregionaldata.enabled=1";
				if (!(Name == null))
				{
					Inc = 0;
					MySelCmd.CommandText += " and (";
					
					if (Char.IsNumber(Name[0], 0))
					{
						AMPCode = true;
					}
					foreach (string NameStr in Name)
					{
						if (!String.IsNullOrEmpty(NameStr))
						{
							if (Inc > 0)
							{
								MySelCmd.CommandText += " or ";
							}
							if (AMPCode)
							{
								Params = FormatFindStr(NameStr, "Name" + Inc, "ampc.code");
							}
							else
							{
								Params = FormatFindStr(NameStr, "Name" + Inc, "catalog.Name");
							}
							MySelCmd.Parameters.Add("Name" + Inc, Params[1]);
							MySelCmd.CommandText += Params[0];
							Inc += 1;
						}
					}
					MySelCmd.CommandText += ")";
				}
				MySelCmd.CommandText += " order by catalog.Name, catalog.Form";
				MySelCmd.CommandText += GetLimitString(SelStart, Limit);

				LogQuery(MyDA.Fill(MyDS, "Catalog"), FunctionName, StartTime);
				MyTrans.Commit();
				return MyDS;
			}
			catch (MySqlException MySQLErr)
			{
				if (!(MyTrans == null))
				{
					MyTrans.Rollback();
				}
				if (MySQLErr.Number == 1213 | MySQLErr.Number == 1205)
				{
					System.Threading.Thread.Sleep(100);
					goto Restart;
				}
				AMPWebService.PostOrder.MailErr(FunctionName, MySQLErr.Message, MySQLErr.Source, UserName);
			}
			catch (Exception ex)
			{
				if (!(MyTrans == null))
				{
					MyTrans.Rollback();
				}
				AMPWebService.PostOrder.MailErr(FunctionName, ex.Message, ex.Source, UserName);
			}
			finally
			{
				if (MyCn.State == ConnectionState.Open)
				{
					MyCn.Close();
				}
			}
			return MyDS;
		}

		[WebMethod()]
		public DataSet GetPricesByPrepCode(Int32[] PrepCode, bool OnlyLeader, UInt32[] PriceID, Int32 Limit, Int32 SelStart)
		{
			FunctionName = "GetPricesByPrepCode";
			Int32 inc;
			string PriceNameStr;
			string[] Params = new string[2];
			string FullCodesString = "(";
			Restart :
			try
			{
				if (MyCn.State == ConnectionState.Closed)
					MyCn.Open();

				MyTrans = MyCn.BeginTransaction(IsolationLevel.ReadCommitted);
				MySelCmd.Transaction = MyTrans;
				foreach (int FullCode in PrepCode)
				{
					if ((FullCodesString.Length > 1) && (FullCode > 0))
						FullCodesString += ", ";
					if (FullCode > 0)
						FullCodesString += FullCode;
				}
				FullCodesString += ")";
				if (OnlyLeader)
				{
					MySelCmd.CommandText +=
@"
DROP temporary table IF EXISTS prices; 
DROP temporary table IF EXISTS mincosts; 
create temporary table prices(OrderID int(32) unsigned, SalerCode varchar(20) not null default 0, CreaterCode varchar(20) not null default 0, ItemID varchar(50) not null default 0, OriginalName varchar(255), OriginalCr varchar(255), Unit varchar(15) not null default 0, Volume varchar(15) not null default 0, Quantity varchar(15) not null default 0, Note varchar(50) not null default 0, Period varchar(20) not null default 0, Doc varchar(20) not null default 0, Junk Bit, UpCost decimal(5,3), Cost Decimal(8,2), SalerID int(32) unsigned, SalerName varchar(20), PriceDate varchar(20), FullCode int(32) unsigned, CodeFirmCr int(32) unsigned, SynonymCode int(32) unsigned, SynonymFirmCrCode int(32) unsigned, primary key ID(OrderID))type= heap; 
create temporary table mincosts( MinCost decimal(8,2), FullCode int(32) unsigned, Junk Bit) type = heap; 
INSERT 
INTO    prices 
";
				}
				MySelCmd.CommandText = String.Format(
@"
SELECT  c.id OrderID,
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
        c.Junk> 0 Junk, 
        intersection.PublicCostCorr As UpCost,
        round(if((1+pricesdata.UpCost/100)*(1+pricesregionaldata.UpCost/100) *(1+(intersection.PublicCostCorr+intersection.FirmCostCorr)/100) *c.BaseCost< c.minboundcost, c.minboundcost, (1+pricesdata.UpCost/100)*(1+pricesregionaldata.UpCost/100) *(1+(intersection.PublicCostCorr+intersection.FirmCostCorr)/100) *c.BaseCost), 2) Cost,
        pricesdata.pricecode SalerID,
        ClientsData.ShortName SalerName,
        if(fr.datelastform> fr.DateCurPrice, fr.DateCurPrice, fr.DatePrevPrice) PriceDate,
        c.fullcode PrepCode,
        c.codefirmcr ,
        c.synonymcode OrderCode1,
        c.synonymfirmcrcode OrderCode2
FROM    (intersection, clientsdata, pricesdata, pricesregionaldata, retclientsset, clientsdata as AClientsData, farm.core0 c, farm.synonym s, farm.formrules fr) 
LEFT JOIN farm.core0 ampc 
        ON ampc.fullcode   = c.fullcode 
        and ampc.codefirmcr= c.codefirmcr 
        and ampc.firmcode  = 1864 
LEFT JOIN farm.synonymfirmcr scr 
        ON scr.firmcode                                             = ifnull(parentsynonym, pricesdata.pricecode) 
        and c.synonymfirmcrcode                                     = scr.synonymfirmcrcode 
WHERE   DisabledByClient                                            = 0 
        and Disabledbyfirm                                          = 0 
        and DisabledByAgency                                        = 0 
        and intersection.clientcode                                 = {1} 
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
        and to_days(now())-to_days(datecurprice)                    < maxold 
        and pricesregionaldata.enabled                              = 1 
        and fr.firmcode                                             = pricesdata.pricecode 
        and c.firmcode                                              = intersection.costcode 
        and c.fullcode in {0} 
        and c.synonymcode = s.synonymcode 
        and s.firmcode    = ifnull(parentsynonym, pricesdata.pricecode) 
", FullCodesString, GetClientCode().ToString());
				if (!(PriceID == null))
				{
					inc = 0;
					MySelCmd.CommandText += " and (";
					for (int i = 0; i < PriceID.Length; i++)
					{
						PriceNameStr = Convert.ToString(PriceID[i]);
						if (inc > 0)
						{
							MySelCmd.CommandText += " or ";
						}
						Params = FormatFindStr(PriceNameStr, "ShortName" + inc, "pricesdata.pricecode");
						MySelCmd.Parameters.Add("ShortName" + inc, Params[1]);
						MySelCmd.CommandText += Params[0];
						inc += 1;
					}
					MySelCmd.CommandText += ")";
				}
				MySelCmd.CommandText += " GROUP BY 1 ";
				MySelCmd.CommandText += GetLimitString(SelStart, Limit); 
				if (OnlyLeader)
				{
					MySelCmd.CommandText += ";";
                    MySelCmd.CommandText +=
@"
INSERT 
INTO    mincosts 
SELECT  min(cost), 
        FullCode, 
        Junk 
FROM    (prices) 
GROUP BY FullCode, 
        Junk; 
SELECT  OrderID, 
        SalerCode, 
        CreaterCode, 
        ItemID, 
        OriginalName, 
        OriginalCr, 
        Unit, 
        Volume, 
        Quantity, 
        Note, 
        Period, 
        Doc, 
        p.Junk, 
        UpCost, 
        Cost, 
        SalerID, 
        SalerName, 
        PriceDate, 
        p.FullCode PrepCode, 
        synonymcode OrderCode1, 
        synonymfirmcrcode OrderCode2 
FROM    (prices p, mincosts m) 
WHERE   p.fullcode= m.fullcode 
        and p.cost= m.mincost 
        and p.junk= m.junk
";
				}

				MySelCmd.CommandText += GetLimitString(SelStart, Limit);
				if (OnlyLeader)
				{
					MySelCmd.CommandText +=
@"
DROP temporary table IF EXISTS prices; 
DROP temporary table IF EXISTS mincosts; 
";
				}

				LogQuery(MyDA.Fill(MyDS, "prices"), FunctionName, StartTime);
				MyTrans.Commit();
				return MyDS;
			}

			catch (MySqlException MySQLErr)
			{
				if (MyTrans != null)
				{
					MyTrans.Rollback();
				}
				if ((MySQLErr.Number == 1213) || (MySQLErr.Number == 1205))
				{
					System.Threading.Thread.Sleep(100);
					goto Restart;
				}
				AMPWebService.PostOrder.MailErr(FunctionName, MySQLErr.Message, MySQLErr.Source, UserName);
			}
			catch (Exception ex)
			{
				if (!(MyTrans == null))
				{
					MyTrans.Rollback();
				}
				AMPWebService.PostOrder.MailErr(FunctionName, ex.Message, ex.Source, UserName);
			}
			finally
			{
				if (MyCn.State == ConnectionState.Open)
				{
					MyCn.Close();
				}
			}
			return MyDS;
		}

		[WebMethod()]
		public DataSet GetPricesByItemID(string[] ItemID, bool OnlyLeader, string[] SalerName, Int32 Limit, Int32 SelStart)
		{
			string NameStr = String.Empty;
			string AMPCodes;
			string[] Params;
			List<int> AMPCodesArr = new List<int>();
			List<int> NotAMPCodesArr = new List<int>();
			FunctionName = "GetPricesByItemID";
			Restart:
			try
			{
			
				if (MyCn.State == ConnectionState.Closed)
					MyCn.Open();

				MyTrans = MyCn.BeginTransaction();
				MySelCmd.Transaction = MyTrans;
				if (ItemID != null)
				{
					foreach (string item in ItemID)
					{
						bool NotID;
						AMPCodes = item;
						if (AMPCodes.Length > 0)
						{
							if (AMPCodes.StartsWith("!"))
							{
								AMPCodes = AMPCodes.Remove(0, 1);
								NotID = true;
							}
							else
							{
								NotID = false;
							}
							if (AMPCodes.IndexOf("..") > 0)
							{
								int SepIndex = AMPCodes.IndexOf("..");
								int beginIndex = Convert.ToInt32(AMPCodes.Substring(0, SepIndex));
								int endIndex = Convert.ToInt32(AMPCodes.Substring(SepIndex + 2, AMPCodes.Length - SepIndex - 2));
								if (beginIndex < endIndex)
								{
									if ((beginIndex - endIndex) > 20000)
										new Exception("Owerflow");
									for (int Inc = beginIndex; Inc <= endIndex; Inc++)
									{
										if (NotID)
											NotAMPCodesArr.Add(Inc);
										else
											AMPCodesArr.Add(Inc);
									}
								}
							}
							else
							{
								if (NotID)
									NotAMPCodesArr.Add(Convert.ToInt32(AMPCodes));
								else
									AMPCodesArr.Add(Convert.ToInt32(AMPCodes));
							}
						}
					}
				}
				if (OnlyLeader)
				{
					MySelCmd.CommandText =
@"
DROP temporary table IF EXISTS prices; 
DROP temporary table IF EXISTS mincosts; 
create temporary table prices(OrderID int(32) unsigned, SalerCode varchar(20) not null default 0, CreaterCode varchar(20) not null default 0, ItemID varchar(50) not null default 0, OriginalName varchar(255), OriginalCr varchar(255), Unit varchar(15) not null default 0, Volume varchar(15) not null default 0, Quantity varchar(15) not null default 0, Note varchar(50) not null default 0, Period varchar(20) not null default 0, Doc varchar(20) not null default 0, Junk Bit, UpCost decimal(5,3), Cost Decimal(8,2), SalerID int(32) unsigned, SalerName varchar(20), PriceDate varchar(20), FullCode int(32) unsigned, CodeFirmCr int(32) unsigned, SynonymCode int(32) unsigned, SynonymFirmCrCode int(32) unsigned, primary key ID(OrderID))type = heap; 
create temporary table mincosts( MinCost decimal(8,2), FullCode int(32) unsigned, Junk Bit) type = heap; 
INSERT 
INTO    prices 
";
				}
				MySelCmd.CommandText += String.Format(
@"
SELECT  c.id OrderID,
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
        c.Junk> 0 Junk, 
        intersection.PublicCostCorr As UpCost,
        round(if((1+pricesdata.UpCost/100)*(1+pricesregionaldata.UpCost/100) *(1+(intersection.PublicCostCorr+intersection.FirmCostCorr)/100) *c.BaseCost< c.minboundcost, c.minboundcost, (1+pricesdata.UpCost/100)*(1+pricesregionaldata.UpCost/100) *(1+(intersection.PublicCostCorr+intersection.FirmCostCorr)/100) *c.BaseCost), 2) Cost,
        pricesdata.pricecode SalerID,
        ClientsData.ShortName SalerName,
        if(fr.datelastform> fr.DateCurPrice, fr.DateCurPrice, fr.DatePrevPrice) PriceDate,
        c.fullcode PrepCode,
        c.codefirmcr ,
        c.synonymcode OrderCode1,
        c.synonymfirmcrcode OrderCode2
FROM    (intersection, clientsdata, pricesdata, pricesregionaldata, retclientsset, clientsdata as AClientsData, farm.core0 c, farm.synonym s, farm.formrules fr)
LEFT JOIN farm.core0 ampc
        ON ampc.fullcode   = c.fullcode
        and ampc.codefirmcr= c.codefirmcr
        and ampc.firmcode  = 1864
LEFT JOIN farm.synonymfirmcr scr
        ON scr.firmcode                                             = ifnull(parentsynonym, pricesdata.pricecode)
        and c.synonymfirmcrcode                                     = scr.synonymfirmcrcode 
WHERE   DisabledByClient                                            = 0 
        and Disabledbyfirm                                          = 0 
        and DisabledByAgency                                        = 0 
        and intersection.clientcode                                 = {0}  
        and retclientsset.clientcode                                = intersection.clientcode 
        and pricesdata.pricecode                                    = intersection.pricecode 
        and clientsdata.firmcode                                    = pricesdata.firmcode 
        and clientsdata.firmstatus                                  = 1 
        and clientsdata.billingstatus                               = 1 
        and clientsdata.firmtype                                    = 0 
        and to_days(now())-to_days(datecurprice)                    < maxold 
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
        and fr.firmcode                                             = pricesdata.pricecode 
        and c.firmcode                                              = if(clientsdata.OldCode= 0, pricesdata.pricecode, intersection.costcode) 
        and c.synonymcode                                           = s.synonymcode 
        and s.firmcode                                              = ifnull(parentsynonym, pricesdata.pricecode)
", GetClientCode().ToString()); 

				if (!(SalerName == null))
				{
					int Inc = 0;
					MySelCmd.CommandText += " and (";
					foreach (string PriceNameStr in SalerName)
					{
						if (Inc > 0)
						{
							MySelCmd.CommandText += " or ";
						}
						Params = FormatFindStr(PriceNameStr, "ShortName" + Inc, "pricesdata.pricename");
						MySelCmd.Parameters.Add("ShortName" + Inc, Params[1]);
						MySelCmd.CommandText += Params[0];
						Inc += 1;
					}
					MySelCmd.CommandText += ")";
				}
				if (NotAMPCodesArr.Count > 0)
				{
					MySelCmd.CommandText += " and ampc.code not in (";
					for (int Inc = 0; Inc <= NotAMPCodesArr.Count - 1; Inc++)
					{
						MySelCmd.CommandText += "'" + NotAMPCodesArr[Inc] + "'";
						if (Inc < NotAMPCodesArr.Count - 1)
						{
							MySelCmd.CommandText += ",";
						}
					}
					MySelCmd.CommandText += ")";
				}
				if (AMPCodesArr.Count > 0)
				{
					MySelCmd.CommandText += " and ampc.code in (";
					for (int Inc = 0; Inc <= AMPCodesArr.Count - 1; Inc++)
					{
						MySelCmd.CommandText += AMPCodesArr[Inc];
						if (Inc < AMPCodesArr.Count - 1)
						{
							MySelCmd.CommandText += ",";
						}
					}
					MySelCmd.CommandText += ")";
				}

				MySelCmd.CommandText += "group by 1";
				if (OnlyLeader)
				{
					MySelCmd.CommandText += ";";
					MySelCmd.CommandText += 
@"
INSERT 
INTO    mincosts 
SELECT  min(cost), 
        FullCode, 
        Junk 
FROM    (prices) 
GROUP BY FullCode, 
        Junk; 
SELECT  OrderID, 
        SalerCode, 
        CreaterCode, 
        ItemID, 
        OriginalName, 
        OriginalCr, 
        Unit, 
        Volume, 
        Quantity, 
        Note, 
        Period, 
        Doc, 
        p.Junk, 
        UpCost, 
        Cost, 
        SalerID, 
        SalerName, 
        PriceDate, 
        p.FullCode PrepCode, 
        synonymcode OrderCode1, 
        synonymfirmcrcode OrderCode2 
FROM    (prices p, mincosts m) 
WHERE   p.fullcode= m.fullcode 
        and p.cost= m.mincost 
        and p.junk= m.junk
";
				}
				MySelCmd.CommandText += GetLimitString(SelStart, Limit);
				if (OnlyLeader)
				{
					MySelCmd.CommandText +=
@"
DROP temporary table IF EXISTS prices; 
DROP temporary table IF EXISTS mincosts; 
";
				}
				LogQuery(MyDA.Fill(MyDS, "Prices"), FunctionName, StartTime);
				MyTrans.Commit();
				return MyDS;
			}
			catch (MySqlException MySQLErr)
			{
				if (!(MyTrans == null))
				{
					MyTrans.Rollback();
				}
				if (MySQLErr.Number == 1213 | MySQLErr.Number == 1205)
				{
					System.Threading.Thread.Sleep(100);
					goto Restart;
				}
				AMPWebService.PostOrder.MailErr(FunctionName, MySQLErr.Message, MySQLErr.Source, UserName);
			}
			catch (Exception ex)
			{
				if (!(MyTrans == null))
				{
					MyTrans.Rollback();
				}
				AMPWebService.PostOrder.MailErr(FunctionName, ex.Message, ex.Source, UserName);
			}
			finally
			{
				if (MyCn.State == ConnectionState.Open)
				{
					MyCn.Close();
				}
			}
			return MyDS;
		}

		[WebMethod()]
		public DataSet GetPricesByName(string[] OriginalName, string[] SalerName, string[] PriceName, bool OnlyLeader, bool NewEar, Int32 Limit, Int32 SelStart)
		{
			string[] Params = new string[2];
			int Inc;
			FunctionName = "GetPricesByName";
			Restart:
			try
			{
				if (MyCn.State == ConnectionState.Closed)
					MyCn.Open();

				MyTrans = MyCn.BeginTransaction();
				MySelCmd.Transaction = MyTrans;
				if (OnlyLeader)
				{
					MySelCmd.CommandText =
@"
DROP temporary table IF EXISTS prices; 
DROP temporary table IF EXISTS mincosts; 
create temporary table prices(OrderID int(32) unsigned, SalerCode varchar(20) not null default 0, CreaterCode varchar(20) not null default 0, ItemID varchar(50) not null default 0, OriginalName varchar(255), OriginalCr varchar(255), Unit varchar(15) not null default 0, Volume varchar(15) not null default 0, Quantity varchar(15) not null default 0, Note varchar(50) not null default 0, Period varchar(20) not null default 0, Doc varchar(20) not null default 0, Junk Bit, UpCost decimal(5,3), Cost Decimal(8,2), SalerID int(32) unsigned, SalerName varchar(20), PriceDate varchar(20), FullCode int(32) unsigned, CodeFirmCr int(32) unsigned, SynonymCode int(32) unsigned, SynonymFirmCrCode int(32) unsigned, primary key ID(OrderID))type = heap; 
create temporary table mincosts( MinCost decimal(8,2), FullCode int(32) unsigned, Junk Bit) type = heap; 
INSERT 
INTO    prices 
";						
				}
				MySelCmd.CommandText += String.Format(
@"
SELECT  c.id OrderID,
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
        c.Junk> 0 Junk, 
        intersection.PublicCostCorr As UpCost,
        round(if((1+pricesdata.UpCost/100)*(1+pricesregionaldata.UpCost/100) *(1+(intersection.PublicCostCorr+intersection.FirmCostCorr)/100) *c.BaseCost< c.minboundcost, c.minboundcost, (1+pricesdata.UpCost/100)*(1+pricesregionaldata.UpCost/100) *(1+(intersection.PublicCostCorr+intersection.FirmCostCorr)/100) *c.BaseCost), 2) Cost,
        pricesdata.pricecode SalerID,
        ClientsData.ShortName SalerName,
        if(fr.datelastform> fr.DateCurPrice, fr.DateCurPrice, fr.DatePrevPrice) PriceDate,
        c.fullcode PrepCode,
        c.codefirmcr ,
        c.synonymcode OrderCode1,
        c.synonymfirmcrcode OrderCode2
FROM    (intersection, clientsdata, pricesdata, pricesregionaldata, retclientsset, clientsdata as AClientsData, farm.core0 c, farm.synonym s, farm.formrules fr)
LEFT JOIN farm.core0 ampc
        ON ampc.fullcode   = c.fullcode
        and ampc.codefirmcr= c.codefirmcr
        and ampc.firmcode  = 1864
LEFT JOIN farm.synonymfirmcr scr
        ON scr.firmcode                                             = ifnull(parentsynonym, pricesdata.pricecode)
        and c.synonymfirmcrcode                                     = scr.synonymfirmcrcode 
WHERE   DisabledByClient                                            = 0 
        and Disabledbyfirm                                          = 0 
        and DisabledByAgency                                        = 0 
        and intersection.clientcode                                 = {0}  
        and retclientsset.clientcode                                = intersection.clientcode 
        and pricesdata.pricecode                                    = intersection.pricecode 
        and clientsdata.firmcode                                    = pricesdata.firmcode 
        and clientsdata.firmstatus                                  = 1 
        and clientsdata.billingstatus                               = 1 
        and clientsdata.firmtype                                    = 0 
        and to_days(now())-to_days(datecurprice)                    < maxold 
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
        and fr.firmcode                                             = pricesdata.pricecode 
        and c.firmcode                                              = if(clientsdata.OldCode= 0, pricesdata.pricecode, intersection.costcode) 
        and c.synonymcode                                           = s.synonymcode 
        and s.firmcode                                              = ifnull(parentsynonym, pricesdata.pricecode)
", GetClientCode().ToString());

				if (NewEar)
					MySelCmd.CommandText += " and ampc.id is null";

				if (SalerName != null)
				{
					Inc = 0;
					MySelCmd.CommandText += " and (";
					foreach (string PriceNameStr in SalerName)
					{
						if (Inc > 0)
							MySelCmd.CommandText += " or ";
						Params = FormatFindStr(PriceNameStr, "ShortName" + Inc, "clientsdata.shortname");
						MySelCmd.Parameters.Add("ShortName" + Inc, Params[1]);
						MySelCmd.CommandText += Params[0];
						Inc += 1;
					}
					MySelCmd.CommandText += ")";
				}
				if (PriceName != null)
				{
					Inc = 0;
					MySelCmd.CommandText += " and (";
					foreach (string PriceNameStr in SalerName)
					{
						if (Inc > 0)
						{
							MySelCmd.CommandText += " or ";
						}
						Params = FormatFindStr(PriceNameStr, "PriceName" + Inc, "pricesdata.pricename");
						MySelCmd.Parameters.Add("PriceName" + Inc, Params[1]);
						MySelCmd.CommandText += Params[0];
						Inc += 1;
					}
					MySelCmd.CommandText += ")";
				}
				if (OriginalName != null)
				{
					Inc = 0;
					MySelCmd.CommandText += " and (";
					foreach (string NameStr in OriginalName)
					{
						if (NameStr.Length > 0)
						{
							if (Inc > 0)
							{
								MySelCmd.CommandText += " or ";
							}
							Params = FormatFindStr(NameStr, "Name" + Inc, "s.synonym");
							MySelCmd.Parameters.Add("Name" + Inc, Params[1]);
							MySelCmd.CommandText += Params[0];
							Inc += 1;
						}
					}
					MySelCmd.CommandText += ")";
				}
				MySelCmd.CommandText += " group by 1";
				if (OnlyLeader)
				{
					MySelCmd.CommandText += ";";
					MySelCmd.CommandText +=
@"
INSERT 
INTO    mincosts 
SELECT  min(cost), 
        FullCode, 
        Junk 
FROM    (prices) 
GROUP BY FullCode, 
        Junk; 
SELECT  OrderID, 
        SalerCode, 
        CreaterCode, 
        ItemID, 
        OriginalName, 
        OriginalCr, 
        Unit, 
        Volume, 
        Quantity, 
        Note, 
        Period, 
        Doc, 
        p.Junk, 
        UpCost, 
        Cost, 
        SalerID, 
        SalerName, 
        PriceDate, 
        p.FullCode PrepCode, 
        synonymcode OrderCode1, 
        synonymfirmcrcode OrderCode2 
FROM    (prices p, mincosts m) 
WHERE   p.fullcode= m.fullcode 
        and p.cost= m.mincost 
        and p.junk= m.junk
";
				}
				MySelCmd.CommandText += GetLimitString(SelStart, Limit);
				if (OnlyLeader)
				{
					MySelCmd.CommandText +=
@"
DROP temporary table IF EXISTS prices; 
DROP temporary table IF EXISTS mincosts; 
";
				}

				LogQuery(MyDA.Fill(MyDS, "Prices"), FunctionName, StartTime);
				MyTrans.Commit();
				return MyDS;
			}
			catch (MySqlException MySQLErr)
			{
				if (!(MyTrans == null))
				{
					MyTrans.Rollback();
				}
				if (MySQLErr.Number == 1213 | MySQLErr.Number == 1205)
				{
					System.Threading.Thread.Sleep(100);
					goto Restart;
				}
				AMPWebService.PostOrder.MailErr(FunctionName, MySQLErr.Message, MySQLErr.Source, UserName);
			}
			catch (Exception ex)
			{
				if (!(MyTrans == null))
				{
					MyTrans.Rollback();
				}
				AMPWebService.PostOrder.MailErr(FunctionName, ex.Message, ex.Source, UserName);
			}
			finally
			{
				if (MyCn.State == ConnectionState.Open)
				{
					MyCn.Close();
				}
			}
			return MyDS;
		}

		[WebMethod()]
		public DataSet PostOrder(Int32[] OrderID, Int32[] Quantity, string[] Message, Int32[] OrderCode1, Int32[] OrderCode2, bool[] Junk)
		{
			FunctionName = "PostOrder";
			try
			{
				return AMPWebService.PostOrder.PostOrderMethod(OrderID, Quantity, Message, OrderCode1, OrderCode2, Junk, GetClientCode(), UserName);
			}
			catch (Exception err)
			{
				AMPWebService.PostOrder.MailErr(FunctionName, err.Message, err.Source, UserName);
			}
			finally
			{
				if (MyCn.State == ConnectionState.Open)
				{
					MyCn.Close();
				}
			}
			return null;
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
				{
					TmpRes += " not like ";
				}
				else
				{
					TmpRes += " <> ";
				}
			}
			else
			{
				if (UseLike)
				{
					TmpRes += " like ";
				}
				else
				{
					TmpRes += " = ";
				}
			}
			InpStr = InpStr.Replace("*", "%");
			TmpRes += "?" + ParameterName;
			Result[0] = TmpRes;
			Result[1] = InpStr;
			return Result;
		}

		private UInt32 GetClientCode()
		{
#if DEBUG
			UserName = "michail";
#else
			UserName = System.Web.HttpContext.Current.User.Identity.Name;
			if (UserName.Substring(0, 7) == "ANALIT\\")
			{
				UserName = UserName.Substring(7);
			}
			UserName = "amp";
#endif
			try
			{
				MySelCmd.CommandText = " SELECT osuseraccessright.clientcode" + " FROM (clientsdata, osuseraccessright)" + " where osuseraccessright.clientcode=clientsdata.firmcode" + " and firmstatus=1" + " and billingstatus=1" + " and allowGetData=1" + " and OSUserName='" + UserName + "'";
				return Convert.ToUInt32(MySelCmd.ExecuteScalar());
			}
			catch (Exception ErrorTXT)
			{
				AMPWebService.PostOrder.MailErr("GetClientCode", ErrorTXT.Message, ErrorTXT.Source, UserName);
			}
			finally
			{
			}

			return 0;
		}

		private void LogQuery(Int32 RowCount, string FunctionName, System.DateTime StartTime)
		{
			//MySelCmd.CommandText = " insert into logs.AMPLogs(LogTime, Host, User, Function, RowCount, ProcessingTime) " + " values(now(), '" + System.Web.HttpContext.Current.Request.UserHostAddress + "', '" + UserName + "', '" + FunctionName + "', " + RowCount + ", " + System.Convert.ToInt32(DateTime.Now.Subtract(StartTime).TotalMilliseconds).ToString() + ")";
			MySelCmd.ExecuteNonQuery();
		}
		/// <summary>
		/// Формирует блок LIMIT для SQL запроса. Пример: "LIMIT 1 20"
		/// </summary>
		/// <param name="offset">Начиная с какого элемента</param>
		/// <param name="count">Количество элементов</param>
		/// <returns></returns>
		private string GetLimitString(int offset, int count)
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