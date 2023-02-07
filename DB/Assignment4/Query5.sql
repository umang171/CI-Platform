--5. write a SQL query to Create Stored procedure in the Northwind database to retrieve
--Ten Most Expensive Products
SELECT Top 10 * FROM Products ORDER BY UnitPrice DESC;

CREATE PROCEDURE spGetTopExpnesiveProducts
AS
BEGIN	
	SELECT Top 10 * FROM Products ORDER BY UnitPrice DESC;
END

Execute spGetTopExpnesiveProducts