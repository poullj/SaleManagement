CREATE PROCEDURE [dbo].spMakeSalesPersonPrimaryInDistrict
	@SalesPersonID int,
	@DistrictID int 
AS
    UPDATE [dbo].[District] SET PrimarySalesPersonId=@SalesPersonID where [Id] = @DistrictID
	