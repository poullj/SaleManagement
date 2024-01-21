CREATE PROCEDURE [dbo].[spGetAllDistricts]
AS
	SELECT [Id], [Name], [PrimarySalesPersonId] FROM [District]
	SELECT [Id], [Name] FROM [SalesPerson]
	SELECT [Id], [Name], [DistrictId] FROM [Store]
	SELECT [SalesPersonId], [DistrictId], [Secondary] FROM [DistrictSalesPerson]
