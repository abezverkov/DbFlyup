using System;
using System.Collections.Generic;
using System.Linq;
using DbUp.Engine.Output;
using DbUp.Engine.Transactions;
using DbUp.SqlServer;
using DbUp.Support;

namespace DbFlyup.SqlServer
{
    /// <summary>
    /// An implementation of the <see cref="Engine.IJournal"/> interface which tracks version numbers for a 
    /// SQL Server database using a table called dbo.SchemaVersions.
    /// </summary>
    public class FlywaySqlTableJournal : HashedTableJournal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlTableJournal"/> class.
        /// </summary>
        /// <param name="connectionManager">The connection manager.</param>
        /// <param name="logger">The log.</param>
        /// <param name="schema">The schema that contains the table.</param>
        /// <param name="table">The table name.</param>
        /// <example>
        /// var journal = new TableJournal("Server=server;Database=database;Trusted_Connection=True", "dbo", "MyVersionTable");
        /// </example>
        public FlywaySqlTableJournal(Func<IConnectionManager> connectionManager, Func<IUpgradeLog> logger, string schema, string table)
            : base(connectionManager, logger, new SqlServerObjectParser(), schema, table)
        {
        }

        protected override IEnumerable<HashedTableJournalEntry> GetDeploymentHashScripts()
        {
            return Enumerable.Empty<HashedTableJournalEntry>();
        }

        protected override string GetInsertJournalEntrySql(string @scriptName, string @applied, string @scriptHash)
        {
            return $"insert into {FqSchemaTableName} (ScriptName, Applied, ScriptHash) values ({@scriptName}, {@applied}, NULL)";
        }

        protected override string GetJournalEntriesSql()
        {
            return $"select [ScriptName],[ScriptHash] from {FqSchemaTableName} where [ScriptHash] IS NULL order by [ScriptName]";
        }

        protected override string CreateSchemaTableSql(string quotedPrimaryKeyName)
        {
            return
$@"create table {FqSchemaTableName} (
    [Id] int identity(1,1) not null constraint {quotedPrimaryKeyName} primary key,
    [ScriptName] nvarchar(255) not null,
    [Applied] datetime not null,
    [ScriptHash] nvarchar(255) null
)";
        }

        protected override string CreateSchemaSql()
        {
            return
$@"IF NOT EXISTS ( SELECT * FROM sys.schemas WHERE name = N'{this.SchemaTableSchema}' ) 
    EXEC('CREATE SCHEMA [{this.SchemaTableSchema}] AUTHORIZATION [dbo]');
";
        }
    }

    public class FlywaySqlHashTableJournal : FlywaySqlTableJournal
    {
        private readonly FlywaySqlFileProvider _provider;

        public FlywaySqlHashTableJournal(IFlywayConf conf, Func<IConnectionManager> connectionManager, Func<IUpgradeLog> logger, string schema, string table)
            : base(connectionManager, logger, schema, table)
        {
            _provider = new FlywaySqlFileProvider(conf);
        }

        protected override IEnumerable<HashedTableJournalEntry> GetDeploymentHashScripts()
        {
            var scripts = _provider.GetRepeatableScripts()
                .Select(s => new HashedTableJournalEntry(s.Name, Md5Utils.Md5EncodeString(s.Contents)));
            return scripts;
        }

        protected override string GetInsertJournalEntrySql(string @scriptName, string @applied, string @scriptHash)
        {
            return $"insert into {FqSchemaTableName} (ScriptName, Applied, ScriptHash) values ({@scriptName}, {@applied}, {@scriptHash})";
        }

        protected override string GetJournalEntriesSql()
        {
            return $"select [ScriptName],[ScriptHash] from {FqSchemaTableName} where [ScriptHash] IS NOT NULL order by [ScriptName]";
        }
    }
}