CREATE TABLE [dbo].[CartItem] (
    [cartID]   INT        NOT NULL,
    [itemID]   INT        NOT NULL,
    [price]    MONEY NULL,
    [quantity] INT        NULL,  
    CONSTRAINT [FK_CartItem_CartID] FOREIGN KEY ([cartID]) REFERENCES [Cart]([cartID]),
	CONSTRAINT [FK_CartItem_ItemID] FOREIGN KEY ([itemID]) REFERENCES [Item]([itemID]) 
);
