CREATE TABLE [dbo].[DistrictSalesPerson] (
    [SalesPersonID] INT NOT NULL,
    [DistrictID]    INT NOT NULL,
    [Secondary]     BIT NOT NULL,
    CONSTRAINT [FK_DistrictSalesPerson_District] FOREIGN KEY ([SalesPersonID]) REFERENCES [dbo].[District] ([ID]),
    CONSTRAINT [FK_DistrictSalesPerson_SalesPerson] FOREIGN KEY ([SalesPersonID]) REFERENCES [dbo].[SalesPerson] ([ID])
);

