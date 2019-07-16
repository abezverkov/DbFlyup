using System.Collections.Generic;
using DbUp.Engine;
using DbUp.Engine.Transactions;

namespace DbFlyup
{
    public interface IFlywayScriptRunner
    {
        IConnectionManager Connection { get; }

        void EnsureDatabase();

        DatabaseUpgradeResult Run(SqlScript script, IEnumerable<SqlScript> before = null, IEnumerable<SqlScript> after = null, IEnumerable<SqlScript> afterErr = null, IJournal journal = null);
        DatabaseUpgradeResult Run(IEnumerable<SqlScript> scripts, IEnumerable<SqlScript> before = null, IEnumerable<SqlScript> after = null, IEnumerable<SqlScript> afterErr = null, IJournal journal = null);

        DatabaseUpgradeResult RunHashed(SqlScript script, IEnumerable<SqlScript> before = null, IEnumerable<SqlScript> after = null, IEnumerable<SqlScript> afterErr = null);
        DatabaseUpgradeResult RunHashed(IEnumerable<SqlScript> scripts, IEnumerable<SqlScript> before = null, IEnumerable<SqlScript> after = null, IEnumerable<SqlScript> afterErr = null);
    }
}