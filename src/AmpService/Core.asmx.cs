using System;
using System.Web.Services;
using MySql.Data.MySqlClient;
using System.Data;
using System.Net.Mail;
using System.Collections.Generic;
using ExecuteTemplate;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Collections;

namespace AMPWebService
{
	internal class FirmNameArgs : ExecuteArgs
	{
		
		private string[] _firmNames;

		public string[] FirmNames
		{
			get { return _firmNames; }
			set { _firmNames = value; }
		}

		public FirmNameArgs(string[] firmNames)
			: base()
		{
			_firmNames = firmNames;
		}
	}

	internal class GetPricesArgs : ExecuteArgs
	{
		private int			_count;
		private int			_offset;
		private bool		_onlyLeader;
		private bool		_newEar;
		private string[]	_rangeField;
		private string[]	_rangeValue;
		private string[]	_sortField;
		private string[]	_sortDirection;

		public int Count
		{
			get { return _count; }
			set { _count = value; }
		}

		public int Offset
		{
			get { return _offset; }
			set { _offset = value; }
		}

		public bool OnlyLeader
		{
			get { return _onlyLeader; }
			set { _onlyLeader = value; }
		}

		public bool NewEar
		{
			get { return _newEar; }
			set { _newEar = value; }
		}

		public string[] RangeField
		{
			get { return _rangeField; }
			set { _rangeField = value; }
		}

		public string[] RangeValue
		{
			get { return _rangeValue; }
			set { _rangeValue = value; }
		}

		public string[] SortField
		{
			get { return _sortField; }
			set { _sortField = value; }
		}
		
		public string[] SortDirection
		{
			get { return _sortDirection; }
			set { _sortDirection = value; }
		}

		public GetPricesArgs(bool onlyLeader, bool newEar, string[] rangeField, string[] rangeValue, string[] sortField, string[] sortDirection, int count, int offset)
			: base()
		{
			_count = count;
			_offset = offset;
			_onlyLeader = onlyLeader;
			_newEar = newEar;
			_rangeField = rangeField;
			_rangeValue = rangeValue;
			_sortField = sortField;
			_sortDirection = sortDirection;
		}
	}

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

				Logger.Write(MyDA.SelectCommand.CommandText);
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

		[WebMethod]
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
					MySelCmd.CommandText =
@"
DROP temporary table IF EXISTS prices; 
DROP temporary table IF EXISTS mincosts; 
create temporary table prices(OrderID int(32) unsigned, SalerCode varchar(20) not null default 0, CreaterCode varchar(20) not null default 0, ItemID varchar(50) not null default 0, OriginalName varchar(255), OriginalCr varchar(255), Unit varchar(15) not null default 0, Volume varchar(15) not null default 0, Quantity varchar(15) not null default 0, Note varchar(50) not null default 0, Period varchar(20) not null default 0, Doc varchar(20) not null default 0, Junk Bit, UpCost decimal(5,3), Cost Decimal(8,2), SalerID int(32) unsigned, SalerName varchar(20), PriceDate varchar(20), FullCode int(32) unsigned, SynonymCode int(32) unsigned, SynonymFirmCrCode int(32) unsigned, primary key ID(OrderID))type= heap; 
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
				Logger.Write(MyDA.SelectCommand.CommandText);
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
		public DataSet GetPricesByItemID(string[] ItemID, bool OnlyLeader, uint[] PriceID , Int32 Limit, Int32 SelStart)
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
create temporary table prices(OrderID int(32) unsigned, SalerCode varchar(20) not null default 0, CreaterCode varchar(20) not null default 0, ItemID varchar(50) not null default 0, OriginalName varchar(255), OriginalCr varchar(255), Unit varchar(15) not null default 0, Volume varchar(15) not null default 0, Quantity varchar(15) not null default 0, Note varchar(50) not null default 0, Period varchar(20) not null default 0, Doc varchar(20) not null default 0, Junk Bit, UpCost decimal(5,3), Cost Decimal(8,2), SalerID int(32) unsigned, SalerName varchar(20), PriceDate varchar(20), FullCode int(32) unsigned, SynonymCode int(32) unsigned, SynonymFirmCrCode int(32) unsigned, primary key ID(OrderID))type = heap; 
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
        and c.firmcode                                              = intersection.costcode
        and c.synonymcode                                           = s.synonymcode 
        and s.firmcode                                              = ifnull(parentsynonym, pricesdata.pricecode)
", GetClientCode().ToString());

