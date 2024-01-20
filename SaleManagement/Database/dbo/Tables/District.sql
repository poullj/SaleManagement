CREATE TABLE [dbo].[District] (
    [Id]                   INT           IDENTITY (1, 1) NOT NULL,
    [Name]                 NVARCHAR (50) NOT NULL,
    [PrimarySalesPersonId] INT           NOT NULL,
    CONSTRAINT [PK_District] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_District_SalesPerson] FOREIGN KEY ([PrimarySalesPersonId]) REFERENCES [dbo].[SalesPerson] ([Id])
);



