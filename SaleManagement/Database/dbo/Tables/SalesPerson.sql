CREATE TABLE [dbo].[SalesPerson] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_SalesPerson] PRIMARY KEY CLUSTERED ([Id] ASC)
);