				MySelCmd.CommandText += FormatPriceIDForQuery(PriceID);


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

				MySelCmd.CommandText += " group by 1 ";
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
				Logger.Write(MyDA.SelectCommand.CommandText);
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
		public DataSet GetPricesByOriginalName(string[] OriginalName, uint[] PriceId, bool OnlyLeader, bool NewEar, Int32 Limit, Int32 SelStart)
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
create temporary table prices(OrderID int(32) unsigned, SalerCode varchar(20) not null default 0, CreaterCode varchar(20) not null default 0, ItemID varchar(50) not null default 0, OriginalName varchar(255), OriginalCr varchar(255), Unit varchar(15) not null default 0, Volume varchar(15) not null default 0, Quantity varchar(15) not null default 0, Note varchar(50) not null default 0, Period varchar(20) not null default 0, Doc varchar(20) not null default 0, Junk Bit, UpCost decimal(5,3), Cost Decimal(8,2), SalerID int(32) unsigned, SalerName varchar(20), PriceDate varchar(20), FullCode int(32) unsigned, SynonymCode int(32) unsigned, SynonymFirmCrCode int(32) unsigned, primary key ID(OrderID))type = heap; 
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
        and c.firmcode                                              = intersection.costcode
        and c.synonymcode                                           = s.synonymcode 
        and s.firmcode                                              = ifnull(parentsynonym, pricesdata.pricecode)
", GetClientCode().ToString());

				if (NewEar)
					MySelCmd.CommandText += " and ampc.id is null";

				MySelCmd.CommandText += FormatPriceIDForQuery(PriceId);

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

				Logger.Write(MyDA.SelectCommand.CommandText);
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

		[WebMethod]
		public DataSet PostOrder(Int32[] OrderID, Int32[] Quantity, string[] Message, Int32[] OrderCode1, Int32[] OrderCode2, bool[] Junk)
		{
			return AMPWebService.PostOrder.PostOrderMethod(OrderID, Quantity, Message, OrderCode1, OrderCode2, Junk, GetClientCode(), UserName);
		}

		private DataSet InnerGetPriceCodeByName(ExecuteArgs e)
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


			e.DataAdapter.SelectCommand.CommandText += StringArrayToQuery(((FirmNameArgs)e).FirmNames, "ClientsData.ShortName");

			e.DataAdapter.SelectCommand.Parameters.Add("?ClientCode", e.ClientCode);

			DataSet data = new DataSet();
			e.DataAdapter.Fill(data, "PriceList");

