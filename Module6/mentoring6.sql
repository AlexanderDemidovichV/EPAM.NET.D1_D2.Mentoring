--1.1.1

SELECT OrderID, ShippedDate, ShipVia 
FROM Orders
WHERE ShippedDate >= '05-06-1998';

--1.1.2

SELECT 
	OrderID, 
	CASE
		WHEN ShippedDate IS NULL
		THEN 'Not Shipped'
		ELSE CAST(Orders.ShippedDate AS NVARCHAR)
	END AS ShippedDate
FROM Orders
WHERE ShippedDate IS NULL;

--1.1.3

SELECT 
	OrderID AS [Order Number], 
	CASE
		WHEN Orders.ShippedDate IS NULL
		THEN 'Not Shipped'
		ELSE CAST(CONVERT(VARCHAR(8), Orders.ShippedDate, 101) AS NVARCHAR)
	END AS [Shipped Date]
FROM Orders
WHERE 
	ShippedDate >= '05-07-1998' OR
	ShippedDate IS NULL;


--1.2.1

SELECT 
	CompanyName,
	Country
FROM Customers
WHERE Country IN ('USA', 'Canada')
ORDER BY 
	CompanyName,
	Country;

--1.2.2

SELECT
	CompanyName,
	Country
FROM Customers
WHERE
	Country NOT IN ('USA', 'Canada')
ORDER BY CompanyName;

--1.2.3

SELECT DISTINCT
	Country
FROM Customers
ORDER BY Country DESC;

--1.3.1

SELECT DISTINCT
	OrderID
FROM [Order Details]
WHERE Quantity BETWEEN 3 AND 10;

--1.3.2

SELECT 
	CustomerID,
	Country
FROM Customers
WHERE
	LEFT(Country, 1) BETWEEN 'b' AND 'g'
ORDER BY Country;

--1.3.3

SELECT 
	CustomerID,
	Country
FROM Customers
WHERE
	LEFT(Country, 1) >= 'b' AND 
	LEFT(Country, 1) <= 'g'
ORDER BY Country;

--1.4

SELECT 
	ProductName
FROM Products
WHERE 
	ProductName LIKE '%cho%olade%';

--2.1.1

SELECT
	SUM(O.UnitPrice * O.Quantity * O.Discount) AS Totals
FROM [Order Details] AS O;

--2.1.2

SELECT 
	COUNT(CASE WHEN ShippedDate IS NULL THEN 1 END) 
FROM Orders;

--2.1.3

SELECT 
	COUNT(DISTINCT CustomerID)
FROM Orders;

--2.2.1

SELECT
	YEAR(OrderDate) AS [Year],
	COUNT(YEAR(OrderDate))
FROM Orders
GROUP BY YEAR(OrderDate);

SELECT COUNT(*) FROM Orders;

--2.2.2

SELECT 
	(Employees.LastName + ', ' + Employees.FirstName) AS Seller,  
	COUNT(Employees.LastName + ', ' + Employees.FirstName) AS Amount
FROM Orders
LEFT JOIN Employees ON Orders.EmployeeID = Employees.EmployeeID
GROUP BY (Employees.LastName + ', ' + Employees.FirstName)
ORDER BY Amount DESC;

--2.2.3
--	По таблице Orders найти количество заказов, сделанных каждым продавцом и для каждого покупателя. 
--  Необходимо определить это только для заказов, сделанных в 1998 году. 

SELECT 
	 EmployeeID,
	 CustomerID,
	 COUNT(CustomerID) AS [Amount of purchases]
FROM Orders
GROUP BY 
	 EmployeeID,
	 CustomerID,
	 OrderDate
HAVING 
	Orders.OrderDate >= '01-01-1998' AND Orders.OrderDate < '01-01-1999';

-- 2.2.4
-- Найти покупателей и продавцов, которые живут в одном городе. 
-- Если в городе живут только один или несколько продавцов, или только один или несколько покупателей, 
-- то информация о таких покупателя и продавцах не должна попадать в результирующий набор. Не использовать конструкцию JOIN. 
--
SELECT
	C.CompanyName,
	(E.LastName + ', ' + E.FirstName) AS EmployeeName,
	C.City	
