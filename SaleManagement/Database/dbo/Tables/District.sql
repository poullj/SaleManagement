﻿CREATE TABLE [dbo].[District] (
    [ID]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_District] PRIMARY KEY CLUSTERED ([ID] ASC)
);

