CREATE PROCEDURE [dbo].[spAddSalesPersonToDistrict]
	@SalesPersonID int,
	@DistrictID int, 
	@Secondary bit

AS

IF EXISTS (SELECT * FROM [DistrictSalesPerson] WHERE [SalesPersonID]=@SalesPersonID and [DistrictID]=@DistrictID)
BEGIN
    UPDATE [dbo].[DistrictSalesPerson] SET [Secondary]=@Secondary WHERE [SalesPersonID]=@SalesPersonID and [DistrictID]=@DistrictID
END
ELSE
BEGIN
	INSERT INTO [dbo].[DistrictSalesPerson] ([SalesPersonID] ,[DistrictID] ,[Secondary])
                                     VALUES (@SalesPersonID, @DistrictID, @Secondary)
END


