using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DbUp.Engine;

namespace DbFlyup
{
    public class FlywayCallbacks
    {
        private readonly IFlywayFileProvider _provider;

        public FlywayCallbacks(IFlywayFileProvider provider)
        {
            this._provider = provider;
        }

        public IEnumerable<SqlScript> beforeMigrate { get { return gcb(); } }
        public IEnumerable<SqlScript> beforeEachMigrate { get { return gcb(); } }
        public IEnumerable<SqlScript> afterEachMigrate { get { return gcb(); } }
        public IEnumerable<SqlScript> afterEachMigrateError { get { return gcb(); } }
        public IEnumerable<SqlScript> afterMigrate { get { return gcb(); } }
        public IEnumerable<SqlScript> afterMigrateError { get { return gcb(); } }
        public IEnumerable<SqlScript> beforeUndo { get { return gcb(); } }
        public IEnumerable<SqlScript> beforeEachUndo { get { return gcb(); } }
        public IEnumerable<SqlScript> afterEachUndo { get { return gcb(); } }
        public IEnumerable<SqlScript> afterEachUndoError { get { return gcb(); } }
        public IEnumerable<SqlScript> afterUndo { get { return gcb(); } }
        public IEnumerable<SqlScript> afterUndoError { get { return gcb(); } }
        public IEnumerable<SqlScript> beforeClean { get { return gcb(); } }
        public IEnumerable<SqlScript> afterClean { get { return gcb(); } }
        public IEnumerable<SqlScript> afterCleanError { get { return gcb(); } }
        public IEnumerable<SqlScript> beforeInfo { get { return gcb(); } }
        public IEnumerable<SqlScript> afterInfo { get { return gcb(); } }
        public IEnumerable<SqlScript> afterInfoError { get { return gcb(); } }
        public IEnumerable<SqlScript> beforeValidate { get { return gcb(); } }
        public IEnumerable<SqlScript> afterValidate { get { return gcb(); } }
        public IEnumerable<SqlScript> afterValidateError { get { return gcb(); } }
        public IEnumerable<SqlScript> beforeBaseline { get { return gcb(); } }
        public IEnumerable<SqlScript> afterBaseline { get { return gcb(); } }
        public IEnumerable<SqlScript> afterBaselineError { get { return gcb(); } }
        public IEnumerable<SqlScript> beforeRepair { get { return gcb(); } }
        public IEnumerable<SqlScript> afterRepair { get { return gcb(); } }
        public IEnumerable<SqlScript> afterRepairError { get { return gcb(); } }

        public IEnumerable<SqlScript> beforeTest { get { return gcb(); } }
        public IEnumerable<SqlScript> beforeEachTest { get { return gcb(); } }
        public IEnumerable<SqlScript> afterEachTest { get { return gcb(); } }
        public IEnumerable<SqlScript> afterEachTestError { get { return gcb(); } }
        public IEnumerable<SqlScript> afterTest { get { return gcb(); } }
        public IEnumerable<SqlScript> afterTestError { get { return gcb(); } }
        public IEnumerable<SqlScript> createDatabase { get { return gcb(); } }
        public IEnumerable<SqlScript> cleanDatabase { get { return gcb(); } }

        private IEnumerable<SqlScript> gcb([CallerMemberName] string caller = null)
        {
            return _provider.GetCallbackScripts(caller);
        }
    }
}
