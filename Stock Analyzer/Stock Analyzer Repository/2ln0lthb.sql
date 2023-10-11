IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [CompanyDataModels] (
    [Name] nvarchar(450) NOT NULL,
    [Id] uniqueidentifier NOT NULL,
    [Series] nvarchar(5) NOT NULL,
    CONSTRAINT [PK_CompanyDataModels] PRIMARY KEY ([Name])
);
GO

CREATE TABLE [BhavCopyInfoDataModel] (
    [Id] uniqueidentifier NOT NULL,
    [Date] datetime2 NOT NULL,
    [CompanyName] nvarchar(max) NOT NULL,
    [CompanyDataName] nvarchar(450) NOT NULL,
    [PreviousClose] float NOT NULL,
    [OpenPrice] float NOT NULL,
    [HighPrice] float NOT NULL,
    [LowPrice] float NOT NULL,
    [LastPrice] float NOT NULL,
    [ClosePrice] float NOT NULL,
    [AvgPrice] float NOT NULL,
    [TtlTrdQnty] float NOT NULL,
    [TurnOverLacs] float NOT NULL,
    [NoOfTrades] float NOT NULL,
    [DeliveryQty] float NOT NULL,
    [DeliveryPercentage] float NOT NULL,
    CONSTRAINT [PK_BhavCopyInfoDataModel] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BhavCopyInfoDataModel_CompanyDataModels_CompanyDataName] FOREIGN KEY ([CompanyDataName]) REFERENCES [CompanyDataModels] ([Name]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_BhavCopyInfoDataModel_CompanyDataName] ON [BhavCopyInfoDataModel] ([CompanyDataName]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230830150137_Database-Creation', N'7.0.10');
GO

COMMIT;
GO

