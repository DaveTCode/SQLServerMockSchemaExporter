using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Represents a single stored procedure in the database.
    /// </summary>
    internal class Procedure : BaseWritableObject
    {
        internal Schema Schema { get; }

        private IReadOnlyList<ProcedureParameter> Parameters { get; }

        internal Procedure(string name, Schema schema, List<ProcedureParameter> parameters)
            : base(name)
        {
            Schema = schema;
            Parameters = new List<ProcedureParameter>(parameters).AsReadOnly();
        }

        internal override string ToSqlFileContents()
        {
            var parameterList = string.Join(",\n", Parameters.Select(p => p.ToSqlString()));
            return $@"
                CREATE PROCEDURE [{Schema.Name}].[{Name}]
                    {parameterList}
                AS
                BEGIN
                    RETURN 0;
                END";
        }

        internal override FileInfo FilePath(string baseDir)
        {
            return new FileInfo(Path.Combine(baseDir, Schema.Name, "Procedures", FileSafeName) + ".sql");
        }

        public override string ToString()
        {
            return $"[{Schema?.Name}].[{Name}]";
        }
    }
}
