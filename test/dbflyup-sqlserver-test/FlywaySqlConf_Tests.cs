using System.Collections.Generic;
using Xunit;
using FluentAssertions;

namespace DbFlyup.SqlServer.Tests
{
    public class FlywaySqlConf_Value_Tests
    {
        private readonly FlywayConf _conf;

        public FlywaySqlConf_Value_Tests()
        {
            var values = new Dictionary<string, string>() {
                { "Url", "server=.;database=test" },
                { "User", "user" },
                { "Password", "password" },
            };
            _conf = FLywayTestBuilder.BuildFlywayConf(values);
        }

        [Fact]
        public void Test_Url()
        {
            //public string Url => cfg<string>();
            _conf.Url.Should().Be("server=.;database=test");
        }

        [Fact]
        public void Test_User()
        {
            //public string User => cfg<string>();
            _conf.User.Should().Be("user");
        }

        [Fact]
        public void Test_Password()
        {
            //public string Password => cfg<string>();
            _conf.Password.Should().Be("password");
        }

        [Fact]
        public void Test_GetDatabaseName()
        {
            _conf.GetDatabaseName().Should().Be("test");
        }

        [Fact]
        public void Test_GetConnectionString()
        {
            _conf.GetConnectionString().Should().Be("Data Source=.;Initial Catalog=test;User ID=user;Password=password");
        }


    }

    public class FlywaySqlConf_Default_Tests
    {
        protected readonly IFlywayConf _conf;

        public FlywaySqlConf_Default_Tests()
        {
            var values = new Dictionary<string, string>();
            _conf = FLywayTestBuilder.BuildFlywayConf(values);
        }

        [Fact]
        public void Test_Url()
        {
            //public string Url => cfg<string>();
            _conf.Url.Should().Be(null);
        }

        [Fact]
        public void Test_Driver()
        {
            //public string Driver => cfg<string>();
            //_conf.Driver.Should().Be("");
        }

        [Fact]
        public void Test_User()
        {
            //public string User => cfg<string>();
            _conf.User.Should().Be(null);
        }

        [Fact]
        public void Test_Password()
        {
            //public string Password => cfg<string>();
            _conf.Password.Should().Be(null);
        }

        [Fact]
        public void Test_ConnectRetries()
        {
            //public int ConnectRetries => cfg<int>();
            _conf.ConnectRetries.Should().Be(0);
        }

        [Fact]
        public void Test_InitSql()
        {
            //public string InitSql => cfg<string>();
            _conf.InitSql.Should().Be(null);
        }

        [Fact]
        public void Test_Schemas()
        {
            //public string[] Schemas => cfg<string>().Split(',');
            _conf.Schemas.Should().BeNull();
        }

        [Fact]
        public void Test_Table()
        {
            //public string Table => cfg<string>() ?? "flyway_schema_history";
            _conf.Table.Should().Be("flyway_schema_history");
        }

        [Fact]
        public void Test_Locations()
        {
            //public string[] Locations => cfg<string>().Split(',');
            _conf.Locations.Should().BeNull();
        }

        [Fact]
        public void Test_Resolvers()
        {
            //public string Resolvers => cfg<string>();
            //_conf.Resolvers.Should().Be("");
        }

        [Fact]
        public void Test_SkipDefaultResolvers()
        {
            //public bool SkipDefaultResolvers => cfg<bool>();
            _conf.SkipDefaultResolvers.Should().Be(false);
        }

        [Fact]
        public void Test_JarDirs()
        {
            //public string JarDirs => cfg<string>();
            //_conf.JarDirs.Should().Be("");
        }

        [Fact]
        public void Test_SqlMigrationPrefix()
        {
            //public string SqlMigrationPrefix => cfg<string>() ?? "V";
            _conf.SqlMigrationPrefix.Should().Be("V");
        }

        [Fact]
        public void Test_UndoSqlMigrationPrefix()
        {
            //public string UndoSqlMigrationPrefix => cfg<string>() ?? "U";
            _conf.UndoSqlMigrationPrefix.Should().Be("U");
        }

        [Fact]
        public void Test_RepeatableSqlMigrationPrefix()
        {
            //public string RepeatableSqlMigrationPrefix => cfg<string>() ?? "R";
            _conf.RepeatableSqlMigrationPrefix.Should().Be("R");
        }

        [Fact]
        public void Test_TestSqlMigrationPrefix()
        {
            //public string TestSqlMigrationPrefix => cfg<string>() ?? "T";
            _conf.TestSqlMigrationPrefix.Should().Be("T");
        }

        [Fact]
        public void Test_SqlMigrationSeparator()
        {
            //public string SqlMigrationSeparator => cfg<string>() ?? "__";
            _conf.SqlMigrationSeparator.Should().Be("__");
        }

        [Fact]
        public void Test_SqlMigrationSuffixes()
        {
            //public string SqlMigrationSuffixes => cfg<string>() ?? ".sql";
            _conf.SqlMigrationSuffixes.Should().Be(".sql");
        }

        [Fact]
        public void Test_Stream()
        {
            //public bool Stream => cfg<bool>();
            _conf.Stream.Should().Be(false);
        }

