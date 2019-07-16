using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DbUp.Engine.Transactions;

namespace DbFlyup
{
    public abstract class FlywayTestRunner : IFlywayTestRunner
    {
        protected readonly IConnectionManager _connection;
        protected readonly IFlywayConf _conf;

        public FlywayTestRunner(IConnectionManager connection, IFlywayConf conf)
        {
            this._connection = connection ?? throw new ArgumentNullException(nameof(connection));
            this._conf = conf ?? throw new ArgumentNullException(nameof(conf));
        }

        public void RunTests()
        {
            using (var writer = GetStreamWriter())
            {
                RunTests(writer);
            }
        }

        protected virtual StreamWriter GetStreamWriter()
        {
            var fileName = _conf.TestResultFile;
            var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            var pathInfo = new FileInfo(path);
            if (!pathInfo.Directory.Exists)
                pathInfo.Directory.Create();

            System.Diagnostics.Trace.TraceInformation($"Test result file = {path}");
            //System.Console.WriteLine($"Test result file = {path}");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            return new StreamWriter(File.OpenWrite(path));
        }

        public abstract void RunTests(System.IO.StreamWriter writer);
    }
}
