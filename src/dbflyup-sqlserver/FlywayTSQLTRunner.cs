using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DbUp.Engine.Transactions;

namespace DbFlyup
{
    public class FlywayTSQLTRunner : FlywayTestRunner
    {
        private static IConnectionManager GetConnection(IFlywayConf conf)
        {
            return new DbUp.SqlServer.SqlConnectionManager(conf.GetConnectionString());
        }

        public FlywayTSQLTRunner(IFlywayConf conf, IConnectionManager conn = null)
            : base(conn ?? GetConnection(conf), conf)
        {
        }

        public override void RunTests(System.IO.StreamWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            var testResults = string.Empty;
            using (var op = _connection.OperationStarting(null,null)) {
                testResults = _connection
                    .ExecuteCommandsWithManagedConnection<string>(x => f(x));
            }
            writer.WriteLine(testResults);
            writer.Flush();
        }
        
        private string f (Func<System.Data.IDbCommand> x)
        {
            var command = x();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "EXEC [tSQLt].[RunAll]; EXEC [tSQLt].[XMLResultFormatter];";
            return command.ExecuteScalar().ToString();
        }
    }
}
