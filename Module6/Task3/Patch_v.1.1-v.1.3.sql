-- 3

-- 1.1 -> 1.3

USE Northwind;

IF NOT EXISTS (SELECT * FROM sys.objects 
	WHERE OBJECT_ID = OBJECT_ID(N'dbo.CreditCard') AND TYPE IN (N'U'))
BEGIN

EXECUTE sp_rename N'Region', N'Regions', 'OBJECT';

END;

USE Northwind;

IF NOT EXISTS (SELECT * FROM sys.objects 
	WHERE OBJECT_ID = OBJECT_ID(N'dbo.CreditCard') AND TYPE IN (N'U'))
BEGIN

ALTER TABLE Customers
  ADD DateOfEstablishment DATETIME;

END;