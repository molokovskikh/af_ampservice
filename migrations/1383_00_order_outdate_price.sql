alter table Orders.OrdersHead
add column IsPriceOutdate tinyint(1) unsigned not null default 0;
