CREATE PROCEDURE [dbo].[spAddSalesPersonToDistrict]
	@SalesPersonID int,
	@DistrictID int, 
	@Secondary bit

AS
	INSERT INTO [dbo].[DistrictSalesPerson] ([SalesPersonID] ,[DistrictID] ,[Secondary])
                                     VALUES (@SalesPersonID, @DistrictID, @Secondary)
