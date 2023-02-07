--7. write a SQL query to Create Stored procedure in the Northwind database to update
--Customer Order DetailS


CREATE PROCEDURE spUpdateCustomerById
@CustomerID nchar(5),
@CompanyName nvarchar(40),
@ContactName nvarchar(30),
@ContactTitle nvarchar(30),
@Address nvarchar(60),
@City nvarchar(15),
@Region nvarchar(15),
@PostalCode nvarchar(10),
@Country nvarchar(15),
@Phone nvarchar(24),
@Fax nvarchar(24)
AS
BEGIN	
	UPDATE Customers SET 
	CompanyName=@CompanyName,
	ContactName=@ContactName,
	ContactTitle=@ContactTitle,
	Address=@Address,
	City= @City,
	Region=@Region,
	PostalCode=@PostalCode,
	Country=@Country,
	Phone=@Phone,
	Fax=@Fax
	WHERE CustomerID=@CustomerID;
END

DROP PROCEDURE spUpdateCustomerById;

DECLARE @CustomerID nchar(5);
DECLARE @CompanyName nvarchar(40);
DECLARE @ContactName nvarchar(30);
DECLARE @ContactTitle nvarchar(30);
DECLARE @Address nvarchar(60);
DECLARE @City nvarchar(15);
DECLARE @Region nvarchar(15);
DECLARE @PostalCode nvarchar(10);
DECLARE @Country nvarchar(15);
DECLARE @Phone nvarchar(24);
DECLARE @Fax nvarchar(24);

SET @CustomerID='CHOPS';
SET @CompanyName='Shinchen';
SET @ContactName='Siro';
SET @ContactTitle='Owner';
SET @Address='Furfuri nagar';
SET @City='Kasukabe';
SET @Region='BC';
SET @PostalCode='00123';
SET @Country='Japan';
SET @Phone='(5)911-8971';
SET @Fax='(5)911-8972';

EXECUTE spUpdateCustomerById @CustomerID,@CompanyName,@ContactName,@ContactTitle,@Address,@City,@Region,@PostalCode,@Country,@Phone,@Fax

SELECT * FROM CUSTOMERS WHERE CustomerID='CHOPS';
