USE [Northwind];

go

sp_rename 'Region', 'Regions';

go

ALTER TABLE [Northwind].[dbo].[Customers]
  ADD [DateOfEstablishment] DATETIME NULL;