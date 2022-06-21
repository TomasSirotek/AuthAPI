CREATE TABLE [dbo].[product]
(
    [id] NVARCHAR(255)  NOT NULL,
    [title] NVARCHAR(255) NOT NULL,
    [description] NVARCHAR(255) NOT NULL,
    [isActive] BIGINT NOT NULL,
    [unitAmount] INT NOT NULL,
    [unitPrice] DECIMAL NOT NULL,
    -- Specify keys
    CONSTRAINT product_pkey PRIMARY KEY (id),
    
);

CREATE TABLE [dbo].[category]
(
    [id] NVARCHAR(255)  NOT NULL ,
    [name] NVARCHAR(255) NOT NULL,
    -- Specify keys
    CONSTRAINT category_pkey PRIMARY KEY (id),
    CONSTRAINT category_ukey UNIQUE (name),
    
);

CREATE TABLE [dbo].[product_category]
(
    [productId] NVARCHAR(255)  NOT NULL,
    [categoryId] NVARCHAR(255) NOT NULL,
    -- Specify keys
    CONSTRAINT productCategory_pkey PRIMARY KEY(productId,categoryId),
    CONSTRAINT fk_productCategory__Product 
        FOREIGN KEY (productId) REFERENCES product(id) ON DELETE CASCADE ,

    CONSTRAINT fk_productCategory__Category
        FOREIGN KEY (categoryId) REFERENCES category(id) ON DELETE CASCADE,
);