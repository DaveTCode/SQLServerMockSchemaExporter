using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Represents a single table in the database.
    /// </summary>
    internal class TableOrView : BaseWritableObject
    {
        internal Schema Schema { get; }

        internal IReadOnlyList<Column> Columns { get; }

        internal TableOrView(string name, Schema schema, List<Column> columns)
            : base(name)
        {
            Schema = schema;
            Columns = new List<Column>(columns).AsReadOnly();
        }

        public override string ToString()
        {
            return $"{Schema.Name}.{Name}";
        }

        internal override string ToSqlFileContents()
        {
            var columnsString = string.Join(",\r\n", Columns.Select(c => c.ToSqlString()));
            return $@"CREATE TABLE [{Schema.Name}].[{Name}] ({columnsString})";
        }

        internal override FileInfo FilePath(string baseDirectory)
        {
            return new FileInfo(Path.Combine(baseDirectory, Schema.Name, "Tables", FileSafeName) + ".sql");
        }
    }
}
