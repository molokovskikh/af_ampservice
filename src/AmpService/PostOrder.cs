using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Net.Mail;


namespace AMPWebService
{
    public class PostOrder
    {
        //Отдает структуру таблицы результата
        private static DataTable GetPricesDataTable()
        {
            DataTable dtPrices = new DataTable();
            dtPrices.Columns.Add("OrderID", typeof(int));
            dtPrices.Columns.Add("OriginalOrderID", typeof(int));
            //Надо сказать, что данная колонка не присутствует в таблице
            dtPrices.Columns.Add("SalerCode");
            dtPrices.Columns.Add("CreaterCode");
            dtPrices.Columns.Add("ItemID");
            dtPrices.Columns.Add("OriginalName");
            dtPrices.Columns.Add("OriginalCr");
            dtPrices.Columns.Add("Unit");
            dtPrices.Columns.Add("Volume");
            dtPrices.Columns.Add("Quantity");
            dtPrices.Columns.Add("Note");
            dtPrices.Columns.Add("Period");
            dtPrices.Columns.Add("Doc");
            dtPrices.Columns.Add("Junk");
            dtPrices.Columns.Add("UpCost", typeof(decimal));
            dtPrices.Columns.Add("Cost", typeof(decimal));
            dtPrices.Columns.Add("SalerID", typeof(uint));
            dtPrices.Columns.Add("SalerName");
            dtPrices.Columns.Add("PriceDate");
            dtPrices.Columns.Add("PrepCode", typeof(uint));
            dtPrices.Columns.Add("OrderCode1", typeof(uint));
            dtPrices.Columns.Add("OrderCode2", typeof(uint));
            return dtPrices;
        }

        private static DataTable GetPos(MySqlCommand cmd, MySqlDataAdapter da, int SynonymCode, int SynonymFirmCrCode, bool Junk)
        {
            DataTable dtRes = GetPricesDataTable();
            cmd.Parameters["SynonymCode"].Value = SynonymCode;
            cmd.Parameters["SynonymFirmCrCode"].Value = SynonymFirmCrCode;
            cmd.Parameters["Junk"].Value = Junk;
            da.Fill(dtRes);
            return dtRes;
        }

        public static void MailErr(string ProcessName, string ErrMessage, string ErrSource, string UserName)
        {
#if !DEBUG
            MailMessage Message = new MailMessage();
            MailAddress Address = new MailAddress("service@analit.net");
            SmtpClient Client = new SmtpClient("box.analit.net");
            try
            {
                Message.From = Address;
                Message.Subject = "Ошибка в AMPService";
                Message.Body = "Процесс: " + ProcessName + Environment.NewLine +
                    "Ошибка: " + ErrMessage + Environment.NewLine +
                    "Источник: " + ErrSource + Environment.NewLine +
                    "Логин: " + UserName;
                Message.To.Add(Address);
                Message.BodyEncoding = System.Text.Encoding.UTF8;
                Client.Send(Message);
            }
            catch
            {
            }
#endif
        }

