using DbUp.Engine;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DbFlyup.SqlServer.Tests
{
    public class FlywayUpdater_Tests
    {
        private Mock<IFlywayScriptRunner> _exec;
        private FlywayUpdater _updater;

        public FlywayUpdater_Tests()
        {
            _exec = new Mock<IFlywayScriptRunner>();
            _exec.Setup(x => x.Run(
                It.IsAny<IEnumerable<SqlScript>>(),
                It.IsAny<IEnumerable<SqlScript>>(),
                It.IsAny<IEnumerable<SqlScript>>(),
                It.IsAny<IEnumerable<SqlScript>>(),
                It.IsAny<IJournal>()
             )).Returns<IEnumerable<SqlScript>
                ,IEnumerable<SqlScript>
                ,IEnumerable<SqlScript>
                ,IEnumerable<SqlScript>
                ,IJournal>(
                    (x1,x2,x3,x4,j) => new DatabaseUpgradeResult(x1, true, null)
             );
            _exec.Setup(x => x.RunHashed(
                It.IsAny<IEnumerable<SqlScript>>(),
                It.IsAny<IEnumerable<SqlScript>>(),
                It.IsAny<IEnumerable<SqlScript>>(),
                It.IsAny<IEnumerable<SqlScript>>()
             )).Returns<IEnumerable<SqlScript>
                , IEnumerable<SqlScript>
                , IEnumerable<SqlScript>
                , IEnumerable<SqlScript>>(
                    (x1, x2, x3, x4) => new DatabaseUpgradeResult(x1, true, null)
             );
            _updater = FLywayTestBuilder.BuildFlywayUpdater(exec: _exec.Object);
        }

        [Fact]
        public void Baseline()
        {
            _updater.Baseline();
            _exec.Verify(x => x.Run(
                It.IsAny<IEnumerable<SqlScript>>(),
                It.IsAny<IEnumerable<SqlScript>>(),
                It.IsAny<IEnumerable<SqlScript>>(),
                It.IsAny<IEnumerable<SqlScript>>(),
                It.IsAny<IJournal>()), Times.Exactly(2));
        }

        [Fact]
        public void Clean()
        {
            _updater.Clean();
            _exec.Verify(x => x.Run(
                It.IsAny<IEnumerable<SqlScript>>(),
                It.Is<IEnumerable<SqlScript>>(b => b == null),
                It.Is<IEnumerable<SqlScript>>(b => b == null),
                It.Is<IEnumerable<SqlScript>>(b => b == null),
                It.IsAny<IJournal>()), Times.Exactly(3));
        }

        [Fact]
        public void Info()
        {
            _updater.Info();
            _exec.Verify(x => x.Run(
                It.IsAny<IEnumerable<SqlScript>>(),
                It.Is<IEnumerable<SqlScript>>(b => b == null),
                It.Is<IEnumerable<SqlScript>>(b => b == null),
                It.Is<IEnumerable<SqlScript>>(b => b == null),
                It.IsAny<IJournal>()), Times.Exactly(2));
        }

        [Fact]
        public void Migrate()
        {
            _updater.Migrate();
            _exec.Verify(x => x.Run(
                It.IsAny<IEnumerable<SqlScript>>(),
                It.IsAny<IEnumerable<SqlScript>>(),
                It.IsAny<IEnumerable<SqlScript>>(),
                It.IsAny<IEnumerable<SqlScript>>(),
                It.Is<IJournal>(j => j == null)), Times.Exactly(3));
            _exec.Verify(x => x.RunHashed(
                It.IsAny<IEnumerable<SqlScript>>(),
                It.IsAny<IEnumerable<SqlScript>>(),
                It.IsAny<IEnumerable<SqlScript>>(),
                It.IsAny<IEnumerable<SqlScript>>()), Times.Exactly(1));
        }

        [Fact]
        public void Repair()
        {
            _updater.Repair();
            _exec.Verify(x => x.Run(
                It.IsAny<IEnumerable<SqlScript>>(),
                It.Is<IEnumerable<SqlScript>>(b => b == null),
                It.Is<IEnumerable<SqlScript>>(b => b == null),
                It.Is<IEnumerable<SqlScript>>(b => b == null),
                It.IsAny<IJournal>()), Times.Exactly(2));
        }

        [Fact]
        public void Test()
        {
            _updater.Test();
            _exec.Verify(x => x.Run(
               It.IsAny<IEnumerable<SqlScript>>(),
               It.IsAny<IEnumerable<SqlScript>>(),
               It.IsAny<IEnumerable<SqlScript>>(),
               It.IsAny<IEnumerable<SqlScript>>(),
               It.IsAny<IJournal>()), Times.Exactly(2));
            _exec.Verify(x => x.RunHashed(
               It.IsAny<IEnumerable<SqlScript>>(),
               It.IsAny<IEnumerable<SqlScript>>(),
               It.IsAny<IEnumerable<SqlScript>>(),
               It.IsAny<IEnumerable<SqlScript>>()), Times.Exactly(1));
        }

        [Fact]
        public void Undo()
        {
            _updater.Undo();
            _exec.Verify(x => x.Run(
                It.IsAny<IEnumerable<SqlScript>>(),
                It.IsAny<IEnumerable<SqlScript>>(),
                It.IsAny<IEnumerable<SqlScript>>(),
                It.IsAny<IEnumerable<SqlScript>>(),
                It.IsAny<IJournal>()), Times.Exactly(3));
        }

        [Fact]
        public void Validate()
        {
            _updater.Validate();
            _exec.Verify(x => x.Run(
                It.IsAny<IEnumerable<SqlScript>>(),
                It.Is<IEnumerable<SqlScript>>(b => b == null),
                It.Is<IEnumerable<SqlScript>>(b => b == null),
                It.Is<IEnumerable<SqlScript>>(b => b == null),
                It.IsAny<IJournal>()), Times.Exactly(2));
        }
    }
}
