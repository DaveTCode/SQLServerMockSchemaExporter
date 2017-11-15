namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Represents a single column in a single database table.
    /// </summary>
    internal class Column
    {
        internal string Name { get; }

        internal Type Type { get; }

        internal bool IsNullable { get; }

        internal string DefaultValue { get; }

        internal Column(string name, Type type, bool isNullable, string columnDefault)
        {
            Name = name;
            Type = type;
            IsNullable = isNullable;
            DefaultValue = columnDefault;
        }

        internal string ToSqlString()
        {
            var sqlString = $"[{Name}] {Type.ToSqlString()}";

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
