using System.Linq;
using Xunit;
using FluentAssertions;

namespace DbFlyup.SqlServer.Tests
{
    public class FlywayFileProvider_Tests
    {
        protected FlywayFileProvider _provider;

        public FlywayFileProvider_Tests()
        {
            _provider = FLywayTestBuilder.BuildFlywayFileProvider();
        }

        [Fact]
        public void When_Retrieving_Default_Scripts()
        {
            var scripts = _provider.GetCallbackScripts("createDatabase");
            scripts.Should().HaveCount(1);
            scripts.ToArray()[0].Name.Should().Be("DbFlyup.SqlServer.sql.createDatabase.sql");
        }

        [Fact]
        public void When_Retrieving_DefaultOverride_Scripts()
        {
            var scripts = _provider.GetCallbackScripts("cleanDatabase");
            scripts.Should().HaveCount(1);
            scripts.ToArray()[0].Name.Should().Be("cleanDatabase_02.sql");
        }

        [Fact]
        public void When_Retrieving_Callback_Scripts()
        {
            var scripts = _provider.GetCallbackScripts("beforeMigrate");
            scripts.Should().HaveCount(2);
            scripts.ToArray()[0].Name.Should().Be("beforeMigrate.sql");
            scripts.ToArray()[1].Name.Should().Be("beforeMigrate_02.sql");
        }

        [Fact]
        public void When_Retrieving_Repeatable_Scripts()
        {
            var scripts = _provider.GetRepeatableScripts();
            scripts.Should().HaveCount(2);
            scripts.ToArray()[0].Name.Should().Be("R__someOtherScript.sql");
            scripts.ToArray()[1].Name.Should().Be("R__somescript.sql");
        }

        [Fact]
        public void When_Retrieving_Test_Scripts()
        {
            var scripts = _provider.GetTestScripts();
            scripts.Should().HaveCount(1);
            scripts.ToArray()[0].Name.Should().Be("T__someTest.sql");
        }

        [Fact]
        public void When_Retrieving_Undo_Scripts()
        {
            var scripts = _provider.GetUndoScripts();
            scripts.Should().HaveCount(3);
            scripts.ToArray()[0].Name.Should().Be("U_1__somescript.sql");
            scripts.ToArray()[1].Name.Should().Be("U_1_1__somescript.sql");
            scripts.ToArray()[2].Name.Should().Be("U_20190308__somescript.sql");
        }

        [Fact]
        public void When_Retrieving_Version_Scripts()
        {
            var scripts = _provider.GetVersionScripts();
            scripts.Should().HaveCount(5);
            scripts.ToArray()[0].Name.Should().Be("V1__somescript.sql");
            scripts.ToArray()[1].Name.Should().Be("V1_1__somescript.sql");
            scripts.ToArray()[2].Name.Should().Be("V1_2__somescript.sql");
            scripts.ToArray()[3].Name.Should().Be("V2_2__somescript.sql");
            scripts.ToArray()[4].Name.Should().Be("V20190308__somescript.sql");
        }
    }
}
