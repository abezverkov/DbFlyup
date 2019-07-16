using System;
using System.Collections.Generic;
using System.Data;
using DbUp.Engine;
using DbUp.Engine.Transactions;
using Moq;
using Xunit;
using FluentAssertions;

namespace DbFlyup.SqlServer.Tests
{
    public class DbUpScriptExecution_Tests 
    {
        private readonly DbUpSqlScriptRunner _exec;
        private readonly Mock<IConnectionManager> _connection;

        public DbUpScriptExecution_Tests()
        {
            _connection = new Mock<IConnectionManager>();
            var conf = FLywayTestBuilder.BuildFlywayConf();
            _exec = new DbUpSqlScriptRunner(_connection.Object, conf);
        }

        private IJournal NullJournal => new global::DbUp.Helpers.NullJournal();
        private IJournal HashJournal => new global::DbUp.Helpers.NullJournal();


        [Fact]
        public void RunHashedScript_Test()
        {
            var opts = new SqlScriptOptions { RunGroupOrder = 10, ScriptType = global::DbUp.Support.ScriptType.RunOnce };
            SqlScript script = new SqlScript("test_script_1", "SELECT GETDATE()", opts);
            IEnumerable<SqlScript> before = null;
            IEnumerable<SqlScript> after = null;
            IEnumerable<SqlScript> afterErr = null;

            _exec.Run(script, before, after, afterErr, HashJournal);
        }

        [Fact]
        public void RunScript_Test()
        {
            var opts = new SqlScriptOptions { RunGroupOrder = 10, ScriptType = global::DbUp.Support.ScriptType.RunOnce };
            SqlScript script = new SqlScript("test_script_1","SELECT GETDATE()", opts);
            IEnumerable<SqlScript> before = null;
            IEnumerable<SqlScript> after = null;
            IEnumerable<SqlScript> afterErr = null;

            var result = _exec.Run(script, before, after, afterErr);
            _connection.Verify(x => x.ExecuteCommandsWithManagedConnection(It.IsAny<Action<Func<IDbCommand>>>()), Times.Exactly(1));
        }

        private void RunHashedScript(SqlScript script, IEnumerable<SqlScript> before = null, IEnumerable<SqlScript> after = null, IEnumerable<SqlScript> afterErr = null)
        {
            throw new NotImplementedException();
        }

        private void RunHashedScripts(IEnumerable<SqlScript> scripts, IEnumerable<SqlScript> before = null, IEnumerable<SqlScript> after = null, IEnumerable<SqlScript> afterErr = null)
        {
            throw new NotImplementedException();
        }

        private void RunScript(SqlScript script, IEnumerable<SqlScript> before = null, IEnumerable<SqlScript> after = null, IEnumerable<SqlScript> afterErr = null)
        {
            throw new NotImplementedException();
        }

        private void RunScripts(IEnumerable<SqlScript> scripts, IEnumerable<SqlScript> before = null, IEnumerable<SqlScript> after = null, IEnumerable<SqlScript> afterErr = null)
        {
            throw new NotImplementedException();
        }
    }
}
