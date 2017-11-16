namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Represents a single column in a single database table.
    /// </summary>
    internal class Column
    {
        private string Name { get; }

        private Type Type { get; }

        private bool IsNullable { get; }

        private string DefaultValue { get; }

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
