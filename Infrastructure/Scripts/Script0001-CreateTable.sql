CREATE TABLE [dbo].[test]
(
    [id] NVARCHAR(255)  NOT NULL PRIMARY KEY,
    [title] NVARCHAR(255) NOT NULL,
    [description] NVARCHAR(255) NOT NULL,
    [isActive] BIGINT NOT NULL,
    [unitAmount] INT NOT NULL,
    [unitPrice] DECIMAL NOT NULL,
)
