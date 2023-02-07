--6. write a SQL query to Create Stored procedure in the Northwind database to insert
--Customer Order Details

CREATE PROCEDURE spSetOrderDetails
@CustomerID nchar(5),
@EmployeeID int,
@OrderDate datetime,
@RequiredDate datetime,
@ShippedDate datetime,
@ShipVia int,
@Freight money,
@ShipName nvarchar(40),
@ShipAddress nvarchar(60),
@ShipCity nvarchar(15),
@ShipRegion nvarchar(15),
@ShipPostalCode nvarchar(10),
@ShipCountry nvarchar(15)
AS
BEGIN	
INSERT INTO [dbo].[Orders]
           ([CustomerID]
           ,[EmployeeID]
           ,[OrderDate]
           ,[RequiredDate]
           ,[ShippedDate]
           ,[ShipVia]
           ,[Freight]
           ,[ShipName]
           ,[ShipAddress]
           ,[ShipCity]
           ,[ShipRegion]
           ,[ShipPostalCode]
           ,[ShipCountry])
     VALUES(
           @CustomerID,
           @EmployeeID,
           @OrderDate,
           @RequiredDate,
           @ShippedDate,
           @ShipVia,
           @Freight,
           @ShipName,
           @ShipAddress,
           @ShipCity,
           @ShipRegion,
           @ShipPostalCode,
           @ShipCountry)
END

DECLARE @CustomerID nchar(5);
DECLARE @EmployeeID int;
DECLARE @OrderDate datetime;
DECLARE @RequiredDate datetime;
DECLARE @ShippedDate datetime;
DECLARE @ShipVia int;
DECLARE @Freight money;
DECLARE @ShipName nvarchar(40);
DECLARE @ShipAddress nvarchar(60);
DECLARE @ShipCity nvarchar(15);
DECLARE @ShipRegion nvarchar(15);
DECLARE @ShipPostalCode nvarchar(10);
DECLARE @ShipCountry nvarchar(15);
SET @CustomerID='TOMSP';
SET @EmployeeID=6;
SET @OrderDate='1996-07-04 00:00:00.000';
SET @RequiredDate='1996-08-01 00:00:00.000';
SET @ShippedDate='1996-07-16 00:00:00.000';
SET @ShipVia=3;
SET @Freight=5;
SET @ShipName='Frankenversand';
SET @ShipAddress='Berliner Platz 43';
SET @ShipCity='München';
SET @ShipRegion=NULL;
SET @ShipPostalCode='80805';
SET @ShipCountry='Brazil';
EXECUTE spSetOrderDetails @CustomerID,@EmployeeID,@OrderDate,@RequiredDate,@ShippedDate,@ShipVia,@Freight,@ShipName,@ShipAddress,@ShipCity,@ShipRegion,@ShipPostalCode,@ShipCountry;