        public static DataSet PostOrderMethod(int[] CoreIDs, int[] Quantities, string[] Messages, int[] SynonymCodes, int[] SynonymFirmCrCodes, bool[] Junks, ulong ClientCode, string UserName, string Host)
        {
            MySqlConnection MyCn = new MySqlConnection(Literals.ConnectionString);
            MySqlCommand MySelCmd = new MySqlCommand();
            MySqlDataAdapter MyDA = new MySqlDataAdapter(MySelCmd);
            MySqlTransaction MyTrans = null;
            bool Quit = false;
            bool Success = false;
            string CoreIDString;
            int Index;
            DataSet dsRes = null;
            DataTable dtPricesRes;

            MySelCmd.Connection = MyCn;

            int[] Res = new int[CoreIDs.Length];
            for(int i = 0; i < Res.Length; i++)
            {
                Res[i] = -1;
            }
            DataSet dsPost;
            DataTable dtSummaryOrder;
            DataTable dtOrderHead;

            try
            {
                do
                {
                    try
                    {
                        dsRes = null;
                        if (MyCn.State == ConnectionState.Closed)
                        {
                            MyCn.Open();
                        }
                        MyTrans = MyCn.BeginTransaction();
                        MySelCmd.Transaction = MyTrans;

                        //Если длины не равны, то возвращаем null
                        if ((CoreIDs == null) || (Quantities == null) || (SynonymCodes == null) || (SynonymFirmCrCodes == null)
                            || (Junks == null)
                            || (CoreIDs.Length != Quantities.Length)
                            || (CoreIDs.Length != SynonymCodes.Length) || (CoreIDs.Length != SynonymFirmCrCodes.Length)
                            || (CoreIDs.Length != Junks.Length))
                            return null;

                        CoreIDString = "(";
                        foreach (int ID in CoreIDs)
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

                        MySelCmd.CommandText =
@"select
  cd.FirmCode ClientCode,
  cd.RegionCode,
  pc.ShowPriceCode PriceCode,
  cast(if(fr.datelastform>fr.DateCurPrice, fr.DateCurPrice, fr.DatePrevPrice) as datetime) PriceDate,
  c.Id,
  c.FullCode,
  c.CodeFirmCr,
  c.SynonymCode,
  c.SynonymFirmCrCode,
  c.Code,
  c.CodeCr,
  0 Quantity,
  c.Junk>0 Junk,
  c.Await>0 Await,
  c.BaseCost,
  round(if((1+pd.UpCost/100)*(1+prd.UpCost/100) *(1+(ins.PublicCostCorr+ins.FirmCostCorr)/100) *c.BaseCost<c.minboundcost, c.minboundcost, (1+pd.UpCost/100)*(1+prd.UpCost/100) *(1+(ins.PublicCostCorr+ins.FirmCostCorr)/100) *c.BaseCost), 2) Cost
from
  (
  usersettings.clientsdata cd,
  farm.core0 c
  )
  inner join farm.formrules fr on fr.FirmCode = c.FirmCode
  inner join usersettings.intersection ins on ins.ClientCode = cd.FirmCode and ins.RegionCode = cd.RegionCode and ins.CostCode = c.FirmCode
  inner join usersettings.pricesdata pd on pd.PriceCode = ins.PriceCode
  inner join usersettings.pricescosts pc on pc.CostCode = c.FirmCode
  inner join usersettings.pricesregionaldata prd on prd.PriceCode = ins.PriceCode and prd.RegionCode = cd.RegionCode
where
  cd.FirmCode = ?ClientCode
and c.ID in " + CoreIDString;

                        MySelCmd.Parameters.Clear();
                        MySelCmd.Parameters.Add("ClientCode", ClientCode);
                        MyDA.Fill(dsPost, "SummaryOrder");
                        dtSummaryOrder = dsPost.Tables["SummaryOrder"];

                        DataRow[] drs;
                        dtSummaryOrder.Columns.Add(new DataColumn("Message", typeof(string)));
                        for (int i = 0; i < CoreIDs.Length; i++)
                        {
                            drs = dtSummaryOrder.Select("Id = " + CoreIDs[i].ToString());
                            if (drs.Length > 0)
                            {
                                drs[0]["Quantity"] = Quantities[i];
                                if ((Messages != null) && (Messages.Length > i))
                                    drs[0]["Message"] = Messages[i];
                            }
                        }

                        dtOrderHead = dtSummaryOrder.DefaultView.ToTable(true, "ClientCode", "RegionCode", "PriceCode", "PriceDate");
                        dtOrderHead.Columns.Add(new DataColumn("OrderID", typeof(ulong)));

                        DataRow[] drOrderList;
                        foreach (DataRow drOH in dtOrderHead.Rows)
                        {
                            drOrderList = dtSummaryOrder.Select("PriceCode = " + drOH["PriceCode"].ToString());
                            if (drOrderList.Length > 0)
                            {
                                MySelCmd.CommandText = "insert into orders.ordershead (WriteTime, ClientCode, PriceCode, RegionCode, PriceDate, RowCount, ClientAddition)" +
                                    "values(now(), ?ClientCode, ?PriceCode, ?RegionCode, ?PriceDate, ?RowCount, ?ClientAddition);";
                                MySelCmd.CommandText += "select LAST_INSERT_ID()";
                                MySelCmd.Parameters.Clear();
                                MySelCmd.Parameters.Add("ClientCode", drOH["ClientCode"]);
                                MySelCmd.Parameters.Add("PriceCode", drOH["PriceCode"]);
                                MySelCmd.Parameters.Add("RegionCode", drOH["RegionCode"]);
                                MySelCmd.Parameters.Add("PriceDate", drOH["PriceDate"]);
                                MySelCmd.Parameters.Add("RowCount", drOrderList.Length);
                                MySelCmd.Parameters.Add("ClientAddition", drOrderList[0]["Message"]);
                                drOH["OrderID"] = Convert.ToUInt64(MySelCmd.ExecuteScalar());
                                MySelCmd.CommandText = "insert into orders.orderslist (OrderID, FullCode, CodeFirmCr, SynonymCode, SynonymFirmCrCode, Code, CodeCr, Quantity, Junk, Await, Cost) values (?OrderID, ?FullCode, ?CodeFirmCr, ?SynonymCode, ?SynonymFirmCrCode, ?Code, ?CodeCr, ?Quantity, ?Junk, ?Await, ?Cost);";
                                MySelCmd.Parameters.Clear();
                                MySelCmd.Parameters.Add("OrderID", MySqlDbType.Int64);
                                MySelCmd.Parameters.Add("FullCode", MySqlDbType.Int64);
                                MySelCmd.Parameters.Add("CodeFirmCr", MySqlDbType.Int64);
                                MySelCmd.Parameters.Add("SynonymCode", MySqlDbType.Int64);
                                MySelCmd.Parameters.Add("SynonymFirmCrCode", MySqlDbType.Int64);
                                MySelCmd.Parameters.Add("Code", MySqlDbType.String);
                                MySelCmd.Parameters.Add("CodeCr", MySqlDbType.String);
                                MySelCmd.Parameters.Add("Junk", MySqlDbType.Byte);
                                MySelCmd.Parameters.Add("Await", MySqlDbType.Byte);
                                MySelCmd.Parameters.Add("Cost", MySqlDbType.Decimal);
                                MySelCmd.Parameters.Add("Quantity", MySqlDbType.Int32);
                                foreach (DataRow drOL in drOrderList)
                                {
                                    MySelCmd.Parameters["OrderID"].Value = drOH["OrderID"];
                                    MySelCmd.Parameters["FullCode"].Value = drOL["FullCode"];
                                    MySelCmd.Parameters["CodeFirmCr"].Value = drOL["CodeFirmCr"];
                                    MySelCmd.Parameters["SynonymCode"].Value = drOL["SynonymCode"];
                                    MySelCmd.Parameters["SynonymFirmCrCode"].Value = drOL["SynonymFirmCrCode"];
                                    MySelCmd.Parameters["Code"].Value = drOL["Code"];
                                    MySelCmd.Parameters["CodeCr"].Value = drOL["CodeCr"];
                                    MySelCmd.Parameters["Junk"].Value = drOL["Junk"];
                                    MySelCmd.Parameters["Await"].Value = drOL["Await"];
                                    MySelCmd.Parameters["Cost"].Value = drOL["Cost"];
                                    MySelCmd.Parameters["Quantity"].Value = drOL["Quantity"];
                                    MySelCmd.ExecuteNonQuery();
                                    Index = Array.IndexOf(CoreIDs, Convert.ToInt32(drOL["ID"]));
                                    if (Index > -1)
                                        Res[Index] = Convert.ToInt32(drOH["OrderID"]);
                                }
                            }
                        }

                        //начинаем заполнять таблицу результатов
                        dtPricesRes = GetPricesDataTable();
                        dtPricesRes.TableName = "Prices";

                        DataRow drOK;

                        DataTable dtTemp;

                        MySelCmd.CommandText =
                            @"select
  c.id OrderID,
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
  length(c.Junk)<1 Junk,
  ins.PublicCostCorr As UpCost,
  round(if((1+pd.UpCost/100)*(1+prd.UpCost/100) *(1+(ins.PublicCostCorr+ins.FirmCostCorr)/100) *c.BaseCost<c.minboundcost, c.minboundcost, (1+pd.UpCost/100)*(1+prd.UpCost/100) *(1+(ins.PublicCostCorr+ins.FirmCostCorr)/100) *c.BaseCost), 2) Cost,
  pd.pricecode SalerID,
  ClientsData.ShortName SalerName,
  if(fr.datelastform>fr.DateCurPrice, fr.DateCurPrice, fr.DatePrevPrice) PriceDate,
  c.fullcode PrepCode,
  c.synonymcode OrderCode1,
  c.synonymfirmcrcode OrderCode2
from
  usersettings.clientsdata cd
  inner join usersettings.intersection ins on ins.ClientCode = cd.FirmCode
  inner join usersettings.retclientsset on retclientsset.clientcode=ins.clientcode
  inner join usersettings.pricesdata pd on pd.PriceCode = ins.PriceCode
  inner join usersettings.clientsdata on clientsdata.firmcode=pd.firmcode
  inner join farm.core0 c on c.FirmCode = ins.CostCode
  inner join farm.formrules fr on fr.FirmCode = pd.PriceCode
  inner join usersettings.pricesregionaldata prd on prd.PriceCode = ins.PriceCode and prd.RegionCode = ins.RegionCode
  left join farm.core0 ampc on ampc.fullcode=c.fullcode and ampc.codefirmcr=c.codefirmcr and ampc.firmcode=1864
  left join farm.synonym s on s.firmcode=ifnull(fr.parentsynonym, pd.pricecode)  and c.synonymcode=s.synonymcode
  left join farm.synonymfirmcr scr on scr.firmcode=ifnull(fr.parentsynonym, pd.pricecode)  and c.synonymfirmcrcode=scr.synonymfirmcrcode
where
  cd.FirmCode = ?ClientCode
  and ins.DisabledByClient=0
  and ins.Disabledbyfirm=0
  and ins.DisabledByAgency=0
  and clientsdata.firmstatus=1
  and clientsdata.billingstatus=1
  and clientsdata.firmtype=0
  and clientsdata.firmsegment=cd.firmsegment
  and (clientsdata.maskregion & ins.regioncode)>0
  and (cd.maskregion & ins.regioncode)>0
  and (retclientsset.workregionmask & ins.regioncode)>0
  and pd.agencyenabled=1
  and pd.enabled=1
  and ins.invisibleonclient=0
  and pd.pricetype<>1
  and to_days(now())-to_days(fr.datecurprice)<fr.maxold
  and prd.enabled=1
  and c.SynonymCode = ?SynonymCode
  and c.SynonymFirmCrCode = ?SynonymFirmCrCode
  and (if(c.Junk > 0, 1, 0) = ?Junk)
";
                        MySelCmd.Parameters.Clear();
                        MySelCmd.Parameters.Add("ClientCode", ClientCode);
                        MySelCmd.Parameters.Add("SynonymCode", MySqlDbType.Int32);
                        MySelCmd.Parameters.Add("SynonymFirmCrCode", MySqlDbType.Int32);
                        MySelCmd.Parameters.Add("Junk", MySqlDbType.Bit);

                        for (int i = 0; i < Res.Length; i++)
                        {
                            if (Res[i] > -1)
                            {
                                drOK = dtPricesRes.NewRow();
                                drOK["OrderID"] = Res[i];
                                drOK["OriginalOrderID"] = CoreIDs[i];
                                dtPricesRes.Rows.Add(drOK);
                            }
                            else
                            {
                                dtTemp = GetPos(MySelCmd, MyDA, SynonymCodes[i], SynonymFirmCrCodes[i], Junks[i]);
                                if (dtTemp.Rows.Count > 0)
                                {
                                    dtTemp.DefaultView.Sort = "Cost";
                                    drOK = dtPricesRes.NewRow();
                                    drOK.ItemArray = dtTemp.DefaultView[0].Row.ItemArray;
                                    drOK["OriginalOrderID"] = CoreIDs[i];
                                    dtPricesRes.Rows.Add(drOK);                                    
                                }
                                else
                                {
                                    drOK = dtPricesRes.NewRow();
                                    drOK["OrderID"] = -1;
                                    drOK["OriginalOrderID"] = CoreIDs[i];
                                    dtPricesRes.Rows.Add(drOK);
                                }
                            }
                        }


                        //Формируем результирующий датасет
                        dsRes = new DataSet();
                        dsRes.Tables.Add(dtPricesRes);

                        MyTrans.Commit();
                        Quit = true;
                        Success = true;
                    }
                    catch (MySqlException MySQLErr)
                    {
                        if (!(MyTrans == null))
                        {
                            MyTrans.Rollback();
                        }
                        if (MySQLErr.Number == 1213 || MySQLErr.Number == 1205)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                        else
                        {
                            Quit = true;
                            AMPWebService.PostOrder.MailErr("PostOrder", MySQLErr.Message, MySQLErr.Source, UserName);
                        }
                    }
                    catch (Exception ex)
                    {
                        Quit = true;
                        if (!(MyTrans == null))
                        {
                            MyTrans.Rollback();
                        }
                        AMPWebService.PostOrder.MailErr("PostOrder", ex.Message, ex.Source, UserName);
                    }
                }
                while (!Quit);

            }
            finally
            {
                if (MyCn.State == ConnectionState.Open)
                {
                    MyCn.Close();
                }
            }
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
            //return (Success) ? Res.ToArray() : null;
            return (Success) ? dsRes : null;
        }
    }
}
