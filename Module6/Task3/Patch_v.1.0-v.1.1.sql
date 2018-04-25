-- 3

-- 1.0 -> 1.1

USE Northwind;

IF NOT EXISTS (SELECT * FROM sys.objects 
	WHERE OBJECT_ID = OBJECT_ID(N'dbo.CreditCard') AND TYPE IN (N'U'))
BEGIN

CREATE TABLE CreditCard   
(  
    CreditCardID INT IDENTITY(1,1) NOT NULL, 
	CreditCardNumber NVARCHAR(12) NOT NULL,
    ExpiryDate DATETIME NOT NULL,   
    HolderLastName NVARCHAR(20) NOT NULL,
	HolderFirstName NVARCHAR(10) NOT NULL,
	HolderID INT NULL
	CONSTRAINT FK_CreditCard_Employee FOREIGN KEY(HolderID)
		REFERENCES Employees (EmployeeID)
);  

END;

