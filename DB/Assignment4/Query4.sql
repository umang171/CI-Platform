--4. write a SQL query to Create Stored procedure in the Northwind database to retrieve
--Sales By Category

SELECT * FROM Products;
SELECT * FROM [Order Details];
SELECT * FROM Orders;
SELECT * FROM categories;

SELECT * FROM Orders WHERE OrderID in(
SELECT OrderId FROM [Order Details] WHERE ProductID in(
SELECT ProductID FROM Products WHERE CategoryID in
(SELECT categoryId FROM categories WHERE CategoryName='Beverages')));

CREATE PROCEDURE spGetSalesByCategory
@Category nvarchar(20)
AS
BEGIN	
	SELECT * FROM Orders WHERE OrderID in(
	SELECT OrderId FROM [Order Details] WHERE ProductID in(
	SELECT ProductID FROM Products WHERE CategoryID in
	(SELECT categoryId FROM categories WHERE CategoryName=@Category)));
END

DECLARE @Category nvarchar(20)
SET @Category='Beverages';
EXECUTE spGetSalesByCategory @Category;
