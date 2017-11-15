using System.Collections.Generic;
using System.Linq;

namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Represents a single function in the database.
    /// </summary>
    internal class TableValueFunction : Function
    {
        internal List<Column> ReturnColumns { get; }

        internal TableValueFunction(string name, Schema schema, List<Column> returnColumns, List<ProcedureParameter> parameters)
            : base(name, schema, parameters)
        {
            ReturnColumns = returnColumns;
        }

        internal override string ReturnSqlString()
        {
            return $@"@RtnValue TABLE
            (
                {string.Join(",", ReturnColumns.Select(c => c.ToSqlString()))}
            )";
        }

        internal override string ReturnValue()
        {
            return "";
        }
    }
}
