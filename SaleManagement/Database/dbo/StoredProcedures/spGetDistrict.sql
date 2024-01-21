CREATE PROCEDURE [dbo].[spGetDistrict]
	@DistrictID int 
AS
	SELECT [ID], [Name], [PrimarySalesPersonID] FROM [District] WHERE [ID] = @DistrictID

