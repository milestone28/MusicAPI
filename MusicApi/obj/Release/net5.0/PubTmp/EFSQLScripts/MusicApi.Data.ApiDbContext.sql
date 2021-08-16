﻿IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210815092138_initial-create')
BEGIN
    CREATE TABLE [Artists] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Gender] nvarchar(max) NOT NULL,
        [ImageUrl] nvarchar(max) NULL,
        CONSTRAINT [PK_Artists] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210815092138_initial-create')
BEGIN
    CREATE TABLE [Albums] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [ImageUrl] nvarchar(max) NULL,
        [ArtistId] int NOT NULL,
        CONSTRAINT [PK_Albums] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Albums_Artists_ArtistId] FOREIGN KEY ([ArtistId]) REFERENCES [Artists] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210815092138_initial-create')
BEGIN
    CREATE TABLE [Songs] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(max) NOT NULL,
        [Duration] nvarchar(max) NOT NULL,
        [UploadedDate] datetime2 NOT NULL,
        [IsFeatured] bit NOT NULL,
        [ImageUrl] nvarchar(max) NULL,
        [AudioUrl] nvarchar(max) NULL,
        [ArtistId] int NOT NULL,
        [AlbumId] int NULL,
        CONSTRAINT [PK_Songs] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Songs_Albums_AlbumId] FOREIGN KEY ([AlbumId]) REFERENCES [Albums] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Songs_Artists_ArtistId] FOREIGN KEY ([ArtistId]) REFERENCES [Artists] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210815092138_initial-create')
BEGIN
    CREATE INDEX [IX_Albums_ArtistId] ON [Albums] ([ArtistId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210815092138_initial-create')
BEGIN
    CREATE INDEX [IX_Songs_AlbumId] ON [Songs] ([AlbumId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210815092138_initial-create')
BEGIN
    CREATE INDEX [IX_Songs_ArtistId] ON [Songs] ([ArtistId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210815092138_initial-create')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210815092138_initial-create', N'5.0.9');
END;
GO

COMMIT;
GO

