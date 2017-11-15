using System.Collections.Generic;
using System.Linq;

namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Represents a single function in the database.
    /// </summary>
    internal abstract class Function
    {
        internal string Name { get; }

        internal Schema Schema { get; }

        internal IReadOnlyList<ProcedureParameter> Parameters { get; }

        internal Function(string name, Schema schema, List<ProcedureParameter> parameters)
        {
            Name = name;
            Schema = schema;
            Parameters = new List<ProcedureParameter>(parameters).AsReadOnly();
        }

        internal abstract string ReturnSqlString();

        internal abstract string ReturnValue();

        internal string ToSqlString()
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
    }
}
