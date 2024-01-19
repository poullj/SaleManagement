CREATE TABLE [dbo].[Store] (
    [ID]         INT           IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (50) NOT NULL,
    [DistrictID] INT           NOT NULL,
    CONSTRAINT [PK_Store] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Store_District] FOREIGN KEY ([DistrictID]) REFERENCES [dbo].[District] ([ID])
);

