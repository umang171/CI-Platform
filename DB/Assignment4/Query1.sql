--1. Create a stored procedure in the Northwind database that will calculate the average
--value of Freight for a specified customer.Then, a business rule will be added that will
--be triggered before every Update and Insert command in the Orders controller,and
--will use the stored procedure to verify that the Freight does not exceed the average
--freight. If it does, a message will be displayed and the command will be cancelled.

SELECT * FROM CUSTOMERS;
SELECT * FROM Orders ;
SELECT AVG(Freight) FROM ORDERS WHERE customerId='ALFKI';

CREATE PROCEDURE spGetAvgFridge
@id nchar(5),
@AvgFridge float out
AS
BEGIN
	SELECT @AvgFridge=AVG(Freight) FROM ORDERS WHERE customerId=@id;
END

DECLARE @AvgFridge float;
DECLARE @id nchar(5);
SET @id='ALFKI';
EXECUTE spGetAvgFridge @id,@AvgFridge out;
print @AvgFridge;
