using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace DbFlyup
{
    public abstract class FlywayConf : IFlywayConf
    {
        public const string DefaultMetadataSchema = "metadata";
        public const string DefaultMetadataTable = "flyway_schema_history";

        private readonly IConfiguration _config;

        public FlywayConf(IConfiguration config)
        {
            this._config = config ?? throw new ArgumentNullException(nameof(config));
        }
        private T cfg<T>([CallerMemberName]string key = null)
        {
            return _config.GetValue<T>(key);
        }

        public virtual string Url => cfg<string>();
        //public string Driver => cfg<string>();
        public virtual string User => cfg<string>();
        public virtual string Password => cfg<string>();
        public virtual int ConnectRetries => cfg<int>();
        public virtual string InitSql => cfg<string>();
        public virtual string[] Schemas => cfg<string>()?.Split(',');
        public virtual string Table => cfg<string>() ?? DefaultMetadataTable;
        public virtual string[] Locations => cfg<string>()?.Split(',');
        //public virtual string Resolvers => cfg<string>();
        public virtual bool SkipDefaultResolvers => cfg<bool>();
        //public virtual string JarDirs => cfg<string>();
        public virtual string SqlMigrationPrefix => cfg<string>() ?? "V";
        public virtual string UndoSqlMigrationPrefix => cfg<string>() ?? "U";
        public virtual string RepeatableSqlMigrationPrefix => cfg<string>() ?? "R";
        public virtual string TestSqlMigrationPrefix => cfg<string>() ?? "T";
        public virtual string SqlMigrationSeparator => cfg<string>() ?? "__";
        public virtual string SqlMigrationSuffixes => cfg<string>() ?? ".sql";
        public virtual bool Stream => cfg<bool>();
        public virtual bool Batch => cfg<bool>();
        public virtual Encoding Encoding { get; set; } = Encoding.UTF8;
        public virtual bool PlaceholderReplacement => cfg<bool>();
        public virtual Dictionary<string, string> Placeholders { get; set; } = new Dictionary<string, string>();
        public virtual string PlaceholderPrefix => cfg<string>() ?? @"${";
        public virtual string PlaceholderSuffix => cfg<string>() ?? @"}";
        public virtual string Target => cfg<string>();
        public virtual bool ValidateOnMigrate => cfg<bool?>() ?? true;
        public virtual bool CleanOnValidationError => cfg<bool>();
        public virtual bool CleanDisabled => cfg<bool>();
        public virtual decimal BaselineVersion => cfg<decimal?>() ?? 1;
        public virtual string BaselineDescription => cfg<string>();
        public virtual bool BaselineOnMigrate => cfg<bool>();
        public virtual bool OutOfOrder => cfg<bool>();
        public virtual string Callbacks => cfg<string>();
        public virtual bool SkipDefaultCallbacks => cfg<bool>();
        public virtual bool IgnoreMissingMigrations => cfg<bool>();
        public virtual bool IgnoreIgnoredMigrations => cfg<bool>();
        public virtual bool IgnorePendingMigrations => cfg<bool>();
        public virtual bool IgnoreFutureMigrations => cfg<bool>();
        public virtual bool Mixed => cfg<bool>();
        public virtual bool Group => cfg<bool>();
        public virtual string InstalledBy => cfg<string>();
        public virtual ErrorOverride[] ErrorOverrides { get; set; }
        public virtual string DryRunOutput { get; set; }
        //public virtual string LicenseKey { get; set; }

        // Custom
        public virtual string Collation => cfg<string>();
        public virtual string TestResultFile => cfg<string>() ?? "TestResults.xml";

        public abstract string GetDatabaseName();
        public abstract string GetConnectionString();
    }

    public class ErrorOverride
    {
        public string State { get; set; }
        public int ErrorCode { get; set; }
        public ErrorOverrideBehavior Behavior { get; set; }
    }

    public enum ErrorOverrideBehavior
    {
        W,E,I,D
    }
}
