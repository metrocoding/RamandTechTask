IF NOT EXISTS(SELECT *
              FROM sys.databases
              WHERE name = 'RamandTest')
    BEGIN
        CREATE DATABASE [RamandTest]
    END
GO
USE [RamandTest]
GO
-- CREATE TABLE --------------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[User]
(
    [Id]       UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [UserName] [varchar](20)    NOT NULL,
    [Password] [varchar](200)   NOT NULL,
)

-- ADD USER ------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('add_user_sp') IS NOT NULL
    BEGIN
        DROP PROC add_user_sp
    END
GO

CREATE PROC add_user_sp
--     @Id UNIQUEIDENTIFIER,
    @UserName varchar(20),
    @Password varchar(200)
AS
BEGIN
    INSERT INTO [User]
        (UserName, Password)
    VALUES (@UserName, @Password)
--     SET @Id = NEWID()
END
GO

-- GET USER BY ID ------------------------------------------------------------------------------------------------------
IF OBJECT_ID('get_user_sp') IS NOT NULL
    BEGIN
        DROP PROC get_user_sp
    END
GO

CREATE PROC get_user_sp @Id UNIQUEIDENTIFIER
AS
BEGIN
    SELECT Id, UserName
    FROM [User]
    WHERE (Id = @Id)
END

GO

-- GET USER BY UserName Password -------------------------------------------------------------------------------------
IF OBJECT_ID('get_user_with_credentials_sp') IS NOT NULL
    BEGIN
        DROP PROC get_user_with_credentials_sp
    END
GO

CREATE PROC get_user_with_credentials_sp 
                           @UserName varchar(20),
                           @Password varchar(200)
AS
BEGIN
    SELECT Id, UserName
    FROM [User]
    WHERE Password = @Password AND UserName = @UserName
END
GO

-- GET ALL USERS -------------------------------------------------------------------------------------------------------
IF OBJECT_ID('get_users_sp') IS NOT NULL
    BEGIN
        DROP PROC get_users_sp
    END
GO

CREATE PROC get_users_sp
AS
BEGIN
    SELECT Id, UserName
    FROM [User]
END
GO

-- UPDATE USER ---------------------------------------------------------------------------------------------------------
IF OBJECT_ID('update_user_sp') IS NOT NULL
    BEGIN
        DROP PROC update_user_sp
    END
GO

CREATE PROC update_user_sp @Id UNIQUEIDENTIFIER,
                           @UserName varchar(20),
                           @Password varchar(200)
AS
BEGIN
    UPDATE [User]
    SET UserName = @UserName,
        Password = @Password
    WHERE Id = @Id
END
GO

-- DELETE USER ---------------------------------------------------------------------------------------------------------
IF OBJECT_ID('delete_user_sp') IS NOT NULL
    BEGIN
        DROP PROC delete_user_sp
    END
GO

CREATE PROC delete_user_sp @Id UNIQUEIDENTIFIER
AS
BEGIN
    DELETE
    FROM [User]
    WHERE Id = @Id
END
GO

-- Add Users -----------------------------------------------------------------
INSERT INTO [User]
VALUES      ('3205aec0-00dc-4e6d-b0e7-0428c4f5a7c8',
             'Armin',
             'safepassword');

GO

INSERT INTO [User]
VALUES      ('7e5164e2-c53d-40cc-b93e-4becf8534c2a',
             'Nima',
             'secretpassword');
GO

INSERT INTO [User]
VALUES      ('db1d0800-4898-4c6b-83f5-06672cc57b98',
             'Elias',
             'securepassword');
GO