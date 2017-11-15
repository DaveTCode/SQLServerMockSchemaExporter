﻿namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Represents a single column in a single database table.
    /// </summary>
    internal class Column : SingleDataItem
    {
        internal bool IsNullable { get; }

        internal string DefaultValue { get; }

        internal Column(string name, string dbType, int? maxCharacterLength, bool isNullable, string columnDefault)
            : base(name, dbType, maxCharacterLength)
        {
            IsNullable = isNullable;
            DefaultValue = columnDefault;
        }

        internal string ToSqlString()
        {
            var sqlString = $"[{Name}] {DbType}";
            if (MaxCharacterLength.HasValue)
            {
                sqlString += $"({(MaxCharacterLength == -1 ? "MAX" : MaxCharacterLength.Value.ToString())})";
            }

            if (!IsNullable)
            {
                sqlString += " NOT NULL";
            }

            if (DefaultValue != null)
            {
                sqlString += $" DEFAULT ({DefaultValue})";
            }

            return sqlString;
        }
    }
}
