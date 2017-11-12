using System.Collections.Generic;
using System.Linq;

namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Represents a single stored procedure in the database.
    /// </summary>
    class Procedure
    {
        internal string Name { get; }

        internal Schema Schema { get; }

        internal IReadOnlyList<ProcedureParameter> Parameters { get; }

        internal Procedure(string name, Schema schema, List<ProcedureParameter> parameters)
        {
            Name = name;
            Parameters = new List<ProcedureParameter>(parameters).AsReadOnly();
        }

        internal string ToSqlString()
        {
            var parameterList = string.Join(",\n", Parameters.Select(p => p.ToSqlString()));
            return $@"
                CREATE PROCEDURE [{Schema.Name}].[{Name}]
                    {parameterList}
                BEGIN
                    RETURN 0;
                END";
        }
    }
}
