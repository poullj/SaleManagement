CREATE PROCEDURE [dbo].[spGetDistrict]
	@DistrictId int 
AS
	SELECT [Id], [Name], [PrimarySalesPersonId] FROM [District] WHERE [Id] = @DistrictId

