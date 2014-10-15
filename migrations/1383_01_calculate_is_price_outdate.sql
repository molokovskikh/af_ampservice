CREATE DEFINER=`RootDBMS`@`127.0.0.1` TRIGGER orders.CalculateIsPriceOutdate
BEFORE INSERT
ON orders.ordershead
FOR EACH ROW
BEGIN
	declare localCostType int unsigned;
	declare localPriceDate datetime;

	select CostType
	into localCostType
	from UserSettings.PricesData
	where PriceCode = NEW.PriceCode;

	if localCostType = 0 then
		select pi.PriceDate
		into localPriceDate
		from UserSettings.PricesCosts pc
			join UserSettings.PriceItems pi on pi.Id = pc.PriceItemId
		where pc.PriceCode = NEW.PriceCode
		limit 1;

		set NEW.IsPriceOutdate = localPriceDate is null or NEW.PriceDate <> localPriceDate;
	end if;
END;
