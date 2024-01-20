CREATE PROCEDURE [dbo].[spGetAllDistricts]
AS
	SELECT [ID], [Name], [PrimarySalesPersonID] FROM [District]
	SELECT [ID], [Name] FROM [SalesPerson]
	SELECT [ID], [Name], [DistrictId] FROM [Store]
	SELECT [SalesPersonId], [DistrictId], [Secondary] FROM [DistrictSalesPerson]
