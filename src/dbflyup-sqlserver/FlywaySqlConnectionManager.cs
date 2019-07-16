namespace DbFlyup.SqlServer
{
    public class FlywaySqlConnectionManager : DbUp.SqlServer.SqlConnectionManager
    {
        public FlywaySqlConnectionManager(IFlywayConf conf) : this(conf.GetConnectionString())
        {
        }

        public FlywaySqlConnectionManager(string connectionString) : base(connectionString)
        {
        }

    }
}
