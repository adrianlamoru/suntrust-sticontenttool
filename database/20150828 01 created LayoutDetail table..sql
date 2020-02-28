USE [st1001]
GO

CREATE TABLE [dbo].[LayoutDetail] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [OfferID]   INT           NOT NULL,
    [ContentID]   NVARCHAR (14) NOT NULL,
    [Note]   NVARCHAR(MAX)      NOT NULL,
    CONSTRAINT [PK_LayoutDetail] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_LayoutDetail_Offer] FOREIGN KEY ([OfferID]) REFERENCES [dbo].[Offer] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);