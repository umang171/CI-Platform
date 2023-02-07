--3. write a SQL query to Create Stored procedure in the Northwind database to retrieve
--Sales by YearSELECT Year(orderdate) FROM orders;
SELECT * FROM orders WHERE Year(orderdate)=1996;


CREATE PROCEDURE spGetSalesByYear
@year int
AS
BEGIN
	SELECT * FROM orders WHERE Year(orderdate)=@year;
END

DROP PROCEDURE spGetSalesByYear;

DECLARE @year int
SET @year=1996;
EXECUTE spGetSalesByYear @year;
