USE [master]
GO
IF (SERVERPROPERTY('Edition') <> 'SQL Azure')
BEGIN
    EXEC sp_configure 'clr enabled', 1;
    EXEC sp_configure 'show advanced options',1
    EXEC sp_configure 'contained database authentication', 1
    EXEC sp_executesql N'RECONFIGURE WITH OVERRIDE'
END
GO

IF NOT EXISTS(SELECT * FROM sys.sysdatabases where name='$(DatabaseName)')
BEGIN
    PRINT N'Creating $(DatabaseName) ...';
    EXEC sp_executesql N'CREATE DATABASE [$(DatabaseName)] CONTAINMENT = PARTIAL COLLATE Latin1_General_100_CI_AS_KS_WS_SC ;'

    ALTER DATABASE [$(DatabaseName)] SET TRUSTWORTHY ON;
END
GO

USE [$DatabaseName$]
GO

IF NOT EXISTS ( SELECT * FROM sys.schemas WHERE name = N'metadata' ) 
    EXEC('CREATE SCHEMA [metadata] AUTHORIZATION [dbo]');
GO
