/* Drop all non-system stored procs */
DECLARE @name VARCHAR(128)
DECLARE @SQL NVARCHAR(254)

SELECT @name = 
	(SELECT TOP(1) QUOTENAME(OBJECT_SCHEMA_NAME([object_id])) + '.' + QUOTENAME([name]) AS fqName 
	FROM sys.procedures WHERE is_ms_shipped = 0
	ORDER BY [name])

WHILE @name is not null
BEGIN
    SELECT @SQL = N'DROP PROCEDURE ' + RTRIM(@name)
    EXEC sp_executesql @stmt = @SQL
    RAISERROR ('Dropped Procedure: %s', 0, 1, @name) WITH NOWAIT 
	SELECT @name = 
		(SELECT TOP(1) QUOTENAME(OBJECT_SCHEMA_NAME([object_id])) + '.' + QUOTENAME([name]) AS fqName 
		FROM sys.procedures WHERE is_ms_shipped = 0
		ORDER BY [name])
END
GO

/* Drop all views */
DECLARE @name NVARCHAR(128)
DECLARE @SQL NVARCHAR(254)

SELECT @name = 
	(SELECT TOP(1) QUOTENAME(OBJECT_SCHEMA_NAME([object_id])) + '.' + QUOTENAME([name]) AS fqName 
	FROM sys.views WHERE is_ms_shipped = 0
	ORDER BY [name])

WHILE @name IS NOT NULL
BEGIN
    SELECT @SQL = N'DROP VIEW ' + RTRIM(@name)
    EXEC sp_executesql @stmt = @SQL
    RAISERROR ('Dropped View: %s', 0, 1, @name) WITH NOWAIT 
    SELECT @name = 
		(SELECT TOP(1) QUOTENAME(OBJECT_SCHEMA_NAME([object_id])) + '.' + QUOTENAME([name]) AS fqName 
		FROM sys.views WHERE is_ms_shipped = 0
		ORDER BY [name])
END
GO

/* Drop all functions */
DECLARE @name VARCHAR(128)
DECLARE @SQL NVARCHAR(254)

SELECT @name = 
	(SELECT TOP(1) QUOTENAME(ROUTINE_SCHEMA) + '.' + QUOTENAME(ROUTINE_NAME) AS fqName 
	FROM information_schema.routines 
	WHERE routine_type = 'function'
	ORDER BY ROUTINE_SCHEMA, ROUTINE_NAME)

WHILE @name IS NOT NULL
BEGIN
    SELECT @SQL = N'DROP FUNCTION ' + RTRIM(@name)
    EXEC sp_executesql @stmt = @SQL
    RAISERROR ('Dropped Function: %s', 0, 1, @name) WITH NOWAIT 
    SELECT @name = 
		(SELECT TOP(1) QUOTENAME(ROUTINE_SCHEMA) + '.' + QUOTENAME(ROUTINE_NAME) AS fqName 
		FROM information_schema.routines 
		WHERE routine_type = 'function'
		ORDER BY ROUTINE_SCHEMA, ROUTINE_NAME)
END
GO

/* Turn off system versioning */
DECLARE @name VARCHAR(128)
DECLARE @SQL NVARCHAR(254)

SELECT @name =
	(SELECT TOP(1) QUOTENAME(OBJECT_SCHEMA_NAME([object_id])) + '.' + QUOTENAME([name]) AS fqName 
	FROM sys.tables 
	WHERE [type] = 'U' AND temporal_type = 2 AND is_ms_shipped = 0 
	ORDER BY [name])

WHILE @name IS NOT NULL
BEGIN
	SELECT @SQL = N'ALTER TABLE ' + RTRIM(@name) + ' SET ( SYSTEM_VERSIONING = OFF )'
    EXEC sp_executesql @stmt = @SQL
    RAISERROR ('SYSTEM_VERSIONING OFF: %s', 0, 1, @name) WITH NOWAIT 
    SELECT @name = 
		(SELECT TOP(1) QUOTENAME(OBJECT_SCHEMA_NAME([object_id])) + '.' + QUOTENAME([name]) AS fqName 
		FROM sys.tables 
		WHERE [type] = 'U' AND temporal_type = 2 AND is_ms_shipped = 0 
		ORDER BY [name])
END
GO

/* Drop all indexes */
DECLARE @fqName VARCHAR(128)
DECLARE @constraint VARCHAR(254)
DECLARE @SQL NVARCHAR(254)

SELECT @fqName =
	(SELECT TOP(1) QUOTENAME(SCHEMA_NAME(t.schema_id)) + '.' + QUOTENAME(t.name)
	FROM sys.indexes ix
	JOIN sys.tables AS t ON t.object_id = ix.object_id
	WHERE t.is_ms_shipped = 0
	AND ix.is_primary_key = 0
	AND ix.type > 0
	ORDER BY t.name, SCHEMA_NAME(t.schema_id))
