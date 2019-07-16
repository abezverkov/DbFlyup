using DbUp.Engine.Transactions;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DbFlyup.SqlServer.Tests
{
    public static class FLywayTestBuilder
    {
        public static FlywayConf BuildFlywayConf(Dictionary<string,string> values = null)
        {
            var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sql");
            values = values ?? new Dictionary<string, string>() {
                { "Locations", path },
                { "Url", "server=.;database=test" },
                { "User", "user" },
                { "Password", "password" },
            };
        
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            var config = configurationBuilder
                .AddInMemoryCollection(values?? new Dictionary<string, string>())
                .Build();

            return new FlywaySqlConf(config);
        }

        public static FlywayFileProvider BuildFlywayFileProvider(Dictionary<string, string> values = null)
        {
            var conf = BuildFlywayConf(values);
            return new FlywaySqlFileProvider(conf);
        }

        public static FlywayUpdater BuildFlywayUpdater(Dictionary<string, string> values = null, IFlywayScriptRunner exec = null)
        {
            var provider = BuildFlywayFileProvider(values);
            return BuildFlywayUpdater(provider, exec);
        }

        public static FlywayUpdater BuildFlywayUpdater(IFlywayFileProvider provider, IFlywayScriptRunner exec = null, IFlywayTestRunner testRunner = null)
        {
            exec = exec ?? new Mock<IFlywayScriptRunner>().Object;
            testRunner = testRunner ?? BuildFlywayTestRunner(provider.Conf);
            return new FlywayUpdater(provider, exec, testRunner);
        }

        public static FlywayTestRunner BuildFlywayTestRunner(IFlywayConf conf)
        {
            var conn = new Mock<IConnectionManager>().Object;
            return new FlywayTSQLTMemoryRunner(conf, conn);
        }

        public static FlywayTestRunner BuildFlywayTestRunner(Dictionary<string, string> values = null)
        {
            var conf = BuildFlywayConf(values);
            return BuildFlywayTestRunner(conf);
        }

    }
}
