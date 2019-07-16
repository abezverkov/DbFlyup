using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DbUp.Engine.Transactions;

namespace DbFlyup
{
    public class FlywayTSQLTMemoryRunner : FlywayTSQLTRunner
    {
        public FlywayTSQLTMemoryRunner(IFlywayConf conf, IConnectionManager conn = null)
            : base(conf, conn)
        {
        }

        protected override StreamWriter GetStreamWriter()
        {
            return new StreamWriter(new MemoryStream());
        }
    }
}
