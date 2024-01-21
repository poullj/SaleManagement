CREATE PROCEDURE [dbo].[spRemoveSalesPersonFromDistrict]
	@SalesPersonId int,
	@DistrictId int 
AS
	DELETE FROM [dbo].[DistrictSalesPerson] where [SalesPersonId] = @SalesPersonId and [DistrictId] = @DistrictId
