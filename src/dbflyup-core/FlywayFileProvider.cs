using System;
using System.Collections.Generic;
using System.Linq;
using DbUp.Engine;
using DbUp.Engine.Transactions;
using DbUp.Support;


namespace DbFlyup
{
    public abstract class FlywayFileProvider : IFlywayFileProvider
    {
        private IEnumerable<SqlScript> _fileScripts;
        private IEnumerable<SqlScript> _defaultScripts;

        public IFlywayConf Conf { get; }
        public FlywayCallbacks Callbacks { get; } 

        public FlywayFileProvider(IFlywayConf conf)
        {
            this.Conf = conf ?? throw new ArgumentNullException(nameof(conf));
            this.Callbacks = new FlywayCallbacks(this);

            _fileScripts = GetAllProviderScripts();
            _defaultScripts = GetDefaultScripts();
        }

        protected abstract IEnumerable<SqlScript> GetAllProviderScripts();

        protected abstract IEnumerable<SqlScript> GetDefaultScripts();

        protected abstract string GetRootNamespace();

        public IEnumerable<SqlScript> GetVersionScripts()
        {
            string prefix = Conf.SqlMigrationPrefix;
            return _fileScripts.FilterFileNameStartsWith(prefix)
                .AsSqlScriptType(ScriptType.RunOnce)
                .OrderByFileName();
        }

        public IEnumerable<SqlScript> GetUndoScripts()
        {
            string prefix = Conf.UndoSqlMigrationPrefix;
            return _fileScripts.FilterFileNameStartsWith(prefix)
                .AsSqlScriptType(ScriptType.RunOnce)
                .OrderByFileName();
        }

        public IEnumerable<SqlScript> GetRepeatableScripts()
        {
            string prefix = Conf.RepeatableSqlMigrationPrefix + Conf.SqlMigrationSeparator;
            return _fileScripts.FilterFileNameStartsWith(prefix)
                .AsSqlScriptType(ScriptType.RunOnce)
                .OrderByFileName();
        }

        public IEnumerable<SqlScript> GetTestScripts()
        {
            string prefix = Conf.TestSqlMigrationPrefix + Conf.SqlMigrationSeparator;
            return _fileScripts.FilterFileNameStartsWith(prefix)
                .AsSqlScriptType(ScriptType.RunOnce)
                .OrderByFileName();
        }

        public IEnumerable<SqlScript> GetCallbackScripts(string callbackName)
        {
            var scripts = _fileScripts.FilterFileNameStartsWith(callbackName)
                .AsSqlScriptType(ScriptType.RunAlways)
                .OrderByFileName();

            if (scripts?.Count() == 0)
            {
                string assemName = this.GetRootNamespace();
                string defaultPath = $"{assemName}.sql.{callbackName}";
                scripts = _defaultScripts.FilterFileNameStartsWith(defaultPath)
                .AsSqlScriptType(ScriptType.RunAlways)
                .OrderByFileName();
            }
            return scripts;
        }
    }

}
