DROP FUNCTION if exists Catalogs.GetProductName;
CREATE DEFINER=`RootDBMS`@`127.0.0.1` FUNCTION Catalogs.`GetProductName`(ProductId int unsigned) RETURNS varchar(255) CHARSET cp1251
    READS SQL DATA
    DETERMINISTIC
BEGIN
  DECLARE result varchar(255);
  select
    concat(cn.Name, ' ', CatalogForms.Form, ' ',
     cast(GROUP_CONCAT(ifnull(PropertyValues.Value, '')
                        order by Properties.PropertyName, PropertyValues.Value
                        SEPARATOR ', '
                       ) as char)
    ) as FullForm
    into result
  from
    (
     catalogs.products,
     catalogs.catalog,
     catalogs.CatalogForms
     )
     join Catalogs.CatalogNames cn on cn.Id = catalog.NameId
     left join catalogs.ProductProperties on ProductProperties.ProductId = Products.Id
     left join catalogs.PropertyValues on PropertyValues.Id = ProductProperties.PropertyValueId
     left join catalogs.Properties on Properties.Id = PropertyValues.PropertyId
   where
      products.Id = ProductId
   and catalog.Id = products.CatalogId
   and CatalogForms.Id = catalog.FormId;

  return result;
END;
