CREATE PROCEDURE [dbo].[spRemoveSalesPersonFromDistrict]
	@SalesPersonID int,
	@DistrictID int 
AS
	DELETE FROM [dbo].[DistrictSalesPerson] where [SalesPersonID] = @SalesPersonID and [DistrictID] = @DistrictID
