using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Represents a single function in the database.
    /// </summary>
    internal abstract class Function : BaseWritableObject
    {
        internal Schema Schema { get; }

        private IReadOnlyList<ProcedureParameter> Parameters { get; }

        internal Function(string name, Schema schema, List<ProcedureParameter> parameters)
            : base(name)
        {
            Schema = schema;
            Parameters = new List<ProcedureParameter>(parameters).AsReadOnly();
        }

        protected abstract string ReturnSqlString();

        protected abstract string ReturnValue();

        internal override string ToSqlFileContents()
        {
            var parameterList = string.Join(",\n", Parameters.Where(p => !p.IsOutput).Select(p => p.ToSqlString()));
            return $@"
                CREATE FUNCTION [{Schema.Name}].[{Name}]
                    ({parameterList})
                RETURNS {ReturnSqlString()}
                AS
                BEGIN
                    RETURN {ReturnValue()};
                END";
        }

        internal override FileInfo FilePath(string baseDir)
        {
            return new FileInfo(Path.Combine(baseDir, Schema.Name, "Functions", FileSafeName) + ".sql");
        }
    }
}
