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
        public static DataTable GetPricesDataTable()
        {
            DataTable dtPrices = new DataTable();
            dtPrices.Columns.Add("OrderID", typeof(long));
            dtPrices.Columns.Add("OriginalOrderID", typeof(long));
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

        public static void MailErr(string ProcessName, string ErrMessage, string ErrSource, string UserName)
        {
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
                Message.BodyEncoding = Encoding.UTF8;
                Client.Send(Message);
            }
            catch
            {
            }
        }
    }
}
