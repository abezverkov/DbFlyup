using System;
using System.Collections.Generic;
using System.Text;
using DbUp.Engine.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace DbFlyup.SqlServer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFlywaySql(this IServiceCollection services)
        {
            return services
                .AddSingleton<IFlywayConf, FlywaySqlConf>()
                .AddTransient<IConnectionManager, FlywaySqlConnectionManager>()
                .AddSingleton<IFlywayUpdater, FlywayUpdater>()
                .AddTransient<IFlywayFileProvider, FlywaySqlFileProvider>()
                .AddTransient<IFlywayScriptRunner, DbUpSqlScriptRunner>()
                .AddTransient<IFlywayTestRunner, FlywayTSQLTRunner>()
                ;
        }
    }
}
