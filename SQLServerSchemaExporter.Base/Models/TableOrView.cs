using System.Collections.Generic;
using System.Linq;

namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Represents a single table in the database.
    /// </summary>
    internal class TableOrView
    {
        internal string Name { get; }

        internal Schema Schema { get; }

        internal IReadOnlyList<Column> Columns { get; }

        internal TableOrView(string name, Schema schema, List<Column> columns)
        {
            Name = name;
            Schema = schema;
            Columns = new List<Column>(columns).AsReadOnly();
        }

        internal virtual string ToSqlString()
        {
            var columnsString = string.Join(",\n", Columns.Select(c => c.ToSqlString()));
            return $@"CREATE TABLE [{Schema.Name}].[{Name}] ({columnsString})";
        }

        public override string ToString()
        {
            return $"{Schema.Name}.{Name}";
        }
    }
}
