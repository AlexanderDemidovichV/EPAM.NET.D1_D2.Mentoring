IF (SELECT COUNT(SupplierID) FROM [dbo].[Suppliers]) = 0
BEGIN
	SET IDENTITY_INSERT [dbo].[Suppliers] ON
	INSERT Suppliers ("SupplierID","CompanyName","ContactName","ContactTitle","Address","City","Region","PostalCode","Country","Phone","Fax","HomePage") 
		VALUES(1,'Exotic Liquids','Charlotte Cooper','Purchasing Manager','49 Gilbert St.','London',NULL,'EC1 4SD','UK','(171) 555-2222',NULL,NULL)
	SET IDENTITY_INSERT [dbo].[Suppliers] OFF
END