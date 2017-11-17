using System.Collections.Generic;
using System.Linq;

namespace SQLServerSchemaExporter.Base.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a single function in the database.
    /// </summary>
    internal class TableValueFunction : Function
    {
        private List<Column> ReturnColumns { get; }

        internal TableValueFunction(string name, Schema schema, List<Column> returnColumns, List<ProcedureParameter> parameters)
            : base(name, schema, parameters)
        {
            ReturnColumns = returnColumns;
        }

        protected override string ReturnSqlString()
        {
            if (ReturnColumns.Any())
            {
                return $@"@RtnValue TABLE
                (
                    {string.Join(",", ReturnColumns.Select(c => c.ToSqlString()))}
                )";
            }
            else
            {
                return " TABLE";
            }
        }

        protected override string ReturnValue()
        {
            if (ReturnColumns.Any())
            {
                return "";
            }
            else
            {
                return "SELECT 1 AS 'a'";
            }
        }
    }
}
