using System.Collections.Generic;
using System.Text;

namespace DbFlyup
{
    public interface IFlywayConf
    {
        string BaselineDescription { get; }
        bool BaselineOnMigrate { get; }
        decimal BaselineVersion { get; }
        bool Batch { get; }
        string Callbacks { get; }
        bool CleanDisabled { get; }
        bool CleanOnValidationError { get; }
        int ConnectRetries { get; }
        string DryRunOutput { get; set; }
        Encoding Encoding { get; set; }
        ErrorOverride[] ErrorOverrides { get; set; }
        bool Group { get; }
        bool IgnoreFutureMigrations { get; }
        bool IgnoreIgnoredMigrations { get; }
        bool IgnoreMissingMigrations { get; }
        bool IgnorePendingMigrations { get; }
        string InitSql { get; }
        string InstalledBy { get; }
        string[] Locations { get; }
        bool Mixed { get; }
        bool OutOfOrder { get; }
        string Password { get; }
        string PlaceholderPrefix { get; }
        bool PlaceholderReplacement { get; }
        Dictionary<string, string> Placeholders { get; set; }
        string PlaceholderSuffix { get; }
        string RepeatableSqlMigrationPrefix { get; }
        string[] Schemas { get; }
        bool SkipDefaultCallbacks { get; }
        bool SkipDefaultResolvers { get; }
        string SqlMigrationPrefix { get; }
        string SqlMigrationSeparator { get; }
        string SqlMigrationSuffixes { get; }
        bool Stream { get; }
        string Table { get; }
        string Target { get; }
        string TestSqlMigrationPrefix { get; }
        string UndoSqlMigrationPrefix { get; }
        string Url { get; }
        string User { get; }
        bool ValidateOnMigrate { get; }
        string TestResultFile { get; }

        string GetConnectionString();
        string GetDatabaseName();
    }
}