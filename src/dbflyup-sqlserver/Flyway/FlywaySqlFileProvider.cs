using System.Collections.Generic;
using System.IO;
using System.Linq;
using DbUp.Engine;
using DbUp.Engine.Transactions;
using DbUp.ScriptProviders;

namespace DbFlyup.SqlServer
{
    public class FlywaySqlFileProvider : FlywayFileProvider
    {
        public FlywaySqlFileProvider(IFlywayConf conf) 
            : base(conf)
        {
        }

        protected override IEnumerable<SqlScript> GetAllProviderScripts()
        {
            var paths = Conf.Locations;
            if (paths == null || paths.Count() == 0)
            {
                paths = new[] { Directory.GetCurrentDirectory() };
            }
            var scripts = new List<SqlScript>();
            foreach (var path in paths)
            {
                //var fsProvider = new FileSystemScriptProvider(path, new FileSystemScriptOptions() { IncludeSubDirectories = true });
                //var fsScripts = fsProvider.GetScripts(Connection);
                scripts.AddRange(GetAllProviderScripts(path));
            }
            return scripts;
        }

        protected IEnumerable<SqlScript> GetAllProviderScripts(string path)
        {
            var encoding = System.Text.Encoding.UTF8;
            var sqlScriptOptions = new SqlScriptOptions();

            var files = GetFiles(path, true);
            return files.Select(x =>
            {
                var s = SqlScript.FromFile(path, x.FullName, encoding, sqlScriptOptions);
                return new SqlScript(x.Name, s.Contents, s.SqlScriptOptions);
            });

        }

        private IEnumerable<FileInfo> GetFiles(string path, bool includeSubDirs)
        {
            var searchOpts = includeSubDirs ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return Directory
                .GetFiles(path, "*.sql", searchOpts)
                .Select(f => new FileInfo(f));
        }

        protected override string GetRootNamespace()
        {
            return this.GetType().Namespace;
        }

        protected override IEnumerable<SqlScript> GetDefaultScripts()
        {
            return new EmbeddedScriptProvider(this.GetType().Assembly, x => true, Conf.Encoding)
                .GetScripts(null);
        }
        private static string GetExecutionDirectory()
        {
            var codebase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            return new System.IO.FileInfo(codebase).Directory.FullName;
        }
    }
}
