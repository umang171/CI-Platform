--2. write a SQL query to Create Stored procedure in the Northwind database to retrieve
--Employee Sales by Country
SELECT * FROM orders;
select * from orders WHERE ShipCountry='USA';

CREATE PROCEDURE spGetSalesByCountry
@Country nvarchar(15)
AS
BEGIN
	SELECT * FROM orders WHERE ShipCountry=@Country;
END

DECLARE @Country nvarchar(15);
SET @Country='USA';
EXECUTE spGetSalesByCountry @Country;