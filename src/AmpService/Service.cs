using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

			int i;
			var searchByApmCode = name != null && name.Length > 0 && int.TryParse(name[0], out i);

			var query = new Query().Select(@"
p.id as PrepCode, 
cn.Name, 
cf.Form,
cast(ifnull(group_concat(distinct pv.Value ORDER BY prop.PropertyName, pv.Value SEPARATOR ', '), '') as CHAR) as Properties,
c.VitallyImportant,
ifnull(m.Mnn, '') as Mnn")
				.From(@"
Catalogs.Catalog c
JOIN Catalogs.CatalogNames cn on cn.id = c.nameid
	left join Catalogs.Mnn m on cn.MnnId = m.Id
JOIN Catalogs.CatalogForms cf on cf.id = c.formid
JOIN Catalogs.Products p on p.CatalogId = c.Id
LEFT JOIN Catalogs.ProductProperties pp on pp.ProductId = p.Id
LEFT JOIN Catalogs.PropertyValues pv on pv.id = pp.PropertyValueId
LEFT JOIN Catalogs.Properties prop on prop.Id = pv.PropertyId")
				.Where("p.Hidden = 0", "c.Hidden = 0")
				.Where(Utils.StringArrayToQuery(form, "cf.Form"))
				.GroupBy("p.Id")
				.OrderBy("cn.Name, cf.Form")
				.Limit(Utils.GetLimitString(selStart, limit).ToLower().Replace("limit ", ""));

			if (offerOnly)
			{
				query.Join("JOIN Farm.Core0 c0 on c0.ProductId = p.Id", "JOIN activeprices ap on ap.PriceCode = c0.PriceCode");

				if (newEar || searchByApmCode)
					query.Join("LEFT JOIN farm.core0 ampc on ampc.ProductId = p.Id and ampc.codefirmcr = c0.codefirmcr and ampc.PriceCode = 1864");

				if (newEar)
					query.Where("ampc.id is null");

				if (searchByApmCode)
					query.Where(Utils.StringArrayToQuery(name, "ampc.code"));
			}

			if (!searchByApmCode)
				query.Where(Utils.StringArrayToQuery(name, "cn.Name"));

			if (offerOnly && priceId != null && !(priceId.Length == 1 && priceId[0] == 0))
				query.Where(Utils.StringArrayToQuery(priceId, "ap.pricecode"));

			With.Connection(c => {
				if (offerOnly)
					using(InvokeGetActivePrices(c))
						query.Table(data, "Catalog", c);
				else
					query.Table(data, "Catalog", c);
			});

			return data;
		}

		public virtual DataSet GetPriceCodeByName(string[] firmNames)
		{
			var adapter = new MySqlDataAdapter();
			adapter.SelectCommand = new MySqlCommand(@"
select p.PriceCode,
		p.PriceName,
		pd.PriceInfo,
		s.Id as FirmCode,
		s.Name as FirmName,
		rd.ContactInfo,
		rd.OperativeInfo,
		ifnull(p.MinReq, 0) as MinReq
from usersettings.prices p
	join Customers.Suppliers as s ON p.FirmCode = s.Id
	join usersettings.pricesdata pd on pd.PriceCode = p.PriceCode
	join usersettings.RegionalData rd on p.FirmCode = rd.FirmCode and p.RegionCode = rd.RegionCode
");

			if (firmNames != null && firmNames.Length > 0)
				adapter.SelectCommand.CommandText += " where " + Utils.StringArrayToQuery(firmNames, "s.Name");

			var data = new DataSet();
			With.Connection(c => {
				using(InvokeGetPrices(c))
				{
					adapter.SelectCommand.Connection = c;
					adapter.Fill(data, "PriceList");
				}
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
				return orderIds.Select((orderId, i) => {

					var message = "";
					if (messages.Length > i)
						message = messages[i];
					
					var toOrder = new ToOrder
					{
						OrderId = orderIds[i],
						Quantity = quanties[i],
						Message = message,
						OrderCode1 = orderCodes1[i],
						OrderCode2 = orderCodes2[i],
						Junk = junks[i],
					};
					return toOrder;
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

			var offers = _offerRepository.GetByIds(ServiceContext.Orderable, orderIds);

			var orders = new List<Order>();
			var toOrders = ToOrder.FromRequest(orderIds, quanties, messages, orderCodes1, orderCodes2, junks);
			foreach(var toOrder in toOrders)
			{
				var offer = offers.FirstOrDefault(o => o.Id.CoreId == toOrder.OrderId);
				if (offer == null)
					continue;

				var order = orders.FirstOrDefault(o => o.PriceList.PriceCode == offer.PriceList.Id.Price.PriceCode);
				if (order == null)
				{
					order = new Order(offer.PriceList, ServiceContext.User, rules);
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
			table.Columns.Add("OrderID", typeof (long));
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
		ifnull(c.RequestRatio, 0) RequestRatio,
		ifnull(c.OrderCost, 0) MinOrderSum,
		ifnull(c.MinOrderCount, 0) MinOrderCount,
		0 UpCost,
		if(if(round(cc.Cost*ap.UpCost,2)<c.MinBoundCost, c.MinBoundCost, round(cc.Cost*ap.UpCost,2))>c.MaxBoundCost,c.MaxBoundCost, if(round(cc.Cost*ap.UpCost,2)<c.MinBoundCost, c.MinBoundCost, round(cc.Cost*ap.UpCost,2))) Cost,
		ap.pricecode SalerID,
		s.Name SalerName,
		ap.PriceDate,
		c.ProductId PrepCode,
		c.synonymcode OrderCode1,
		c.synonymfirmcrcode OrderCode2
FROM farm.core0 c
	JOIN ActivePrices ap on c.PriceCode = ap.PriceCode
		JOIN usersettings.PricesData pd on pd.PriceCode = ap.PriceCode
	JOIN farm.CoreCosts cc on cc.Core_Id = c.Id and cc.PC_CostCode = ap.CostCode
	JOIN Customers.Suppliers as s ON ap.FirmCode = s.Id
	JOIN farm.synonym s on s.PriceCode = ifnull(pd.parentsynonym, ap.pricecode) and s.SynonymCode = c.SynonymCode
	LEFT JOIN farm.core0 ampc ON ampc.ProductId = c.ProductId and ampc.codefirmcr = c.codefirmcr and ampc.PriceCode = 1864
	LEFT JOIN farm.synonymfirmcr scr ON scr.PriceCode = ifnull(pd.ParentSynonym, ap.pricecode) and c.synonymfirmcrcode = scr.synonymfirmcrcode
WHERE	c.SynonymCode = ?SynonymCode
		and c.SynonymFirmCrCode = ?SynonymFirmCrCode
		and c.Junk = ?Junk;";

			With.Connection(c => {
				using(InvokeGetActivePrices(c))
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

		[OfferRowCalculator("PrepCode")]
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
			var validRequestFields = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase) {
				{"OrderID", "c.id"},
				{"SalerCode", "c.Code"},
				{"SalerName", "s.Name"},
				{"ItemID", "ampc.Code"},
				{"OriginalCR", "sfc.Synonym"},
				{"OriginalName", "s.Synonym"},
				{"PriceCode", "ap.PriceCode"},
				{"PrepCode", "c.ProductId"}
			};

			var validSortFields = new List<string> {
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
			//      имя поля      список значений для него
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

			Func<MySqlConnection, DisposibleAction>  prerequirements;
			string command;
			if (!onlyLeader)
			{
				prerequirements = c => InvokeGetActivePrices(c);

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
		ifnull(c.RequestRatio, 0) RequestRatio,
		ifnull(c.OrderCost, 0) MinOrderSum,
		ifnull(c.MinOrderCount, 0) MinOrderCount,
		0 UpCost,
		ap.PriceCode PriceCode,
		s.Name SalerName,
		ap.PriceDate,
		c.ProductId as PrepCode,
		c.synonymcode OrderCode1,
		c.synonymfirmcrcode OrderCode2,
		if(if(round(cc.Cost*ap.UpCost,2)<c.MinBoundCost, c.MinBoundCost, round(cc.Cost*ap.UpCost,2))>c.MaxBoundCost, c.MaxBoundCost, if(round(cc.Cost*ap.UpCost,2)<c.MinBoundCost, c.MinBoundCost, round(cc.Cost*ap.UpCost,2))) as Cost
FROM farm.core0 c
	JOIN ActivePrices ap on c.PriceCode = ap.PriceCode
		JOIN farm.CoreCosts cc on cc.Core_Id = c.Id and cc.PC_CostCode = ap.CostCode
		JOIN Customers.Suppliers as s ON ap.FirmCode = s.Id
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
				prerequirements = c => InvokeGetOffers(c);

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
		ifnull(c.RequestRatio, 0) RequestRatio,
		ifnull(c.OrderCost, 0) MinOrderSum,
		ifnull(c.MinOrderCount, 0) MinOrderCount,
		0 UpCost,
		ap.pricecode PriceCode,
		s.Name SalerName,
		ap.PriceDate,
		p.Id PrepCode,
		c.synonymcode OrderCode1,
		c.synonymfirmcrcode OrderCode2,
		offers.MinCost as Cost
FROM UserSettings.MinCosts as offers
	JOIN farm.core0 as c on c.id = offers.id
		JOIN UserSettings.activeprices as ap ON ap.PriceCode = offers.PriceCode
			JOIN Customers.Suppliers as s ON ap.FirmCode = s.Id
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

			With.Connection(c => {
				using(prerequirements(c))
				{
					var dataAdapter = new MySqlDataAdapter(command, c);
					dataAdapter.Fill(data, "PriceList");
				}
			});

			return data;
		}

		private DisposibleAction InvokeGetOffers(MySqlConnection connection)
		{
			return StorageProcedures.FutureGetOffers(connection, ServiceContext.User.Id);
		}

		private DisposibleAction InvokeGetActivePrices(MySqlConnection c)
		{
			return StorageProcedures.FutureGetActivePrices(c, ServiceContext.User.Id);
		}

		private DisposibleAction InvokeGetPrices(MySqlConnection c)
		{
			var user = ServiceContext.User;
			return StorageProcedures.GetPricesForAddress(c, user.Id, user.AvaliableAddresses.Single().Id);
		}

		public virtual DataSet GetOrdersByDate(DateTime olderThan, int priceCode)
		{
			return GetOrders(adapter => {
				var filter = "and oh.WriteTime >= ?OlderThan";
				adapter.SelectCommand.Parameters.AddWithValue("?OlderThan", olderThan);
				if (priceCode > 0)
				{
					filter += " and oh.PriceCode = ?PriceCode";
					adapter.SelectCommand.Parameters.AddWithValue("?PriceCode", priceCode);
				}
				return filter;
			});
		}

		public virtual DataSet GetOrder(uint orderId)
		{
			return GetOrders(adapter => {
				adapter.SelectCommand.Parameters.AddWithValue("?OrderId", orderId);
				return " and oh.RowId = ?OrderId";
			});
		}

		private DataSet GetOrders(Func<MySqlDataAdapter, string> action)
		{
			var adapter = new MySqlDataAdapter();
			adapter.SelectCommand = new MySqlCommand("");
			var filter = action(adapter);

			adapter.SelectCommand.CommandText = String.Format(@"
SELECT  ol.RowId OrderLineId,
		ifnull(i.SupplierClientId, '') as ClientCode,
		ifnull(ai.SupplierDeliveryId, '') as ClientCode2,
		ifnull(i.SupplierPaymentId, '') as ClientCode3,
		oh.RowID as OrderID,
		cast(oh.PriceDate as char) as PriceDate,
		cast(oh.WriteTime as char) as OrderDate,
		ifnull(oh.ClientAddition, '') as Comment,
		ol.Code as ItemID,
		ol.Cost,
		ol.Quantity,
		if(length(ifnull(i.SupplierClientId, ''))< 1, concat(cd.Name, '; ', a.Address, '; ',
		(select c.contactText
		from contacts.contact_groups cg
		  join contacts.contacts c on cg.Id = c.ContactOwnerId
		where cd.ContactGroupOwnerId = cg.ContactGroupOwnerId
			  and cg.Type = 0
			  and c.Type = 1
		limit 1)), '') as Addition,
		if(length(ifnull(i.SupplierClientId, '')) < 1, cd.Name, '') as ClientName,
		ifnull(i.PriceMarkup + pd.UpCost + prd.UpCost, 0) PriceMarkup
FROM UserSettings.PricesData pd 
	JOIN Orders.OrdersHead oh ON pd.PriceCode = oh.PriceCode
	JOIN Orders.OrdersList ol ON oh.RowID = ol.OrderID
	join Usersettings.pricesregionaldata prd on prd.PriceCode = pd.PriceCode and prd.RegionCode = oh.RegionCode
	JOIN Customers.Intersection i ON i.ClientId = oh.ClientCode and i.RegionId = oh.RegionCode and i.PriceId = oh.PriceCode
	JOIN Customers.Clients cd ON cd.Id = oh.ClientCode
	join Customers.Addresses a on a.Id = oh.AddressId
		join Customers.AddressIntersection ai on i.Id = ai.IntersectionId and a.Id = ai.AddressId
		JOIN UserSettings.RetClientsSet rcs on rcs.ClientCode = cd.Id
WHERE (pd.FirmCode = 62 or pd.FirmCode = 94)
	and oh.Deleted = 0
	and oh.Submited = 1
	and rcs.ServiceClient = 0
	and rcs.InvisibleOnFirm != 2
	{0}
order by OrderDate
", filter);

			var data = new DataSet();

			With.Connection(c => {
				adapter.SelectCommand.Connection = c;
				adapter.Fill(data);
			});

			return data;

		}
	}
}
