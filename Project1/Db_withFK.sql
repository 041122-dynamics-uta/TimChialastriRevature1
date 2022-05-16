
CREATE TABLE [UserType] (
    [UserTypeID] int  NOT NULL ,
    [UserTypeName] nvarchar(30)  NOT NULL ,
    CONSTRAINT [PK_UserType] PRIMARY KEY CLUSTERED (
        [UserTypeID] ASC
    )
)

CREATE TABLE [UserAuthentication] (
    [UserID] int  NOT NULL ,
    [FullName] int  NOT NULL ,
    [UserName] nvarchar(30)  NOT NULL ,
    [Password] nvarchar(15)  NOT NULL ,
    [UserTypeID] int  NOT NULL ,
    CONSTRAINT [PK_UserAuthentication] PRIMARY KEY CLUSTERED (
        [UserID] ASC
    )
)

CREATE TABLE [Product] (
    [ProductID] int  NOT NULL ,
    [ProductName] nvarchar(50)  NOT NULL ,
    [ProductDescription] string  NOT NULL ,
    [Price] money  NOT NULL ,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED (
    [ProductID] ASC
    ),
    CONSTRAINT [UK_Product_ProductName] UNIQUE (
        [ProductName]
    )
)

CREATE TABLE [Stock] (
    [StockID] int  NOT NULL ,
    [ProductID] int  NOT NULL ,
    [Qty] int  NOT NULL ,
    [StoreID] nvarchar  NOT NULL ,
    CONSTRAINT [PK_Stock] PRIMARY KEY CLUSTERED (
        [StockID] ASC
    )
)

CREATE TABLE [Locations] (
    [StoreID] int  NOT NULL ,
    [StoreName] nvarchar  NOT NULL ,
    CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED (
        [StoreID] ASC
    )
)

CREATE TABLE [Cart] (
    [CartID] int  NOT NULL ,
    [UserId] int  NOT NULL ,
    [DateCreated] DATETIME2  NOT NULL ,CONSTRAINT 
    [PK_Cart] PRIMARY KEY CLUSTERED (
        [CartID] ASC
    )
)

CREATE TABLE [CartItems] (
    [CartItemID] int  NOT NULL ,
    [CartID] int  NOT NULL ,
    [ProductID] int  NOT NULL ,
    [Qty] int  NOT NULL , CONSTRAINT 
    [PK_CartItems] PRIMARY KEY CLUSTERED (
        [CartItemID] ASC
    )
)

CREATE TABLE [CustomerOrders] (
    [OrderID] int  NOT NULL ,
    [UserID] int  NOT NULL ,
    [DateCreated] DATETIME2  NOT NULL ,
    [CartTotal] money  NOT NULL , CONSTRAINT 
    [PK_CustomerOrders] PRIMARY KEY CLUSTERED (
        [OrderID] ASC
    )
)

CREATE TABLE [CustomerOrderDetails] (
    [OrderDetailsID] int  NOT NULL ,
    [OrderID] int  NOT NULL ,
    [ProductID] int  NOT NULL ,
    [Qty] int  NOT NULL ,
    [Price] money  NOT NULL , CONSTRAINT 
    [PK_CustomerOrderDetails] PRIMARY KEY CLUSTERED (
        [OrderDetailsID] ASC
    )
)

ALTER TABLE [UserType] WITH CHECK ADD CONSTRAINT [FK_UserType_UserTypeID] FOREIGN KEY([UserTypeID])
REFERENCES [UserAuthentication] ([UserTypeID])

ALTER TABLE [UserType] CHECK CONSTRAINT [FK_UserType_UserTypeID]

ALTER TABLE [UserAuthentication] WITH CHECK ADD CONSTRAINT [FK_UserAuthentication_UserID] FOREIGN KEY([UserID])
REFERENCES [Cart] ([UserId])

ALTER TABLE [UserAuthentication] CHECK CONSTRAINT [FK_UserAuthentication_UserID]

ALTER TABLE [Product] WITH CHECK ADD CONSTRAINT [FK_Product_ProductID] FOREIGN KEY([ProductID])
REFERENCES [CustomerOrderDetails] ([ProductID])

ALTER TABLE [Product] CHECK CONSTRAINT [FK_Product_ProductID]

ALTER TABLE [Locations] WITH CHECK ADD CONSTRAINT [FK_Locations_StoreID] FOREIGN KEY([StoreID])
REFERENCES [Stock] ([StoreID])

ALTER TABLE [Locations] CHECK CONSTRAINT [FK_Locations_StoreID]

ALTER TABLE [Cart] WITH CHECK ADD CONSTRAINT [FK_Cart_CartID] FOREIGN KEY([CartID])
REFERENCES [CartItems] ([CartID])

ALTER TABLE [Cart] CHECK CONSTRAINT [FK_Cart_CartID]

ALTER TABLE [Cart] WITH CHECK ADD CONSTRAINT [FK_Cart_UserId] FOREIGN KEY([UserId])
REFERENCES [CustomerOrders] ([UserID])

ALTER TABLE [Cart] CHECK CONSTRAINT [FK_Cart_UserId]

ALTER TABLE [CustomerOrderDetails] WITH CHECK ADD CONSTRAINT [FK_CustomerOrderDetails_OrderID] FOREIGN KEY([OrderID])
REFERENCES [CustomerOrders] ([OrderID])

ALTER TABLE [CustomerOrderDetails] CHECK CONSTRAINT [FK_CustomerOrderDetails_OrderID]

ALTER TABLE [CustomerOrderDetails] WITH CHECK ADD CONSTRAINT [FK_CustomerOrderDetails_ProductID] FOREIGN KEY([ProductID])
REFERENCES [CartItems] ([ProductID])

ALTER TABLE [CustomerOrderDetails] CHECK CONSTRAINT [FK_CustomerOrderDetails_ProductID]

