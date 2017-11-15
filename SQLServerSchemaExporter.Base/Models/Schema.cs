using System.Collections.Generic;

namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Simple class representing a schema in the database.
    /// </summary>
    internal class Schema
    {
        private readonly static HashSet<string> DefaultSchemas = new HashSet<string>
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

        internal string Name { get; }

        internal bool IsDefaultSchema
        {
            get
            {
                return DefaultSchemas.Contains(Name);
            }
        }

        internal Schema(string name)
        {
            Name = name;
        }

        internal string ToSqlString()
        {
            return $"CREATE SCHEMA {Name}";
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
