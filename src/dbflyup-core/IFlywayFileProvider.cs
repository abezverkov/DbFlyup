using System.Collections.Generic;
using DbUp.Engine;
using DbUp.Engine.Transactions;

namespace DbFlyup
{
    public interface IFlywayFileProvider
    {
        IFlywayConf Conf { get; }
        IEnumerable<SqlScript> GetCallbackScripts(string callbackName);
        IEnumerable<SqlScript> GetRepeatableScripts();
        IEnumerable<SqlScript> GetTestScripts();
        IEnumerable<SqlScript> GetUndoScripts();
        IEnumerable<SqlScript> GetVersionScripts();
    }
}