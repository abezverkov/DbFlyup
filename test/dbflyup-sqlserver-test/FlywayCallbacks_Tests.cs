using DbUp.Engine.Transactions;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Xunit;
using FluentAssertions;

namespace DbFlyup.SqlServer.Tests
{
    public class FlywayCallbacks_Tests
    {
        protected FlywayCallbacks _callbacks;
        private Dictionary<string, string> ConfigValues = new Dictionary<string, string>();

        public FlywayCallbacks_Tests()
        {
            var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sql");
            this.ConfigValues.Add("Locations", path);

            _callbacks = BuildCallbacks();
        }

        protected FlywayCallbacks BuildCallbacks()
        {
            var provider = FLywayTestBuilder.BuildFlywayFileProvider();
            return new FlywayCallbacks(provider);
        }

        [Fact]
        public void When_Callback_beforeMigrate()
        {
            // public IEnumerable<SqlScript> beforeMigrate { get { return gcb(); } }
            var scripts = _callbacks.beforeMigrate;
            scripts.Should().HaveCount(2);
        }

        [Fact]
        public void When_Callback_beforeEachMigrate()
        {
            // public IEnumerable<SqlScript> beforeEachMigrate { get { return gcb(); } }
            var scripts = _callbacks.beforeEachMigrate;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_afterEachMigrate()
        {
            // public IEnumerable<SqlScript> afterEachMigrate { get { return gcb(); } }
            var scripts = _callbacks.afterEachMigrate;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_afterEachMigrateError()
        {
            // public IEnumerable<SqlScript> afterEachMigrateError { get { return gcb(); } }
            var scripts = _callbacks.afterEachMigrateError;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_afterMigrate()
        {
            // public IEnumerable<SqlScript> afterMigrate { get { return gcb(); } }
            var scripts = _callbacks.afterMigrate;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_afterMigrateError()
        {
            // public IEnumerable<SqlScript> afterMigrateError { get { return gcb(); } }
            var scripts = _callbacks.afterMigrateError;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_beforeUndo()
        {
            // public IEnumerable<SqlScript> beforeUndo { get { return gcb(); } }
            var scripts = _callbacks.beforeUndo;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_beforeEachUndo()
        {
            // public IEnumerable<SqlScript> beforeEachUndo { get { return gcb(); } }
            var scripts = _callbacks.beforeEachUndo;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_afterEachUndo()
        {
            // public IEnumerable<SqlScript> afterEachUndo { get { return gcb(); } }
            var scripts = _callbacks.afterEachUndo;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_afterEachUndoError()
        {
            // public IEnumerable<SqlScript> afterEachUndoError { get { return gcb(); } }
            var scripts = _callbacks.afterEachUndoError;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_afterUndo()
        {
            // public IEnumerable<SqlScript> afterUndo { get { return gcb(); } }
            var scripts = _callbacks.afterUndo;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_afterUndoError()
        {
            // public IEnumerable<SqlScript> afterUndoError { get { return gcb(); } }
            var scripts = _callbacks.afterUndoError;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_beforeClean()
        {
            // public IEnumerable<SqlScript> beforeClean { get { return gcb(); } }
            var scripts = _callbacks.beforeClean;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_afterClean()
        {
            // public IEnumerable<SqlScript> afterClean { get { return gcb(); } }
            var scripts = _callbacks.afterClean;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_afterCleanError()
        {
            // public IEnumerable<SqlScript> afterCleanError { get { return gcb(); } }
            var scripts = _callbacks.afterCleanError;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_beforeInfo()
        {
            // public IEnumerable<SqlScript> beforeInfo { get { return gcb(); } }
            var scripts = _callbacks.beforeInfo;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_afterInfo()
        {
            // public IEnumerable<SqlScript> afterInfo { get { return gcb(); } }
            var scripts = _callbacks.afterInfo;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_afterInfoError()
        {
            // public IEnumerable<SqlScript> afterInfoError { get { return gcb(); } }
            var scripts = _callbacks.afterInfoError;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_beforeValidate()
        {
            // public IEnumerable<SqlScript> beforeValidate { get { return gcb(); } }
            var scripts = _callbacks.beforeValidate;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_afterValidate()
        {
            // public IEnumerable<SqlScript> afterValidate { get { return gcb(); } }
            var scripts = _callbacks.afterValidate;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_afterValidateError()
        {
            // public IEnumerable<SqlScript> afterValidateError { get { return gcb(); } }
            var scripts = _callbacks.afterValidateError;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_beforeBaseline()
        {
            // public IEnumerable<SqlScript> beforeBaseline { get { return gcb(); } }
            var scripts = _callbacks.beforeBaseline;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_afterBaseline()
        {
            // public IEnumerable<SqlScript> afterBaseline { get { return gcb(); } }
            var scripts = _callbacks.afterBaseline;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_afterBaselineError()
        {
            // public IEnumerable<SqlScript> afterBaselineError { get { return gcb(); } }
            var scripts = _callbacks.afterBaselineError;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_beforeRepair()
        {
            // public IEnumerable<SqlScript> beforeRepair { get { return gcb(); } }
            var scripts = _callbacks.beforeRepair;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_afterRepair()
        {
            // public IEnumerable<SqlScript> afterRepair { get { return gcb(); } }
            var scripts = _callbacks.afterRepair;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_afterRepairError()
        {
            // public IEnumerable<SqlScript> afterRepairError { get { return gcb(); } }
            var scripts = _callbacks.afterRepairError;
            scripts.Should().HaveCount(0);
        }


        [Fact]
        public void When_Callback_beforeTest()
        {
            // public IEnumerable<SqlScript> beforeTest { get { return gcb(); } }
            var scripts = _callbacks.beforeTest;
            scripts.Should().HaveCount(1);
        }

        [Fact]
        public void When_Callback_beforeEachTest()
        {
            // public IEnumerable<SqlScript> beforeEachTest { get { return gcb(); } }
            var scripts = _callbacks.beforeEachTest;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_afterEachTest()
        {
            // public IEnumerable<SqlScript> afterEachTest { get { return gcb(); } }
            var scripts = _callbacks.afterEachTest;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_afterEachTestError()
        {
            // public IEnumerable<SqlScript> afterEachTestError { get { return gcb(); } }
            var scripts = _callbacks.afterEachTestError;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_afterTest()
        {
            // public IEnumerable<SqlScript> afterTest { get { return gcb(); } }
            var scripts = _callbacks.afterTest;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_afterTestError()
        {
            // public IEnumerable<SqlScript> afterTestError { get { return gcb(); } }
            var scripts = _callbacks.afterTestError;
            scripts.Should().HaveCount(0);
        }

        [Fact]
        public void When_Callback_createDatabase()
        {
            // public IEnumerable<SqlScript> createDatabase { get { return gcb(); } }
            var scripts = _callbacks.createDatabase;
            scripts.Should().HaveCount(1);
        }

        [Fact]
        public void When_Callback_cleanDatabase()
        {
            // public IEnumerable<SqlScript> cleanDatabase { get { return gcb(); } }
            var scripts = _callbacks.cleanDatabase;
            scripts.Should().HaveCount(1);
        }
    }
}
