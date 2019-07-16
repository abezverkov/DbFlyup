using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DbFlyup.SqlServer
{
    public class FlywaySqlConf: FlywayConf
    {
        public FlywaySqlConf(IConfiguration config) : base(config)
        {
            var dbName = GetDatabaseName();
            this.Placeholders.Add("DatabaseName", dbName);
            this.Placeholders.Add("DbName", dbName);
            this.Placeholders.Add("Database", dbName);
            this.Placeholders.Add("Db", dbName);
        }

        public override string GetDatabaseName()
        {
            if (!string.IsNullOrWhiteSpace(this.Url))
            {
                var builder = new SqlConnectionStringBuilder(this.Url);
                return builder.InitialCatalog;
            }
            return null;
        }

        public override string GetConnectionString()
        {
            if (!string.IsNullOrWhiteSpace(this.Url))
            {
                var builder = new SqlConnectionStringBuilder(this.Url);
                if (!string.IsNullOrWhiteSpace(this.User))
                {
                    builder.UserID = this.User;
                }
                if (!string.IsNullOrWhiteSpace(this.Password))
                {
                    builder.Password = this.Password;
                }
                if (string.IsNullOrWhiteSpace(builder.UserID) && string.IsNullOrWhiteSpace(builder.Password))
                {
                    builder.IntegratedSecurity = true;
                }
                return builder.ConnectionString;
            }
            return null;
        }
    }
}
