﻿using System;
using System.Data;
using System.Data.SqlClient;
using DbUp;
using DbUp.Builder;
using DbUp.Engine.Output;
using DbUp.Engine.Transactions;
using DbFlyup;
using DbFlyup.SqlServer;
using DbUp.SqlServer;

/// <summary>
/// Configuration extension methods for SQL Server.
/// </summary>
// NOTE: DO NOT MOVE THIS TO A NAMESPACE
// Since the class just contains extension methods, we leave it in the global:: namespace so that it is always available
// ReSharper disable CheckNamespace
public static class FlywaySqlServerExtensions
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Creates an upgrader for SQL Server databases.
    /// </summary>
    /// <param name="supported">Fluent helper type.</param>
    /// <param name="connectionString">The connection string.</param>
    /// <returns>
    /// A builder for a database upgrader designed for SQL Server databases.
    /// </returns>
    public static UpgradeEngineBuilder FlywaySqlDatabase(this SupportedDatabases supported, string connectionString)
    {
        return FlywaySqlDatabase(supported, connectionString, null);
    }

    /// <summary>
    /// Creates an upgrader for SQL Server databases.
    /// </summary>
    /// <param name="supported">Fluent helper type.</param>
    /// <param name="connectionString">The connection string.</param>
    /// <param name="schema">The SQL schema name to use. Defaults to 'dbo'.</param>
    /// <returns>
    /// A builder for a database upgrader designed for SQL Server databases.
    /// </returns>
    public static UpgradeEngineBuilder FlywaySqlDatabase(this SupportedDatabases supported, string connectionString, string schema)
    {
        return FlywaySqlDatabase(new FlywaySqlConnectionManager(connectionString), schema);
    }

    /// <summary>
    /// Creates an upgrader for SQL Server databases.
    /// </summary>
    /// <param name="supported">Fluent helper type.</param>
    /// <param name="connectionManager">The <see cref="IConnectionManager"/> to be used during a database
    /// upgrade. See <see cref="SqlConnectionManager"/> for an example implementation</param>
    /// <param name="schema">The SQL schema name to use. Defaults to 'dbo'.</param>
    /// <returns>
    /// A builder for a database upgrader designed for SQL Server databases.
    /// </returns>
    public static UpgradeEngineBuilder FlywaySqlDatabase(this SupportedDatabases supported, IConnectionManager connectionManager, string schema = null)
        => FlywaySqlDatabase(connectionManager, schema);

    /// <summary>
    /// Creates an upgrader for SQL Server databases.
    /// </summary>
    /// <param name="connectionManager">The <see cref="IConnectionManager"/> to be used during a database
    /// upgrade. See <see cref="SqlConnectionManager"/> for an example implementation</param>
    /// <param name="schema">The SQL schema name to use. Defaults to 'dbo'.</param>
    /// <returns>
    /// A builder for a database upgrader designed for SQL Server databases.
    /// </returns>
    private static UpgradeEngineBuilder FlywaySqlDatabase(IConnectionManager connectionManager, string schema)
    {
        var builder = new UpgradeEngineBuilder();
        builder.Configure(c => c.ConnectionManager = connectionManager);
        builder.Configure(c => c.ScriptExecutor = new FlywaySqlScriptExecutor(() => c.ConnectionManager, () => c.Log, schema, () => c.VariablesEnabled, c.ScriptPreprocessors, () => c.Journal));
        builder.Configure(c => c.Journal = new FlywaySqlTableJournal(() => c.ConnectionManager, () => c.Log, schema, "SchemaVersions"));
        return builder;
    }

    /// <summary>
    /// Tracks the list of executed scripts in a SQL Server table.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="schema">The schema.</param>
    /// <param name="table">The table.</param>
    /// <returns></returns>
    public static UpgradeEngineBuilder JournalToFlywaySqlTable(this UpgradeEngineBuilder builder, IFlywayConf conf)
    {
        (var schema, var table) = GetTableParts(conf);
        builder.Configure(c => 
            c.Journal = new FlywaySqlTableJournal(() => c.ConnectionManager, () => c.Log, schema, table)
        );
        return builder;
    }

    public static UpgradeEngineBuilder JournalToFlywayHashTable(this UpgradeEngineBuilder builder, IFlywayConf conf)
    {
        (var schema, var table) = GetTableParts(conf);
        builder.Configure(c => 
            c.Journal = new FlywaySqlHashTableJournal(conf, () => c.ConnectionManager, () => c.Log, schema, table)
        );
        return builder;
    }

    private static (string schema, string table) GetTableParts(IFlywayConf conf)
    {
        var jrnlParts = conf.Table?.Split('.') ?? new[] { FlywayConf.DefaultMetadataSchema, FlywayConf.DefaultMetadataTable };
        var jrnlSchema = jrnlParts.Length > 1 && !string.IsNullOrWhiteSpace(jrnlParts[jrnlParts.Length - 2]) ?
             jrnlParts[jrnlParts.Length - 2] : FlywayConf.DefaultMetadataSchema;
        var jrnlTable = !string.IsNullOrWhiteSpace(jrnlParts[jrnlParts.Length - 1]) ?
             jrnlParts[jrnlParts.Length - 1] : FlywayConf.DefaultMetadataTable;

        return (jrnlSchema, jrnlTable);
    }


    /// <summary>
    /// Ensures that the database specified in the connection string exists.
    /// </summary>
    /// <param name="supported">Fluent helper type.</param>
    /// <param name="connectionString">The connection string.</param>
    /// <returns></returns>
    public static void FlywaySqlDatabase(this SupportedDatabasesForEnsureDatabase supported, string connectionString)
    {
        FlywaySqlDatabase(supported, connectionString, new ConsoleUpgradeLog());
    }

    /// <summary>
    /// Ensures that the database specified in the connection string exists.
    /// </summary>
    /// <param name="supported">Fluent helper type.</param>
    /// <param name="connectionString">The connection string.</param>
    /// <param name="azureDatabaseEdition">Azure edition to Create</param>
    /// <returns></returns>
    public static void FlywaySqlDatabase(this SupportedDatabasesForEnsureDatabase supported, string connectionString, AzureDatabaseEdition azureDatabaseEdition)
    {
        FlywaySqlDatabase(supported, connectionString, new ConsoleUpgradeLog(), -1, azureDatabaseEdition);
    }

    /// <summary>
    /// Ensures that the database specified in the connection string exists.
    /// </summary>
    /// <param name="supported">Fluent helper type.</param>
    /// <param name="connectionString">The connection string.</param>
    /// <param name="commandTimeout">Use this to set the command time out for creating a database in case you're encountering a time out in this operation.</param>
    /// <returns></returns>
    public static void FlywaySqlDatabase(this SupportedDatabasesForEnsureDatabase supported, string connectionString, int commandTimeout)
    {
        FlywaySqlDatabase(supported, connectionString, new ConsoleUpgradeLog(), commandTimeout);
    }

    /// <summary>
    /// Ensures that the database specified in the connection string exists.
    /// </summary>
    /// <param name="supported">Fluent helper type.</param>
    /// <param name="connectionString">The connection string.</param>
    /// <param name="collation">The collation name to set during database creation</param>
    /// <returns></returns>
    public static void FlywaySqlDatabase(this SupportedDatabasesForEnsureDatabase supported, string connectionString, string collation)
    {
        FlywaySqlDatabase(supported, connectionString, new ConsoleUpgradeLog(), collation: collation);
    }

    /// <summary>
    /// Ensures that the database specified in the connection string exists.
    /// </summary>
    /// <param name="supported">Fluent helper type.</param>
    /// <param name="connectionString">The connection string.</param>
    /// <param name="commandTimeout">Use this to set the command time out for creating a database in case you're encountering a time out in this operation.</param>
    /// <param name="azureDatabaseEdition">Azure edition to Create</param>
    /// <returns></returns>
    public static void FlywaySqlDatabase(this SupportedDatabasesForEnsureDatabase supported, string connectionString, int commandTimeout, AzureDatabaseEdition azureDatabaseEdition)
    {
        FlywaySqlDatabase(supported, connectionString, new ConsoleUpgradeLog(), commandTimeout, azureDatabaseEdition);
    }

    /// <summary>
    /// Ensures that the database specified in the connection string exists.
    /// </summary>
    /// <param name="supported">Fluent helper type.</param>
    /// <param name="connectionString">The connection string.</param>
    /// <param name="commandTimeout">Use this to set the command time out for creating a database in case you're encountering a time out in this operation.</param>
    /// <param name="collation">The collation name to set during database creation</param>
    /// <returns></returns>
    public static void FlywaySqlDatabase(this SupportedDatabasesForEnsureDatabase supported, string connectionString, int commandTimeout, string collation)
    {
        FlywaySqlDatabase(supported, connectionString, new ConsoleUpgradeLog(), commandTimeout, collation: collation);
    }

    /// <summary>
    /// Ensures that the database specified in the connection string exists.
    /// </summary>
    /// <param name="supported">Fluent helper type.</param>
    /// <param name="connectionString">The connection string.</param>
    /// <param name="azureDatabaseEdition">Azure edition to Create</param>
    /// <param name="collation">The collation name to set during database creation</param>
    /// <returns></returns>
    public static void FlywaySqlDatabase(this SupportedDatabasesForEnsureDatabase supported, string connectionString, AzureDatabaseEdition azureDatabaseEdition, string collation)
    {
        FlywaySqlDatabase(supported, connectionString, new ConsoleUpgradeLog(), azureDatabaseEdition: azureDatabaseEdition, collation: collation);
    }

    /// <summary>
    /// Ensures that the database specified in the connection string exists.
    /// </summary>
    /// <param name="supported">Fluent helper type.</param>
    /// <param name="connectionString">The connection string.</param>
    /// <param name="commandTimeout">Use this to set the command time out for creating a database in case you're encountering a time out in this operation.</param>
    /// <param name="azureDatabaseEdition">Azure edition to Create</param>
    /// <param name="collation">The collation name to set during database creation</param>
    /// <returns></returns>
    public static void FlywaySqlDatabase(this SupportedDatabasesForEnsureDatabase supported, string connectionString, int commandTimeout, AzureDatabaseEdition azureDatabaseEdition, string collation)
    {
        FlywaySqlDatabase(supported, connectionString, new ConsoleUpgradeLog(), commandTimeout, azureDatabaseEdition, collation);
    }

    /// <summary>
    /// Ensures that the database specified in the connection string exists.
    /// </summary>
    /// <param name="supported">Fluent helper type.</param>
    /// <param name="connectionString">The connection string.</param>
    /// <param name="logger">The <see cref="DbUp.Engine.Output.IUpgradeLog"/> used to record actions.</param>
    /// <param name="timeout">Use this to set the command time out for creating a database in case you're encountering a time out in this operation.</param>
    /// <param name="azureDatabaseEdition">Use to indicate that the SQL server database is in Azure</param>
    /// <param name="collation">The collation name to set during database creation</param>
    /// <returns></returns>
    public static void FlywaySqlDatabase(
        this SupportedDatabasesForEnsureDatabase supported,
        string connectionString,
        IUpgradeLog logger,
        int timeout = -1,
        AzureDatabaseEdition azureDatabaseEdition = AzureDatabaseEdition.None,
        string collation = null)
    {
        string databaseName;
        string masterConnectionString;
        GetMasterConnectionStringBuilder(connectionString, logger, out masterConnectionString, out databaseName);

        using (var connection = new SqlConnection(masterConnectionString))
        {

            /*
            if (!string.IsNullOrWhiteSpace(_provider.Conf.Url))
            {
                var connstring = _provider.Conf.GetConnectionString();
                var sqlbuilder = new SqlConnectionStringBuilder(connstring);
                sqlbuilder.InitialCatalog = "master";

                var cm = new global::DbUp.SqlServer.SqlConnectionManager(sqlbuilder.ConnectionString);
                var exec = new DbUpSqlScriptExecution(cm, _provider.Conf);
                exec.Run(_callbacks.createDatabase,  journal:NullJournal);                
            }
            */
            connection.Open();

            var sqlCommandText = string.Format
                (
                    @"SELECT TOP 1 case WHEN dbid IS NOT NULL THEN 1 ELSE 0 end FROM sys.sysdatabases WHERE name = '{0}';",
                    databaseName
                );


            // check to see if the database already exists..
            using (var command = new SqlCommand(sqlCommandText, connection)
            {
                CommandType = CommandType.Text
            })

            {
                var results = (int?)command.ExecuteScalar();

                // if the database exists, we're done here...
                if (results.HasValue && results.Value == 1)
                {
                    return;
                }
            }

            string collationString = string.IsNullOrEmpty(collation) ? "" : string.Format(@" COLLATE {0}", collation);
            sqlCommandText = string.Format
                    (
                        @"create database [{0}]{1};",
                        databaseName,
                        collationString
                    );

            switch (azureDatabaseEdition)
            {
                case AzureDatabaseEdition.Basic:
                    sqlCommandText += " ( EDITION = ''basic'' );";
                    break;
                case AzureDatabaseEdition.Standard:
                    sqlCommandText += " ( EDITION = ''standard'' );";
                    break;
                case AzureDatabaseEdition.Premium:
                    sqlCommandText += " ( EDITION = ''premium'' );";
                    break;
            }


            // Create the database...
            using (var command = new SqlCommand(sqlCommandText, connection)
            {
                CommandType = CommandType.Text
            })
            {
                if (timeout >= 0)
                {
                    command.CommandTimeout = timeout;
                }

                command.ExecuteNonQuery();

            }

            logger.WriteInformation(@"Created database {0}", databaseName);
        }
    }

    /// <summary>
    /// Drop the database specified in the connection string.
    /// </summary>
    /// <param name="supported">Fluent helper type.</param>
    /// <param name="connectionString">The connection string.</param>
    /// <returns></returns>
    public static void FlywaySqlDatabase(this SupportedDatabasesForDropDatabase supported, string connectionString)
    {
        FlywaySqlDatabase(supported, connectionString, new ConsoleUpgradeLog());
    }

    /// <summary>
    /// Drop the database specified in the connection string.
    /// </summary>
    /// <param name="supported">Fluent helper type.</param>
    /// <param name="connectionString">The connection string.</param>
    /// <param name="commandTimeout">Use this to set the command time out for dropping a database in case you're encountering a time out in this operation.</param>
    /// <returns></returns>
    public static void FlywaySqlDatabase(this SupportedDatabasesForDropDatabase supported, string connectionString, int commandTimeout)
    {
        FlywaySqlDatabase(supported, connectionString, new ConsoleUpgradeLog(), commandTimeout);
    }

    /// <summary>
    /// Drop the database specified in the connection string.
    /// </summary>
    /// <param name="supported">Fluent helper type.</param>
    /// <param name="connectionString">The connection string.</param>
    /// <param name="logger">The <see cref="DbUp.Engine.Output.IUpgradeLog"/> used to record actions.</param>
    /// <param name="timeout">Use this to set the command time out for dropping a database in case you're encountering a time out in this operation.</param>
    /// <returns></returns>
    public static void FlywaySqlDatabase(this SupportedDatabasesForDropDatabase supported, string connectionString, IUpgradeLog logger, int timeout = -1)
    {
        string databaseName;
        string masterConnectionString;
        GetMasterConnectionStringBuilder(connectionString, logger, out masterConnectionString, out databaseName);

        using (var connection = new SqlConnection(masterConnectionString))
        {
            connection.Open();
            var databaseExistCommand = new SqlCommand($"SELECT TOP 1 case WHEN dbid IS NOT NULL THEN 1 ELSE 0 end FROM sys.sysdatabases WHERE name = '{databaseName}';", connection)
            {
                CommandType = CommandType.Text
            };
            using (var command = databaseExistCommand)
            {
                var exists = (int?)command.ExecuteScalar();
                if (!exists.HasValue)
                    return;
            }

            var dropDatabaseCommand = new SqlCommand($"ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{databaseName}];", connection) { CommandType = CommandType.Text };
            using (var command = dropDatabaseCommand)
            {
                command.ExecuteNonQuery();
            }

            logger.WriteInformation("Dropped database {0}", databaseName);
        }
    }

    internal static void GetMasterConnectionStringBuilder(string connectionString, IUpgradeLog logger, out string masterConnectionString, out string databaseName)
    {
        if (string.IsNullOrEmpty(connectionString) || connectionString.Trim() == string.Empty)
            throw new ArgumentNullException("connectionString");

        var masterConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        databaseName = masterConnectionStringBuilder.InitialCatalog;

        if (string.IsNullOrEmpty(databaseName) || databaseName.Trim() == string.Empty)
            throw new InvalidOperationException("The connection string does not specify a database name.");

        masterConnectionStringBuilder.InitialCatalog = "master";
        var logMasterConnectionStringBuilder = new SqlConnectionStringBuilder(masterConnectionStringBuilder.ConnectionString)
        {
            Password = string.Empty.PadRight(masterConnectionStringBuilder.Password.Length, '*')
        };

        logger?.WriteInformation("Master ConnectionString => {0}", logMasterConnectionStringBuilder.ConnectionString);
        masterConnectionString = masterConnectionStringBuilder.ConnectionString;
    }
}