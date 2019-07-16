using System;
using System.Collections.Generic;
using System.Linq;
using DbUp.Engine;
using DbUp.Engine.Transactions;

namespace DbFlyup.SqlServer
{
    public class DbUpSqlScriptRunner : IFlywayScriptRunner
    {
        private readonly IConnectionManager _connection;
        private readonly IFlywayFileProvider _provider;
        private readonly IFlywayConf _conf;

        public IConnectionManager Connection => _connection;

        public DbUpSqlScriptRunner(IFlywayFileProvider provider)
        {
            this._provider = provider ?? throw new ArgumentNullException(nameof(provider));
            this._conf = _provider.Conf;
            this._connection = new FlywaySqlConnectionManager(_conf);
        }

        public DbUpSqlScriptRunner(IConnectionManager connection, IFlywayConf conf)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _conf = conf ?? throw new ArgumentNullException(nameof(conf));
            _provider = new FlywaySqlFileProvider(conf);
        }


        public void EnsureDatabase()
        {
            DbUp.EnsureDatabase.For.FlywaySqlDatabase(
                _conf.GetConnectionString()
            );
        }

        public DatabaseUpgradeResult RunHashed(IEnumerable<SqlScript> scripts, IEnumerable<SqlScript> before = null, IEnumerable<SqlScript> after = null, IEnumerable<SqlScript> afterErr = null)
        {
            foreach (var v in scripts) { RunHashed(v, before, after, afterErr); }
            return new DatabaseUpgradeResult(scripts, true, null);
        }
        public DatabaseUpgradeResult Run(IEnumerable<SqlScript> scripts, IEnumerable<SqlScript> before = null, IEnumerable<SqlScript> after = null, IEnumerable<SqlScript> afterErr = null, IJournal journal = null)
        {
            foreach (var v in scripts) {
                Run(v, before, after, afterErr, journal);
            }
            return new DatabaseUpgradeResult(scripts, true, null);
        }

        public DatabaseUpgradeResult RunHashed(SqlScript script, IEnumerable<SqlScript> before = null, IEnumerable<SqlScript> after = null, IEnumerable<SqlScript> afterErr = null)
        {
            if (script == null) { throw new ArgumentNullException(nameof(script)); }

            var builder = CreateHashBuilder();
            var result = Run(builder, script, before, after, afterErr);
            return result;
        }

        public DatabaseUpgradeResult Run(SqlScript script, IEnumerable<SqlScript> before = null, IEnumerable<SqlScript> after = null, IEnumerable<SqlScript> afterErr = null, IJournal journal = null)
        {
            if (script == null) { throw new ArgumentNullException(nameof(script)); }

            var builder = CreateBuilder(journal);
            var result = Run(builder, script, before, after, afterErr);
            return result;
        }

        protected DatabaseUpgradeResult Run(DbUp.Builder.UpgradeEngineBuilder builder, SqlScript script, IEnumerable<SqlScript> before = null, IEnumerable<SqlScript> after = null, IEnumerable<SqlScript> afterErr = null)
        {
            if (before?.Any() ?? false)
                builder.WithScripts(before);

            builder.WithScript(script);

            var eng = builder.Build();

            if (eng.IsUpgradeRequired())
            {
                var result = eng.PerformUpgrade();

                if (result.Successful)
                {
                    if (after?.Any() ?? false)
                        global::DbUp.DeployChanges.To.SqlDatabase(Connection)
                            .JournalToFlywaySqlTable(_conf)
                            .WithVariables(_conf.Placeholders)
                            .WithScripts(after ?? Enumerable.Empty<SqlScript>())
                            .Build().PerformUpgrade();
                }
                else
                {
                    if (afterErr?.Any() ?? false)
                        global::DbUp.DeployChanges.To.SqlDatabase(Connection)
                            .JournalToFlywaySqlTable(_conf)
                            .WithVariables(_conf.Placeholders)
                            .WithScripts(afterErr ?? Enumerable.Empty<SqlScript>())
                            .Build().PerformUpgrade();

                }
                return result;
            }
            return new DatabaseUpgradeResult(Enumerable.Empty<SqlScript>(), true, null);
        }


        private DbUp.Builder.UpgradeEngineBuilder CreateBuilder(IJournal journal)
        {
            var builder = CreateBaseBuilder();
            if (journal != null)
            {
                builder.JournalTo(journal);
            }
            else
            {
                builder.JournalToFlywaySqlTable(_conf);
            }
            return builder;
        }

        private DbUp.Builder.UpgradeEngineBuilder CreateHashBuilder()
        {
            return CreateBaseBuilder()
                .JournalToFlywayHashTable(_conf);
        }

        private DbUp.Builder.UpgradeEngineBuilder CreateBaseBuilder()
        {
            return global::DbUp.DeployChanges.To.SqlDatabase(Connection)
                .WithVariablesEnabled()
                .WithVariables(_conf.Placeholders)
                .LogScriptOutput()
                .LogToConsole();
        }
    }
}
