-- Creates user table
CREATE TABLE [dbo].[app_user]
(
    [id] NVARCHAR(255)  NOT NULL ,
    [firstName] NVARCHAR(255) NOT NULL,
    [lastName] NVARCHAR(255) NOT NULL,
    [email] NVARCHAR(255) NOT NULL,
    [passwordHash] NVARCHAR(255) NOT NULL,
    [isActivated] BIGINT NOT NULL ,
    [createdAt] DATETIME NOT NULL,
    [updatedAt] DATETIME 
        -- Specify keys
        CONSTRAINT user_pkey UNIQUE PRIMARY KEY (id),
    CONSTRAINT user_ukey UNIQUE (email),

);

CREATE TABLE [dbo].[role]
(
    [id] NVARCHAR(255)  NOT NULL ,
    [name] NVARCHAR(255) NOT NULL
    -- Specify keys
    CONSTRAINT role_pkey UNIQUE PRIMARY KEY (id),
    CONSTRAINT role_ukey UNIQUE (name),

);

CREATE TABLE [dbo].[user_role]
(
    [userId] NVARCHAR(255)  NOT NULL,
    [roleId] NVARCHAR(255) NOT NULL
    -- Specify keys
    CONSTRAINT userRole_pkey UNIQUE PRIMARY KEY(userId,roleId),
    CONSTRAINT fk_userRole__User
        FOREIGN KEY (userId) REFERENCES app_user(id) ON DELETE CASCADE ,

    CONSTRAINT fk_userRole__Role
        FOREIGN KEY (roleId) REFERENCES role(id) ON DELETE CASCADE,
);

CREATE TABLE [dbo].[product]
(
    [id] NVARCHAR(255)  NOT NULL,
    [title] NVARCHAR(255) NOT NULL,
    [description] NVARCHAR(255) NOT NULL,
    [isActive] BIGINT NOT NULL,
    [unitPrice] DECIMAL NOT NULL,
    [unitsInStock] INT NOT NULL
    -- Specify keys
    CONSTRAINT product_pkey UNIQUE PRIMARY KEY (id),
    
);

CREATE TABLE [dbo].[email_token]
(
    [id] NVARCHAR(255)  NOT NULL UNIQUE,
    [userId] NVARCHAR(255) NOT NULL,
    [token] NVARCHAR(255) NOT NULL,
    [createdAt] DATETIME NOT NULL,
    [isUsed] BIGINT NOT NULL DEFAULT 0
    -- Specify keys
    CONSTRAINT email_token_pkey PRIMARY KEY (id),
    
    CONSTRAINT uk_userEmailToken__User
        FOREIGN KEY (userId) REFERENCES app_user(id)

);


