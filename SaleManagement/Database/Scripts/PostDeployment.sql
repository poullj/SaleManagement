/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

DELETE FROM [DistrictSalesPerson]
DELETE FROM [Store]
DELETE FROM [District]
DELETE FROM [SalesPerson]

DBCC CHECKIDENT ('[Store]', RESEED, 0);
DBCC CHECKIDENT ('[District]', RESEED, 0);
DBCC CHECKIDENT ('[SalesPerson]', RESEED, 0);

INSERT INTO [dbo].[SalesPerson] ([Name]) VALUES ('Joe')
INSERT INTO [dbo].[SalesPerson] ([Name]) VALUES ('Lucy')
INSERT INTO [dbo].[SalesPerson] ([Name]) VALUES ('Jack')
INSERT INTO [dbo].[SalesPerson] ([Name]) VALUES ('Ellen')
GO

INSERT INTO [dbo].[District] ([Name],[PrimarySalesPersonID]) VALUES ('Distict1',1)
INSERT INTO [dbo].[District] ([Name],[PrimarySalesPersonID]) VALUES ('Distict2',2)
INSERT INTO [dbo].[District] ([Name],[PrimarySalesPersonID]) VALUES ('Distict3',4)
INSERT INTO [dbo].[District] ([Name],[PrimarySalesPersonID]) VALUES ('Distict4',4)
GO

INSERT INTO [dbo].[Store] ([Name],[DistrictID]) VALUES ('Store11',1)
INSERT INTO [dbo].[Store] ([Name],[DistrictID]) VALUES ('Store12',1)
INSERT INTO [dbo].[Store] ([Name],[DistrictID]) VALUES ('Store21',2)
INSERT INTO [dbo].[Store] ([Name],[DistrictID]) VALUES ('Store31',3)
INSERT INTO [dbo].[Store] ([Name],[DistrictID]) VALUES ('Store41',4)
INSERT INTO [dbo].[Store] ([Name],[DistrictID]) VALUES ('Store42',4)

GO
INSERT INTO [dbo].[DistrictSalesPerson] ([SalesPersonID], [DistrictID] ,[Secondary]) VALUES(1, 1, 0)
INSERT INTO [dbo].[DistrictSalesPerson] ([SalesPersonID], [DistrictID] ,[Secondary]) VALUES(2, 2, 0)
INSERT INTO [dbo].[DistrictSalesPerson] ([SalesPersonID], [DistrictID] ,[Secondary]) VALUES(3, 3, 0)
INSERT INTO [dbo].[DistrictSalesPerson] ([SalesPersonID], [DistrictID] ,[Secondary]) VALUES(4, 1, 0)
INSERT INTO [dbo].[DistrictSalesPerson] ([SalesPersonID], [DistrictID] ,[Secondary]) VALUES(4, 2, 0)
INSERT INTO [dbo].[DistrictSalesPerson] ([SalesPersonID], [DistrictID] ,[Secondary]) VALUES(4, 3, 0)
INSERT INTO [dbo].[DistrictSalesPerson] ([SalesPersonID], [DistrictID] ,[Secondary]) VALUES(4, 4, 0)
GO



