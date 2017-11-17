using System.Collections.Generic;
using System.IO;

namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Simple class representing a schema in the database.
    /// </summary>
    internal class Schema : BaseWritableObject
    {
        private static readonly HashSet<string> DefaultSchemas = new HashSet<string>
        {
            "db_accessadmin",
            "db_backupoperator", 
            "db_datareader",
            "db_datawriter",
            "db_ddladmin",
            "db_denydatareader",
            "db_denydatawriter",
            "db_owner",
            "db_securityadmin", 
            "dbo",
            "guest",
            "INFORMATION_SCHEMA",
            "sys"
        };

        internal bool IsDefaultSchema => DefaultSchemas.Contains(Name);

        internal Schema(string name) : base(name) { }

        internal override string ToSqlFileContents()
        {
            return $"CREATE SCHEMA {Name};";
        }

        internal override FileInfo FilePath(string baseDirectory)
        {
            return new FileInfo(Path.Combine(baseDirectory, "Security", FileSafeName) + ".sql");
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