SELECT @constraint =
	(SELECT TOP(1) QUOTENAME(ix.name)
	FROM sys.indexes ix
	JOIN sys.tables AS t ON t.object_id = ix.object_id
	WHERE t.is_ms_shipped = 0
	AND ix.is_primary_key = 0
	AND ix.type > 0
	ORDER BY t.name, SCHEMA_NAME(t.schema_id))

WHILE @constraint is not null
BEGIN
    SELECT @SQL = N'DROP INDEX ' + RTRIM(@constraint) +' ON ' + RTRIM(@fqName)
    EXEC sp_executesql @stmt = @SQL
	RAISERROR ('Dropped Index: %s on %s', 0, 1, @constraint, @fqName) WITH NOWAIT 

	SELECT @fqName =
		(SELECT TOP(1) QUOTENAME(SCHEMA_NAME(t.schema_id)) + '.' + QUOTENAME(t.name)
		FROM sys.indexes ix
		JOIN sys.tables AS t ON t.object_id = ix.object_id
		WHERE t.is_ms_shipped = 0
		AND ix.is_primary_key = 0
		AND ix.type > 0
		ORDER BY t.name, SCHEMA_NAME(t.schema_id))
	SELECT @constraint =
		(SELECT TOP(1) QUOTENAME(ix.name)
		FROM sys.indexes ix
		JOIN sys.tables AS t ON t.object_id = ix.object_id
		WHERE t.is_ms_shipped = 0
		AND ix.is_primary_key = 0
		AND ix.type > 0
		ORDER BY t.name, SCHEMA_NAME(t.schema_id))
END
GO

/* Drop all Foreign Key constraints */
DECLARE @fqName VARCHAR(128)
DECLARE @constraint VARCHAR(254)
DECLARE @SQL NVARCHAR(254)

SELECT @fqName =
	(SELECT TOP(1) QUOTENAME(SCHEMA_NAME(fk.schema_id)) +'.'+ QUOTENAME(OBJECT_NAME(fk.parent_object_id))
	FROM sys.foreign_keys AS fk
	ORDER BY schema_id, parent_object_id)
SELECT @constraint =
	(SELECT TOP(1) QUOTENAME(name)
	FROM sys.foreign_keys AS fk
	ORDER BY schema_id, parent_object_id)

WHILE @constraint is not null
BEGIN
    SELECT @SQL = N'ALTER TABLE ' + RTRIM(@fqName) +' DROP CONSTRAINT ' + RTRIM(@constraint)
    EXEC sp_executesql @stmt = @SQL
	RAISERROR ('Dropped FK Constraint: %s on %s', 0, 1, @constraint, @fqName) WITH NOWAIT 

	SELECT @fqName =
		(SELECT TOP(1) QUOTENAME(SCHEMA_NAME(fk.schema_id)) +'.'+ QUOTENAME(OBJECT_NAME(fk.parent_object_id))
		FROM sys.foreign_keys AS fk
		ORDER BY schema_id, parent_object_id)
	SELECT @constraint =
		(SELECT TOP(1) QUOTENAME(name)
		FROM sys.foreign_keys AS fk
		ORDER BY schema_id, parent_object_id)
END
GO

/* Drop all Primary Key constraints */
DECLARE @fqName VARCHAR(128)
DECLARE @constraint VARCHAR(254)
DECLARE @SQL NVARCHAR(254)

SELECT @fqName =
	(SELECT TOP(1) QUOTENAME(SCHEMA_NAME(t.schema_id)) + '.' + QUOTENAME(t.name)
	FROM sys.indexes ix
	JOIN sys.tables AS t ON t.object_id = ix.object_id
	WHERE 1=1
	AND ix.is_primary_key = 1
	AND ix.type > 0
	ORDER BY t.name, SCHEMA_NAME(t.schema_id))
SELECT @constraint = 
	(SELECT TOP(1) QUOTENAME(ix.name)
	FROM sys.indexes ix
	JOIN sys.tables AS t ON t.object_id = ix.object_id
	WHERE 1=1
	AND ix.is_primary_key = 1
	AND ix.type > 0
	ORDER BY t.name, SCHEMA_NAME(t.schema_id))

