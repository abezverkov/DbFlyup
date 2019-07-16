using System.Collections.Generic;
using System.Linq;
using DbUp.Engine;
using DbUp.Engine.Transactions;
using DbUp.Support;

namespace DbFlyup
{
    internal static class SqlScriptExtensions
    {
        internal static void Execute(this IEnumerable<SqlScript> scripts, IConnectionManager connection)
        {
            connection.ExecuteCommandsWithManagedConnection(x =>
            {
                using (var command = x())
                {
                    scripts.SelectMany(s1 => connection.SplitScriptIntoCommands(s1.Contents))
                        .ToList().ForEach(s2 =>
                        {
                            command.CommandText = s2;
                            command.ExecuteNonQuery();
                        });
                }
            });
        }

        internal static IEnumerable<SqlScript> OrderByFileName(this IEnumerable<SqlScript> scripts)
        {
            return scripts.OrderBy(x => new System.IO.FileInfo(x.Name).Name);
        }

        internal static IEnumerable<SqlScript> FilterFileNameStartsWith(this IEnumerable<SqlScript> scripts, string filter)
        {
            return scripts.Where(x => new System.IO.FileInfo(x.Name).Name.StartsWith(filter));
        }

        internal static IEnumerable<SqlScript> AsSqlScriptType(this IEnumerable<SqlScript> scripts, ScriptType scriptType)
        {
            return scripts.Select(x => x.Clone(new SqlScriptOptions { ScriptType = scriptType }));
        }

        internal static SqlScript Clone(this SqlScript script, SqlScriptOptions newOpts = null)
        {
            newOpts = newOpts ?? new SqlScriptOptions()
            {
                RunGroupOrder = script.SqlScriptOptions.RunGroupOrder,
                ScriptType = script.SqlScriptOptions.ScriptType,
            };
            return new SqlScript(script.Name, script.Contents, newOpts);
        }
    }
}
