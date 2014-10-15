drop trigger Orders.CalculateIsPriceOutdate;
CREATE DEFINER=`RootDBMS`@`127.0.0.1` TRIGGER orders.CalculateIsPriceOutdate
BEFORE INSERT
ON orders.ordershead
FOR EACH ROW
BEGIN
	declare localPriceDate datetime;

	select pi.PriceDate
	into localPriceDate
	from Customers.Intersection i
		join UserSettings.PricesCosts pc on pc.CostCode = i.CostId
			join UserSettings.PriceItems pi on pi.Id = pc.PriceItemId
	where i.PriceId = NEW.PriceCode and i.RegionId = NEW.RegionCode and i.ClientId = NEW.ClientCode
	limit 1;

	set NEW.IsPriceOutdate = localPriceDate is null or NEW.PriceDate <> localPriceDate;
END;