WHILE @constraint IS NOT NULL
BEGIN
    SELECT @SQL = N'ALTER TABLE ' + RTRIM(@fqName) +' DROP CONSTRAINT ' + RTRIM(@constraint)
    EXEC sp_executesql @stmt = @SQL
	RAISERROR ('Dropped PK Constraint: %s on %s', 0, 1, @constraint, @fqName) WITH NOWAIT 

	SELECT @fqName =
		(SELECT TOP(1) QUOTENAME(SCHEMA_NAME(t.schema_id)) + '.' + QUOTENAME(t.name)
		FROM sys.indexes ix
		JOIN sys.tables AS t ON t.object_id = ix.object_id
		WHERE 1=1
		AND ix.is_primary_key = 1
		AND ix.type > 0
		ORDER BY t.name, SCHEMA_NAME(t.schema_id))
	SELECT @constraint = 
		(SELECT TOP(1) QUOTENAME(ix.name)
		FROM sys.indexes ix
		JOIN sys.tables AS t ON t.object_id = ix.object_id
		WHERE 1=1
		AND ix.is_primary_key = 1
		AND ix.type > 0
		ORDER BY t.name, SCHEMA_NAME(t.schema_id))
END
GO

/* Drop all type */
DECLARE @name VARCHAR(128)
DECLARE @SQL NVARCHAR(254)

SELECT @name =
	(SELECT TOP(1) QUOTENAME(SCHEMA_NAME([schema_id])) + '.' + QUOTENAME([name]) AS fqName 
	FROM sys.types 
	WHERE is_user_defined = 1
	ORDER BY [name])

WHILE @name IS NOT NULL
BEGIN
    SELECT @SQL = N'DROP TYPE ' + RTRIM(@name)
    EXEC sp_executesql @stmt = @SQL
    RAISERROR ('Dropped Type: %s', 0, 1, @name) WITH NOWAIT 
    SELECT @name = 
		(SELECT TOP(1) QUOTENAME(SCHEMA_NAME([schema_id])) + '.' + QUOTENAME([name]) AS fqName 
		FROM sys.types 
		WHERE is_user_defined = 1
		ORDER BY [name])
END
GO

/* Drop all tables */
DECLARE @name VARCHAR(128)
DECLARE @SQL NVARCHAR(254)

SELECT @name =
	(SELECT TOP(1) QUOTENAME(OBJECT_SCHEMA_NAME([object_id])) + '.' + QUOTENAME([name]) AS fqName 
	FROM sys.tables 
	WHERE [type] = 'U'
	ORDER BY [name])

WHILE @name IS NOT NULL
BEGIN
    SELECT @SQL = N'DROP TABLE ' + RTRIM(@name)
    EXEC sp_executesql @stmt = @SQL
    RAISERROR ('Dropped Table: %s', 0, 1, @name) WITH NOWAIT 
    SELECT @name = 
		(SELECT TOP(1) QUOTENAME(OBJECT_SCHEMA_NAME([object_id])) + '.' + QUOTENAME([name]) AS fqName 
		FROM sys.tables 
		WHERE [type] = 'U'
		ORDER BY [name])
END
GO

/* Drop all assemblies */
DECLARE @name VARCHAR(128)
DECLARE @SQL NVARCHAR(254)

SELECT @name =
	(SELECT TOP(1) QUOTENAME([name]) AS fqName
	FROM sys.assemblies
	WHERE is_user_defined = 1
	ORDER BY [name])

WHILE @name IS NOT NULL
BEGIN
    SELECT @SQL = N'DROP ASSEMBLY ' + RTRIM(@name)
    EXEC sp_executesql @stmt = @SQL
    RAISERROR ('Dropped ASSEMBLY: %s', 0, 1, @name) WITH NOWAIT 
    SELECT @name = 
		(SELECT TOP(1) QUOTENAME([name]) AS fqName
		FROM sys.assemblies
		WHERE is_user_defined = 1
		ORDER BY [name])
END
GO 

/* Drop all user schemas */
DECLARE @name VARCHAR(128)
DECLARE @SQL NVARCHAR(254)

SELECT @name =
	(SELECT TOP(1) QUOTENAME(s.name)
	FROM sys.schemas s
		INNER JOIN sys.database_principals u
			ON u.principal_id = s.principal_id
	WHERE u.is_fixed_role = 0
		  AND u.name NOT IN ( 'sys', 'guest', 'INFORMATION_SCHEMA' )
		  AND s.name <> 'dbo'
	ORDER BY s.name)

WHILE @name IS NOT NULL
BEGIN
    SELECT @SQL = N'DROP SCHEMA ' + RTRIM(@name)
    EXEC sp_executesql @stmt = @SQL
    RAISERROR ('Dropped SCHEMA: %s', 0, 1, @name) WITH NOWAIT 

	SELECT @name =
		(SELECT TOP(1) QUOTENAME(s.name)
		FROM sys.schemas s
			INNER JOIN sys.database_principals u
				ON u.principal_id = s.principal_id
		WHERE u.is_fixed_role = 0
			  AND u.name NOT IN ( 'sys', 'guest', 'INFORMATION_SCHEMA' )
			  AND s.name <> 'dbo'
		ORDER BY s.name)
END
GO 