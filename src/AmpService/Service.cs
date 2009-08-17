using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using System.Xml;
using Common.Models;
using Common.Models.Repositories;
using Common.MySql;
using Common.Service;
using Common.Tools;
using MySql.Data.MySqlClient;
using With=Common.MySql.With;

namespace AmpService
{
	public class Service
	{
		private readonly IRepository<Order> _orderRepository;
		private readonly IOfferRepository _offerRepository;
		private readonly IRepository<OrderRules> _rulesRepository;

		public Service(IOfferRepository offerRepository,
			IRepository<Order> orderRepository,
			IRepository<OrderRules> rulesRepository)
		{
			_offerRepository = offerRepository;
			_orderRepository = orderRepository;
			_rulesRepository = rulesRepository;
		}

		public virtual DataSet GetNameFromCatalog(string[] name,
												  string[] form,
												  bool newEar,
												  bool offerOnly,
												  uint[] priceId,
												  uint limit,
												  uint selStart)
		{
			var data = new DataSet();
			var clientCode = ServiceContext.Client.FirmCode;
			var commandText = new StringBuilder();

			int i;
			var searchByApmCode = name != null && name.Length > 0 && int.TryParse(name[0], out i);

			if (offerOnly)
			{
				commandText.AppendLine(@"
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
				commandText.AppendLine(@"
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
				.AddCriteria(Utils.StringArrayToQuery(priceId, "ap.pricecode"),
				             offerOnly
				             && priceId != null
				             && !(priceId.Length == 1
				                  && priceId[0] == 0))
				.AddCriteria(Utils.StringArrayToQuery(form, "cf.Form"))
				.AddCriteria(Utils.StringArrayToQuery(name, "ampc.code"), searchByApmCode)
				.AddCriteria(Utils.StringArrayToQuery(name, "cn.Name"), !searchByApmCode)
				.AddCriteria("ampc.id is null", newEar)
				.AddCriteria("p.Hidden = 0")
				.AddCriteria("c.Hidden = 0");

			commandText.AppendLine("GROUP BY p.Id");
			commandText.AppendLine("ORDER BY cn.Name, cf.Form");
			commandText.AppendLine(Utils.GetLimitString(selStart, limit));

			With.Slave(c => {
				if (offerOnly)
				{
					using(StorageProcedures.GetActivePrices(c, clientCode))
					{
						var adapter = new MySqlDataAdapter(commandText.ToString(), c);
						adapter.SelectCommand.Parameters.AddWithValue("?ClientCode", clientCode);
						adapter.Fill(data, "Catalog");
					}
				}
				else
				{
					var adapter = new MySqlDataAdapter(commandText.ToString(), c);
					adapter.SelectCommand.Parameters.AddWithValue("?ClientCode", clientCode);
					adapter.Fill(data, "Catalog");
				}
			});

			return data;
		}

		public virtual DataSet GetPriceCodeByName(string[] firmNames)
		{
			var clientCode = ServiceContext.Client.FirmCode;
			var adapter = new MySqlDataAdapter();
			adapter.SelectCommand = new MySqlCommand(@"
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
        and RegionalData.RegionCode = Intersection.RegionCode");

			if (firmNames != null && firmNames.Length > 0)
				adapter.SelectCommand.CommandText += " and " + Utils.StringArrayToQuery(firmNames, "ClientsData.ShortName");

			adapter.SelectCommand.Parameters.AddWithValue("?ClientCode", clientCode);

			var data = new DataSet();
			With.Slave(c => {
				adapter.SelectCommand.Connection = c;
				adapter.Fill(data, "PriceList");
			});
			return data;
		}

		public class ToOrder
		{
			public ulong OrderId;
			public uint Quantity;
			public string Message;
			public uint OrderCode1;
			public uint OrderCode2;
			public bool Junk;
			public OrderItem OrderItem;

			public static IList<ToOrder> FromRequest(ulong[] orderIds, 
													 uint[] quanties,
													 string[] messages,
													 uint[] orderCodes1,
													 uint[] orderCodes2,
													 bool[] junks)
			{
				return orderIds.Select((orderId, i) => new ToOrder
				{
					OrderId = orderIds[i],
					Quantity = quanties[i],
					Message = messages[i],
					OrderCode1 = orderCodes1[i],
					OrderCode2 = orderCodes2[i],
					Junk = junks[i],
				}).ToList();
			}
		}

		public virtual DataSet PostOrder(ulong[] orderIds,
										 uint[] quanties, 
										 string[] messages, 
										 uint[] orderCodes1, 
										 uint[] orderCodes2,
										 bool[] junks)
		{
			if (orderIds == null
				|| quanties == null
				|| orderCodes1 == null
				|| orderCodes2 == null
				|| junks == null
				|| orderIds.Length != quanties.Length
				|| orderIds.Length != orderCodes1.Length
				|| orderIds.Length != orderCodes2.Length
				|| orderIds.Length != junks.Length)
				return null;

			var client = ServiceContext.Client;
			var rules = _rulesRepository.Get(client.FirmCode);

			var offers = _offerRepository.GetByIds(client, orderIds);
			var orders = new List<Order>();
			var toOrders = ToOrder.FromRequest(orderIds, quanties, messages, orderCodes1, orderCodes2, junks);
			foreach(var toOrder in toOrders)
			{
				var offer = offers.FirstOrDefault(o => o.Id == toOrder.OrderId);
				if (offer == null)
					continue;

				var order = orders.FirstOrDefault(o => o.PriceList.PriceCode == offer.PriceList.PriceCode);
				if (order == null)
				{
					order = new Order(true, offer.PriceList, client, rules);
					order.ClientAddition = toOrder.Message;
					orders.Add(order);
				}
				toOrder.OrderItem = order.AddOrderItem(offer, toOrder.Quantity);
			}

			Common.Models.With.Transaction(() => {
				orders.Each(_orderRepository.Save);
			});

			var result = new DataSet();
			var table = result.Tables.Add("Prices");
			table.Columns.Add("OrderID", typeof (uint));
			table.Columns.Add("OriginalOrderID", typeof (ulong));

			var selectOffer = @"
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
		and c.Junk = ?Junk;";

			With.Slave(c => {
				using(StorageProcedures.GetActivePrices(c, client.FirmCode))
				{
					foreach (var toOrder in toOrders)
					{
						var row = result.Tables[0].NewRow();
						row["OriginalOrderID"] = toOrder.OrderId;
						result.Tables[0].Rows.Add(row);
						if (toOrder.OrderItem != null)
						{
							row["OrderID"] = toOrder.OrderItem.Order.RowId;
							continue;
						}
						var data = new DataSet();
						var dataAdapter = new MySqlDataAdapter(selectOffer, c);
						dataAdapter.SelectCommand.Parameters.AddWithValue("?ClientCode", client.FirmCode);
						dataAdapter.SelectCommand.Parameters.AddWithValue("?SynonymCode", toOrder.OrderCode1);
						dataAdapter.SelectCommand.Parameters.AddWithValue("?SynonymFirmCrCode", toOrder.OrderCode2);
						dataAdapter.SelectCommand.Parameters.AddWithValue("?Junk", toOrder.Junk);
						dataAdapter.Fill(data);
						if (data.Tables[0].Rows.Count == 0)
						{
							row["OrderID"] = -1;
							continue;
						}
						data.Tables[0].Rows[0].CopyTo(row);
					}
				}
			});

			return result; 
		}

		public virtual DataSet GetPrices(bool onlyLeader, 
										 bool newEar,
										 string[] rangeField,
										 string[] rangeValue, 
										 string[] sortField,
										 string[] sortOrder, 
										 uint limit, 
										 uint selStart)
		{
			//словарь для валидации и трансляции имен полей для клиента в имена полей для использования в запросе
			var validRequestFields = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase)
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

			//проверка входящих параметров
			if (rangeValue == null || rangeField == null
			    || rangeField.Length != rangeValue.Length)
				throw new ArgumentException("Входящие параметры не валидны");
			//TODO: в принципе в этой проверке нет нужды если будет неверное название поля 
			//то будет Exception на этапе трансляции

			//проверка имен полей для фильтрации
			foreach (var fieldName in rangeField)
				if (!validRequestFields.ContainsKey(fieldName))
					throw new ArgumentException(String.Format("По полю {0} не может производиться фильтрация", fieldName), fieldName);
			//проверка имен полей для сортировки
			if (sortField != null)
			{
				foreach (var fieldName in sortField)
					if (!validSortFields.Exists(value => String.Compare(fieldName, value, true) == 0))
						throw new ArgumentException(String.Format("По поляю {0} не может производиться сортировка", fieldName), fieldName);
			}
			//проверка направлений сортировки
			if (sortOrder != null)
			{
				foreach (var direction in sortOrder)
					if (!(String.Compare(direction, "Asc", true) == 0 || String.Compare(direction, "Desc", true) == 0))
						throw new ArgumentException(
							String.Format("Не допустимое значение направления сортровки {0}. Допустимые значение \"Asc\" и \"Desc\"",
							              direction), direction);
			}

			//словарь хранящий имена фильтруемых полей и значения фильтрации
			//           |            |
			//                имя поля      список значений для него
			var filtedField = new Dictionary<string, List<string>>();
			//разбирается входящие параметры args.RangeField и args.RangeValue и одновременно клиентские имена
			//транслируются во внутренние
			for (var i = 0; i < rangeField.Length; i++)
			{
				//преобразовываем клиентские названия полей во внутренние
				var innerFieldName = validRequestFields[rangeField[i]];
				//если в словаре не такого поля 
				if (!filtedField.ContainsKey(innerFieldName))
					//то добавляем его и создаем массив для хранения его значений
					filtedField.Add(innerFieldName, new List<string>());
				//добавляем значение для фильтрации
				filtedField[innerFieldName].Add(rangeValue[i]);
			}

			if (filtedField.Count == 0)
				throw new SoapException("Нет критериев для фильтрации", new XmlQualifiedName("0"));

			var data = new DataSet();

			var predicatBlock = "";

			if (newEar)
				predicatBlock += " ampc.id is null ";

			foreach (var fieldName in filtedField.Keys)
			{
				if (predicatBlock != "")
					predicatBlock += " and ";
				predicatBlock += Utils.StringArrayToQuery(filtedField[fieldName], fieldName);
			}

			var clientCode = ServiceContext.Client.FirmCode;
			Func<MySqlConnection, DisposibleAction>  prerequirements;
			string command;
			if (!onlyLeader)
			{
				prerequirements = c => StorageProcedures.GetActivePrices(c, clientCode);

				command = @"
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
		c.RequestRatio,
		c.OrderCost as MinOrderSum,
		c.MinOrderCount,
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
				if (predicatBlock != "")
					command += " and " + predicatBlock;

				command += @"; select * from offers ";
			}
			else
			{
				prerequirements = c => StorageProcedures.GetOffers(c, clientCode);
				command = @"
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
					command += " where " + predicatBlock;

			}

			//группировка нужна т.к. в асортиментном прайсе амп может быть несколько записей 
			//соответствующие с одинаковым ProductId и CodeFirmCr но смысла в этом нет 
			//ни какого они просто не нужны
			command += " GROUP BY OrderID";
			command += Utils.FormatOrderBlock(sortField, sortOrder);
			command += Utils.GetLimitString(selStart, limit);

			With.Slave(c => {
				using(prerequirements(c))
				{
					var dataAdapter = new MySqlDataAdapter(command, c);
					dataAdapter.Fill(data, "PriceList");
				}
			});

			return data;
		}

		public virtual DataSet GetOrdersByDate(DateTime olderThan, int priceCode)
		{
			var adapter = new MySqlDataAdapter();
			adapter.SelectCommand = new MySqlCommand(@"
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
		JOIN UserSettings.RetClientsSet rcs on rcs.ClientCode = cd.FirmCode
WHERE (pd.FirmCode = 62 or pd.FirmCode = 94)
	  and oh.Deleted = 0
	  and oh.Submited = 1
	  and oh.WriteTime >= ?OlderThan
	  and rcs.ServiceClient = 0
	  and rcs.InvisibleOnFirm != 2");
			adapter.SelectCommand.Parameters.AddWithValue("?OlderThan", olderThan);
			if (priceCode > 0)
			{
				adapter.SelectCommand.CommandText += " and oh.PriceCode = ?PriceCode";
				adapter.SelectCommand.Parameters.AddWithValue("?PriceCode", priceCode);
			}
			adapter.SelectCommand.CommandText += " order by WriteTime;";
			var data = new DataSet();

			With.Slave(c => {
				adapter.SelectCommand.Connection = c;
				adapter.Fill(data);
			});

			return data;
		}
	}
}