			return data;
		}

		[WebMethod]
		public DataSet GetPriceCodeByName(string[] firmName)
		{
			return MethodTemplate.ExecuteMethod(new FirmNameArgs(firmName), new ExecuteMethodDelegate(this.InnerGetPriceCodeByName), MyCn);
		}

		private DataSet InnerGetPrices(ExecuteArgs e)
		{
			//словарь для валидации и трансляции имен полей для клиента в имена полей для использования в запросе
			Dictionary<string, string> validRequestFields = new Dictionary<string, string>();
			validRequestFields.Add("prepcode", "c.fullcode");
			validRequestFields.Add("pricecode", "pricesdata.pricecode");
			validRequestFields.Add("itemid", "ampc.code");
			validRequestFields.Add("originalname", "s.synonym");	

			List<string> validSortFields = new List<string>();
			validSortFields.Add("orderid");
			validSortFields.Add("salercode");
			validSortFields.Add("creatercode");
			validSortFields.Add("itemid");
			validSortFields.Add("originalname");
			validSortFields.Add("originalcr");
			validSortFields.Add("unit");
			validSortFields.Add("volume");
			validSortFields.Add("quantity");
			validSortFields.Add("note");
			validSortFields.Add("period");
			validSortFields.Add("doc");
			validSortFields.Add("junk");
			validSortFields.Add("upcost");
			validSortFields.Add("cost");
			validSortFields.Add("pricecode");
			validSortFields.Add("salername");
			validSortFields.Add("pricedate");
			validSortFields.Add("prepcode");
			validSortFields.Add("ordercode1");
			validSortFields.Add("ordercode2");

			GetPricesArgs args = e as GetPricesArgs;
			
			//проверка входящих параметров
			if ((args.RangeValue ==null) || (args.RangeField == null) 
				|| (args.RangeField.Length != args.RangeValue.Length))
				throw new Exception();
			//TODO: в принципе в этой проверке нет нужды если будет неверное название поля 
			//то будет Exception на этапе трансляции
			//проверка имен полей для фильтрации
			foreach (string fieldName in args.RangeField)
				if (!validRequestFields.ContainsKey(fieldName.ToLower()))
					throw new Exception();
			//проверка имен полей для сортировки
			if (args.SortField != null)
			{
				foreach (string fieldName in args.SortField)
					if (!validSortFields.Contains(fieldName.ToLower()))
						throw new Exception();
			}
			//проверка направлений сортировки
			if (args.SortDirection != null)
			{
				foreach (string direction in args.SortDirection)
					if (!((String.Compare(direction, "Asc", true) == 0) || (String.Compare(direction, "Desc", true) == 0)))
						throw new Exception();
			}

			//словарь хранящий имена фильтруемых полей и значения фильтрации
			//           |            |
			//		  имя поля	список значений для него
			Dictionary<string, List<string>> FiltedField = new Dictionary<string, List<string>>();
			//разбирается входящие параметры args.RangeField и args.RangeValue и одновременно клиентские имена
			//транслируются во внутренние
			for (int i = 0; i < args.RangeField.Length; i++)
			{
				//преобразовываем клиентские названия полей во внутренние
				string innerFieldName = validRequestFields[args.RangeField[i].ToLower()];
				//если в словаре не такого поля 
				if (!FiltedField.ContainsKey(innerFieldName))
					//то добавляем его и создаем массив для хранения его значений
					FiltedField.Add(innerFieldName, new List<string>());
				//добавляем значение для фильтрации
				FiltedField[innerFieldName].Add(args.RangeValue[i]);
			}


			e.DataAdapter.SelectCommand.CommandText +=
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
        pricesdata.pricecode PriceCode,
        ClientsData.ShortName SalerName,
        if(fr.datelastform> fr.DateCurPrice, fr.DateCurPrice, fr.DatePrevPrice) PriceDate,
        c.fullcode PrepCode,
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
        and intersection.clientcode                                 = ?ClientCode  
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
        and c.firmcode                                              = intersection.costcode 
        and c.synonymcode                                           = s.synonymcode 
        and s.firmcode                                              = ifnull(parentsynonym, pricesdata.pricecode)
";
			if (args.NewEar)
				e.DataAdapter.SelectCommand.CommandText += " and ampc.id is null ";
			foreach (string fieldName in FiltedField.Keys)
				e.DataAdapter.SelectCommand.CommandText += StringArrayToQuery(FiltedField[fieldName], fieldName);

			if (!args.OnlyLeader)
			{
				e.DataAdapter.SelectCommand.CommandText += FormatOrderBlock(args.SortField, args.SortDirection);
				e.DataAdapter.SelectCommand.CommandText += GetLimitString(args.Offset, args.Count);
			}
			e.DataAdapter.SelectCommand.Parameters.Add("?ClientCode", e.ClientCode);

			DataSet data = new DataSet();
			e.DataAdapter.Fill(data, "PriceList");

			if (args.OnlyLeader)
			{
				DataTable resultTable = data.Tables[0].Clone();
				DataTable prepCodeTable = data.Tables[0].DefaultView.ToTable(true, "PrepCode");
				foreach (DataRow row in prepCodeTable.Rows)
				{
					DataRow[] orderedRows = data.Tables[0].Select(String.Format("PrepCode = {0}", row["PrepCode"]), "Cost ASC");
					resultTable.ImportRow(orderedRows[0]);
				}

				if (args.Offset > -1)
				{
					DataTable tempTable = resultTable;
					resultTable = data.Tables[0].Clone();

					int count;
					if ((args.Count > tempTable.Rows.Count) || (args.Count < args.Offset))
						count = tempTable.Rows.Count;
					else
						count = args.Count;

					for (int i = args.Offset; i < count; i++)
						resultTable.ImportRow(tempTable.Rows[i]);
				}

				if ((args.SortField != null) && (args.SortField.Length > 0))
				{
					resultTable.DefaultView.Sort = FormatOrderBlock(args.SortField, args.SortDirection).Substring(10);
					resultTable = resultTable.DefaultView.ToTable();
				}
				data.Tables.Remove(data.Tables[0]);
				data.Tables.Add(resultTable);
			}

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
		[WebMethod]
		public DataSet GetPrices(bool OnlyLeader, bool NewEar, string[] RangeField, string[] RangeValue, string[] SortField, string[] SortOrder, int Limit, int SelStart)
		{
			return MethodTemplate.ExecuteMethod(new GetPricesArgs(OnlyLeader, NewEar, RangeField, RangeValue, SortField, SortOrder, Limit, SelStart), new ExecuteMethodDelegate(this.InnerGetPrices), MyCn);
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
		
		/// <summary>
		/// Форматирует массив идентификаторов прайс листа для применения в запросе.
		/// Пример: массив {1, 2} приводится к виду " and PricesData.PriceCode in (1, 2) "
		/// </summary>
		/// <param name="priceID">массив идентификаторов</param>
		/// <returns>отформатированная строка</returns>
		private string FormatPriceIDForQuery(params uint[] priceID)
		{ 
			string result = String.Empty;
			if ((priceID != null) && (priceID.Length > 0))
			{ 
				string ids = String.Empty; 
				foreach (uint id in priceID)
					ids += Convert.ToString(id) + " , ";

				ids = ids.Remove(ids.Length - 3);
				result = String.Format(" and PricesData.PriceCode in ({0}) ", ids);
			}
			return result;
		}

		/// <summary>
		/// Преобразовывает массив строк для использования в запросе.
		/// Также происходит замена символа "*" на "%".
		/// Пример: массив {"hello", "w*rld"} разворачивается в строку 
		/// "and ( someField like 'hello'  or  someField like 'w%rld')"
		/// </summary>
		/// <param name="array">Аргументы поиска</param>
		/// <param name="fieldName">Имя поля по которому происходит поиск</param>
		/// <returns>Отформатированная строка</returns>
		private string StringArrayToQuery(IList<string> array, string fieldName)
		{		
			StringBuilder builder = new StringBuilder();
			if ((array != null) && (array.Count > 0))
			{
				builder.Append(" and (");
				foreach (string item in array)
				{
					if (item.IndexOf("*") > -1)
						builder.Append(fieldName + " like '" + item.Replace("*", "%") + "'");
					else
						builder.Append(fieldName + " = '" + item + "'");
					builder.Append(" or "); 
				}
				builder.Remove(builder.Length - 4, 4);
				builder.Append(") ");
			}
			return builder.ToString();
		}
		/// <summary>
		/// Преобразовывает список строк для использования в блок сортировки SQL запроса.
		/// Пример: массив orderFields = {"field1", "field2"}, orderDirection = {"ASC"}
		/// преобразуется к виду " ORDER BY field1 ASC, field2 ".
		/// </summary>
		/// <param name="orderFields">Список полей из которых формируется блок сортировки.</param>
		/// <param name="orderDirection">Список направлений сортировки для orderFields.
		/// Допустимые значения: null, "ASC", "DESC".
		/// Примечание: длинна списка orderDirection может быть меньше или равна длинне orderFields.
		/// </param>
		/// <returns></returns>
		private string FormatOrderBlock(IList<string> orderFields, IList<string> orderDirection)
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

		private void LogQuery(Int32 RowCount, string FunctionName, System.DateTime StartTime)
		{
			//MySelCmd.CommandText = " insert into logs.AMPLogs(LogTime, Host, User, Function, RowCount, ProcessingTime) " + " values(now(), '" + System.Web.HttpContext.Current.Request.UserHostAddress + "', '" + UserName + "', '" + FunctionName + "', " + RowCount + ", " + System.Convert.ToInt32(DateTime.Now.Subtract(StartTime).TotalMilliseconds).ToString() + ")";
			//MySelCmd.ExecuteNonQuery();
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