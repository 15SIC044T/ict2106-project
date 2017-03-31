CREATE TABLE [dbo].[Item] (
    [itemID]       INT             IDENTITY (1, 1) NOT NULL,
    [iName]        NCHAR (200)     NULL,
    [iDescription] NCHAR (512)     NULL,
    [iImage]       NCHAR (512)		 NULL,
    [iPrice]       MONEY           NULL,
    [iCategory]    NCHAR (200)     NULL,
    [iQuantity]    INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([itemID] ASC)
);

