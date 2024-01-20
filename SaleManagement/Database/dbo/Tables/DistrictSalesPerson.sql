CREATE TABLE [dbo].[DistrictSalesPerson] (
    [SalesPersonId] INT NOT NULL,
    [DistrictId]    INT NOT NULL,
    [Secondary]     BIT NOT NULL,
    CONSTRAINT [FK_DistrictSalesPerson_District] FOREIGN KEY ([SalesPersonId]) REFERENCES [dbo].[District] ([Id]),
    CONSTRAINT [FK_DistrictSalesPerson_SalesPerson] FOREIGN KEY ([SalesPersonId]) REFERENCES [dbo].[SalesPerson] ([Id])
);

