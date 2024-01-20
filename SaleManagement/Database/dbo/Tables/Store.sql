CREATE TABLE [dbo].[Store] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (50) NOT NULL,
    [DistrictId] INT           NOT NULL,
    CONSTRAINT [PK_Store] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Store_District] FOREIGN KEY ([DistrictId]) REFERENCES [dbo].[District] ([Id])
);



