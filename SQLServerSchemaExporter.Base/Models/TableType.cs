using System.Collections.Generic;
using System.Linq;

namespace SQLServerSchemaExporter.Base.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a user defined table type in the database..
    /// </summary>
    internal class TableType : TableOrView
    {
        internal TableType(string name, Schema schema, List<Column> columns)
            : base(name, schema, columns)
        { }

        internal override string ToSqlString()
        {
            var columnsString = string.Join(",\n", Columns.Select(c => c.ToSqlString()));
            return $@"CREATE TYPE [{Schema.Name}].[{Name}] AS TABLE ({columnsString})";
        }
    }
}