FROM Customers AS C, Employees AS E 
GROUP BY 
	C.CompanyName,
	(E.LastName + ', ' + E.FirstName),
	C.City,
	E.City
HAVING C.City = E.City; 

-- 2.2.5
-- Найти всех покупателей, которые живут в одном городе. 

SELECT 
	City,
	COUNT(CompanyName) AS [Amount of customers]
FROM Customers
GROUP BY 
	city
ORDER BY COUNT(CompanyName) DESC;

-- 2.2.6
-- По таблице Employees найти для каждого продавца его руководителя.

SELECT 
	E.ReportsTo AS ReportsTo
FROM Employees AS E
GROUP BY ReportsTo
HAVING ReportsTo IS NOT NULL;

-- 2.3.1
-- Определить продавцов, которые обслуживают регион 'Western' (таблица Region). 

SELECT
	Employees.FirstName,
	Employees.LastName,
	Region.RegionDescription
FROM Employees
INNER JOIN EmployeeTerritories ON EmployeeTerritories.EmployeeID = Employees.EmployeeID
INNER JOIN Territories ON Territories.TerritoryID = EmployeeTerritories.TerritoryID 
INNER JOIN Region ON Territories.RegionID = Region.RegionID AND RegionDescription = 'Western'
GROUP BY
	Employees.FirstName,
	Employees.LastName,
	Region.RegionDescription;

-- 2.3.2

-- Выдать в результатах запроса имена всех заказчиков из таблицы Customers и суммарное количество их заказов из таблицы Orders. 
-- Принять во внимание, что у некоторых заказчиков нет заказов, но они также должны быть выведены в результатах запроса. Упорядочить 
-- результаты запроса по возрастанию количества заказов.

SELECT
	Customers.CompanyName,
	COUNT(Orders.OrderID) AS [Amount of orders]
FROM Customers
LEFT JOIN Orders ON Orders.CustomerID = Customers.CustomerID
GROUP BY 
	Customers.CompanyName
ORDER BY COUNT(Orders.OrderID);

-- 2.4.1

-- Выдать всех поставщиков (колонка CompanyName в таблице Suppliers), 
-- у которых нет хотя бы одного продукта на складе (UnitsInStock в таблице Products равно 0). 
-- Использовать вложенный SELECT для этого запроса с использованием оператора IN. 

SELECT 
	CompanyName
FROM Suppliers
WHERE SupplierID IN 
	(SELECT 
		SupplierID
	FROM Products
	WHERE UnitsInStock = 0);

-- 2.4.2

--  Выдать всех продавцов, которые имеют более 150 заказов. Использовать вложенный SELECT.

SELECT 
	(FirstName + ', ' + LastName) AS [First Name, Last Name]
FROM Employees
WHERE EmployeeID IN 
	(SELECT 
		EmployeeID
	FROM Orders
	GROUP BY EmployeeID
	HAVING COUNT(EmployeeID) > 150);

-- 2.4.3

-- Выдать всех заказчиков (таблица Customers), 
-- которые не имеют ни одного заказа (подзапрос по таблице Orders). 
-- Использовать оператор EXISTS.

SELECT 
	Customers.CompanyName
FROM Customers
WHERE EXISTS
	(SELECT
		*
	FROM Orders
	WHERE Orders.CustomerID = Customers.CustomerID);

-- 3

-- 1.0 -> 1.1

CREATE TABLE CreditCard   
(  
    CreditCardID INT IDENTITY(1,1) NOT NULL, 
	CreditCardNumber NVARCHAR(12) NOT NULL,
    ExpiryDate DATETIME NOT NULL,   
    HolderLastName NVARCHAR(20) NOT NULL,
	HolderFirstName NVARCHAR(10) NOT NULL,
	HolderID INT NULL
);  

ALTER TABLE CreditCard  WITH NOCHECK ADD  CONSTRAINT FK_CreditCard_Employee FOREIGN KEY(HolderID)
REFERENCES Employees (EmployeeID);
GO

-- 1.1 -> 1.3

sp_rename 'Region', 'Regions';

ALTER TABLE Customers
  ADD DateOfEstablishment DATETIME;
