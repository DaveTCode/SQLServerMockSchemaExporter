using System.Collections.Generic;

namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Represents the common elements between all single fields (like a 
    /// parameter or a table/view column)
    /// </summary>
    internal class Type
    {
        // TODO - Any want to ignore these at the data retrieval stage? This is ugly.
        private static readonly HashSet<string> TypesWithCharLengthThatShouldBeIgnored = new HashSet<string>
        {
            "XML",
            "TEXT"
        };

        internal string DbType { get; }

        internal int? MaxCharacterLength { get; }

        internal Type(string dbType, int? maxCharacterLength)
        {
            DbType = dbType;
            MaxCharacterLength = maxCharacterLength;
        }

        public string ToSqlString()
        {
            var sqlString = DbType;
            if (MaxCharacterLength.HasValue && !TypesWithCharLengthThatShouldBeIgnored.Contains(DbType.ToUpperInvariant()))
            {
                sqlString += $"({(MaxCharacterLength == -1 ? "MAX" : MaxCharacterLength.Value.ToString())})";
            }

            return sqlString;
        }
    }
}
