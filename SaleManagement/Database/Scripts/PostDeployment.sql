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

DBCC CHECKIdENT ('[Store]', RESEED, 1);
DBCC CHECKIdENT ('[District]', RESEED, 1);
DBCC CHECKIdENT ('[SalesPerson]', RESEED, 1);

INSERT INTO [dbo].[SalesPerson] ([Name]) VALUES ('Joe')
INSERT INTO [dbo].[SalesPerson] ([Name]) VALUES ('Lucy')
INSERT INTO [dbo].[SalesPerson] ([Name]) VALUES ('Jack')
INSERT INTO [dbo].[SalesPerson] ([Name]) VALUES ('Ellen')
GO

INSERT INTO [dbo].[District] ([Name],[PrimarySalesPersonId]) VALUES ('Distict1',1)
INSERT INTO [dbo].[District] ([Name],[PrimarySalesPersonId]) VALUES ('Distict2',2)
INSERT INTO [dbo].[District] ([Name],[PrimarySalesPersonId]) VALUES ('Distict3',4)
INSERT INTO [dbo].[District] ([Name],[PrimarySalesPersonId]) VALUES ('Distict4',4)
GO

INSERT INTO [dbo].[Store] ([Name],[DistrictId]) VALUES ('Store11',1)
INSERT INTO [dbo].[Store] ([Name],[DistrictId]) VALUES ('Store12',1)
INSERT INTO [dbo].[Store] ([Name],[DistrictId]) VALUES ('Store21',2)
INSERT INTO [dbo].[Store] ([Name],[DistrictId]) VALUES ('Store31',3)
INSERT INTO [dbo].[Store] ([Name],[DistrictId]) VALUES ('Store41',4)
INSERT INTO [dbo].[Store] ([Name],[DistrictId]) VALUES ('Store42',4)

GO
INSERT INTO [dbo].[DistrictSalesPerson] ([SalesPersonId], [DistrictId] ,[Secondary]) VALUES(1, 1, 0)
INSERT INTO [dbo].[DistrictSalesPerson] ([SalesPersonId], [DistrictId] ,[Secondary]) VALUES(2, 2, 0)
INSERT INTO [dbo].[DistrictSalesPerson] ([SalesPersonId], [DistrictId] ,[Secondary]) VALUES(3, 3, 0)
INSERT INTO [dbo].[DistrictSalesPerson] ([SalesPersonId], [DistrictId] ,[Secondary]) VALUES(4, 1, 1)
INSERT INTO [dbo].[DistrictSalesPerson] ([SalesPersonId], [DistrictId] ,[Secondary]) VALUES(4, 2, 1)
INSERT INTO [dbo].[DistrictSalesPerson] ([SalesPersonId], [DistrictId] ,[Secondary]) VALUES(4, 3, 1)
INSERT INTO [dbo].[DistrictSalesPerson] ([SalesPersonId], [DistrictId] ,[Secondary]) VALUES(4, 4, 0)
GO


Declare @MachineName varchar(20) = convert(varchar, SERVERPROPERTY('MachineName'))
Declare @DatabaseName varchar(100) = DB_NAME()
Declare @PathFinal varchar(max)

set @PathFinal = 'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA'

declare @SnapshotSql nvarchar(max) = 'CREATE DATABASE ' + @DatabaseName + '_snapshot ' + 'ON '
+'(NAME = '+ @DatabaseName + ', FILENAME = ''' + @PathFinal + '\' + @DatabaseName + '.ss'')'
+ 'AS SNAPSHOT OF ' + @DatabaseName

Set @DatabaseName = @DatabaseName + '_snapshot';
IF EXISTS (SELECT name FROM sys.databases WHERE name = @DatabaseName)
	begin
	Set @SnapshotSql = 'DROP DATABASE ' + @DatabaseName + '; ' + @SnapshotSql
	print @SnapshotSql
	EXEC (@SnapshotSql)
	end
else 
    exec sp_executesql @SnapshotSql;

