IF (SELECT COUNT(ProductID) FROM [dbo].[Products]) = 0
BEGIN
	SET IDENTITY_INSERT [dbo].[Products] ON
	INSERT Products ("ProductID","ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") 
		VALUES (1,'Chai',1,1,'10 boxes x 20 bags',18,39,0,10,0)
	SET IDENTITY_INSERT [dbo].[Products] OFF
END