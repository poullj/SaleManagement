CREATE PROCEDURE [dbo].[spAddSalesPersonToDistrict]
	@SalesPersonId int,
	@DistrictId int, 
	@Secondary bit

AS

IF EXISTS (SELECT * FROM [DistrictSalesPerson] WHERE [SalesPersonId]=@SalesPersonId and [DistrictId]=@DistrictId)
BEGIN
    UPDATE [dbo].[DistrictSalesPerson] SET [Secondary]=@Secondary WHERE [SalesPersonId]=@SalesPersonId and [DistrictId]=@DistrictId
END
ELSE
BEGIN
	INSERT INTO [dbo].[DistrictSalesPerson] ([SalesPersonID] ,[DistrictId] ,[Secondary])
                                     VALUES (@SalesPersonID, @DistrictId, @Secondary)
END