        [Fact]
        public void Test_Batch()
        {
            //public bool Batch => cfg<bool>();
            _conf.Batch.Should().Be(false);
        }

        [Fact]
        public void Test_Encoding()
        {
            //public Encoding Encoding { get; set; } = Encoding.UTF8;
            _conf.Encoding.Should().Be(System.Text.Encoding.UTF8);
        }

        [Fact]
        public void Test_PlaceholderReplacement()
        {
            //public bool PlaceholderReplacement => cfg<bool>();
            _conf.PlaceholderReplacement.Should().Be(false);
        }

        [Fact]
        public void Test_Placeholders()
        {
            //public Dictionary<string, string> Placeholders { get; set; } = new Dictionary<string, string>();
            _conf.Placeholders.Count.Should().Be(4);
        }

        [Fact]
        public void Test_PlaceholderPrefix()
        {
            //public string PlaceholderPrefix => cfg<string>() ?? @"${";
            _conf.PlaceholderPrefix.Should().Be(@"${");
        }

        [Fact]
        public void Test_PlaceholderSuffix()
        {
            //public string PlaceholderSuffix => cfg<string>() ?? @"}";
            _conf.PlaceholderSuffix.Should().Be(@"}");
        }

        [Fact]
        public void Test_Target()
        {
            //public string Target => cfg<string>();
            _conf.Target.Should().Be(null);
        }

        [Fact]
        public void Test_ValidateOnMigrate()
        {
            //public bool ValidateOnMigrate => cfg<bool?>() ?? true;
            _conf.ValidateOnMigrate.Should().Be(true);
        }

        [Fact]
        public void Test_CleanOnValidationError()
        {
            //public bool CleanOnValidationError => cfg<bool>();
            _conf.CleanOnValidationError.Should().Be(false);
        }

        [Fact]
        public void Test_CleanDisabled()
        {
            //public bool CleanDisabled => cfg<bool>();
            _conf.CleanDisabled.Should().Be(false);
        }

        [Fact]
        public void Test_BaselineVersion()
        {
            //public decimal BaselineVersion => cfg<decimal?>() ?? 1;
            _conf.BaselineVersion.Should().Be(1);
        }

        [Fact]
        public void Test_BaselineDescription()
        {
            //public string BaselineDescription => cfg<string>();
            _conf.BaselineDescription.Should().Be(null);
        }

        [Fact]
        public void Test_BaselineOnMigrate()
        {
            //public bool BaselineOnMigrate => cfg<bool>();
            _conf.BaselineOnMigrate.Should().Be(false);
        }

        [Fact]
        public void Test_OutOfOrder()
        {
            //public bool OutOfOrder => cfg<bool>();
            _conf.OutOfOrder.Should().Be(false);
        }

        [Fact]
        public void Test_Callbacks()
        {
            //public string Callbacks => cfg<string>();
            _conf.Callbacks.Should().Be(null);
        }

        [Fact]
        public void Test_SkipDefaultCallbacks()
        {
            //public bool SkipDefaultCallbacks => cfg<bool>();
            _conf.SkipDefaultCallbacks.Should().Be(false);
        }

        [Fact]
        public void Test_IgnoreMissingMigrations()
        {
            //public bool IgnoreMissingMigrations => cfg<bool>();
            _conf.IgnoreMissingMigrations.Should().Be(false);
        }

        [Fact]
        public void Test_IgnoreIgnoredMigrations()
        {
            //public bool IgnoreIgnoredMigrations => cfg<bool>();
            _conf.IgnoreIgnoredMigrations.Should().Be(false);
        }

        [Fact]
        public void Test_IgnorePendingMigrations()
        {
            //public bool IgnorePendingMigrations => cfg<bool>();
            _conf.IgnorePendingMigrations.Should().Be(false);
        }

        [Fact]
        public void Test_IgnoreFutureMigrations()
        {
            //public bool IgnoreFutureMigrations => cfg<bool>();
            _conf.IgnoreFutureMigrations.Should().Be(false);
        }

        [Fact]
        public void Test_Mixed()
        {
            //public bool Mixed => cfg<bool>();
            _conf.Mixed.Should().Be(false);
        }

        [Fact]
        public void Test_Group()
        {
            //public bool Group => cfg<bool>();
            _conf.Group.Should().Be(false);
        }

        [Fact]
        public void Test_InstalledBy()
        {
            //public string InstalledBy => cfg<string>();
            _conf.InstalledBy.Should().Be(null);
        }

        [Fact]
        public void Test_ErrorOverrides()
        {
            //public ErrorOverride[] ErrorOverrides { get; set; }
            _conf.ErrorOverrides.Should().BeNull();
        }

        [Fact]
        public void Test_DryRunOutput()
        {
            //public string DryRunOutput { get; set; }
            _conf.DryRunOutput.Should().Be(null);
        }

        [Fact]
        public void Test_LicenseKey()
        {
            //public string LicenseKey { get; set; }
            //_conf.LicenseKey.Should().Be("");
        }

        [Fact]
        public void Test_GetDatabaseName()
        {
            _conf.GetDatabaseName().Should().Be(null);
        }

        [Fact]
        public void Test_GetConnectionString()
        {
            _conf.GetConnectionString().Should().Be(null);
        }
    }
}
