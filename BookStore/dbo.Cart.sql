CREATE TABLE [dbo].[Cart] (
    [cartID]          INT       IDENTITY (1, 1)     NOT NULL,
    [cartUserID]          NVARCHAR (128) NULL,
    [subTotal]        FLOAT (53)     NULL,
    [gst]             FLOAT (53)     NULL,
    [discountPercent] FLOAT (53)     NULL,
    [discountAmount]  MONEY          NULL,
    [deliveryCharge]  MONEY          NULL,
    [collectionDay]   INT            NULL,
    [totalPrice]      MONEY          NULL,
    [dateOfPurchase]  DATETIME       NULL,
    [cUserID]         INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.Cart] PRIMARY KEY CLUSTERED ([cartID] ASC)
);

