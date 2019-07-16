using System;
using System.IO;
using System.Threading.Tasks;
using DbUp.Engine;
using DbUp.Engine.Transactions;

namespace DbFlyup
{
    public class FlywayUpdater : IFlywayUpdater
    {
        private readonly IFlywayFileProvider _provider;
        private readonly IFlywayScriptRunner _runner;
        private readonly IFlywayTestRunner _testRunner;
        private readonly FlywayCallbacks _callbacks;

        public FlywayUpdater(IFlywayFileProvider provider, IFlywayScriptRunner runner, IFlywayTestRunner testRunner)
        {
            this._provider = provider ??
                throw new ArgumentNullException(nameof(provider));
            this._runner = runner ??
                throw new ArgumentNullException(nameof(runner));
            this._testRunner = testRunner;
            this._callbacks = new FlywayCallbacks(provider);
        }

        private Task<int> ParseReturn(DatabaseUpgradeResult result,
            [System.Runtime.CompilerServices.CallerMemberName] string caller = null)
        {
            if (result.Successful)
            {
                Console.WriteLine($"Finished Successful: {caller}");
                return Task.FromResult(0);
            }
            else
            {
                Console.WriteLine($"Finished Failed: {caller}");
                return Task.FromResult(1);
            }
        }

        private void EnsureDatabase() { _runner.EnsureDatabase(); }

        private IJournal NullJournal  => new global::DbUp.Helpers.NullJournal();


        public Task<int> Migrate()
        {
            Console.WriteLine($"Starting: {nameof(this.Migrate)}");
            EnsureDatabase();

            DatabaseUpgradeResult result = null;
            result = _runner.Run(_callbacks.beforeMigrate);
            if (!result.Successful) return ParseReturn(result);

            // Collect
            var before = _callbacks.beforeEachMigrate;
            var after = _callbacks.afterEachMigrate;
            var afterErr = _callbacks.afterEachMigrateError;

            result = _runner.Run(_provider.GetVersionScripts(), before, after, afterErr);
            result = _runner.RunHashed(_provider.GetRepeatableScripts(), before, after, afterErr);

            if (!result.Successful)
            {
                _runner.Run(_callbacks.afterMigrateError);
            }
            else
            {
                _runner.Run(_callbacks.afterMigrate);
            }

            return ParseReturn(result);
        }


        public Task<int> Clean()
        {
            Console.WriteLine($"Starting: {nameof(this.Clean)}");
            EnsureDatabase();

            DatabaseUpgradeResult result = null;
            var j = new DbUp.Helpers.NullJournal();
            result = _runner.Run(_callbacks.beforeClean, journal: j);
            result = _runner.Run(_callbacks.cleanDatabase, journal: j);

            if (!result.Successful)
            {
                _runner.Run(_callbacks.afterCleanError, journal: j);
            }
            else
            {
                _runner.Run(_callbacks.afterClean, journal: j);
            }

            return ParseReturn(result);
        }

        public Task<int> Info()
        {
            Console.WriteLine($"Starting: {nameof(this.Info)}");
            EnsureDatabase();

            DatabaseUpgradeResult result = null;
            result = _runner.Run(_callbacks.beforeInfo);

            //TODO: Get Info scripts

            if (!result.Successful)
            {
                _runner.Run(_callbacks.afterInfoError);
            }
            else
            {
                _runner.Run(_callbacks.afterInfo);
            }

            return ParseReturn(result);
        }

        public Task<int> Validate()
        {
            Console.WriteLine($"Starting: {nameof(this.Validate)}");
            EnsureDatabase();

            DatabaseUpgradeResult result = null;
            result = _runner.Run(_callbacks.beforeValidate);

            //TODO: Get Validate scripts

            if (!result.Successful)
            {
                _runner.Run(_callbacks.afterValidateError);
            }
            else
            {
                _runner.Run(_callbacks.afterValidate);
            }

            return ParseReturn(result);
        }

        public Task<int> Undo()
        {
            Console.WriteLine($"Starting: {nameof(this.Undo)}");
            EnsureDatabase();

            DatabaseUpgradeResult result = null;
            result = _runner.Run(_callbacks.beforeUndo);

            // Collect
            var before = _callbacks.beforeEachUndo;
            var after = _callbacks.afterEachUndo;
            var afterErr = _callbacks.afterEachUndoError;

            result = _runner.Run(_provider.GetUndoScripts(), before, after, afterErr);

            if (!result.Successful)
            {
                _runner.Run(_callbacks.afterUndoError);
            }
            else
            {
                _runner.Run(_callbacks.afterUndo);
            }

            return ParseReturn(result);
        }

        public Task<int> Baseline()
        {
            Console.WriteLine($"Starting: {nameof(this.Baseline)}");
            EnsureDatabase();

            DatabaseUpgradeResult result = null;
            result = _runner.Run(_callbacks.beforeBaseline);

            //TODO: Get Baseline scripts

            if (!result.Successful)
            {
                _runner.Run(_callbacks.afterBaselineError);
            }
            else
            {
                _runner.Run(_callbacks.afterBaseline);
            }

            return ParseReturn(result);
        }

        public Task<int> Repair()
        {
            Console.WriteLine($"Starting: {nameof(this.Repair)}");
            EnsureDatabase();

            DatabaseUpgradeResult result = null;
            result = _runner.Run(_callbacks.beforeRepair);

            //TODO: Get Repair scripts

            if (!result.Successful)
            {
                _runner.Run(_callbacks.afterRepairError);
            }
            else
            {
                _runner.Run(_callbacks.afterRepair);
            }

            return ParseReturn(result);
        }

        public Task<int> Test()
        {
            Console.WriteLine($"Starting: {nameof(this.Test)}");
            EnsureDatabase();

            DatabaseUpgradeResult result = null;
            result = _runner.Run(_callbacks.beforeTest);

            // Collect
            var before = _callbacks.beforeEachTest;
            var after = _callbacks.afterEachTest;
            var afterErr = _callbacks.afterEachTestError;

            result = _runner.RunHashed(_provider.GetTestScripts(), before, after, afterErr);

            if (!result.Successful)
            {
                _runner.Run(_callbacks.afterTestError);               
            }
            else
            {
                _runner.Run(_callbacks.afterTest);
                if (_testRunner != null)
                {
                    _testRunner.RunTests();
                }
            }

            return ParseReturn(result);
        }
    }
}
