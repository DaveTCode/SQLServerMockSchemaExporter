using System.Collections.Generic;
using System.Linq;

namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Represents a single function in the database.
    /// </summary>
    internal class ScalarFunction : Function
    {
        internal Type ReturnType { get; }

        internal ScalarFunction(string name, Schema schema, Type returnType, List<ProcedureParameter> parameters)
            : base(name, schema, parameters)
        {
            ReturnType = returnType;
        }

        internal override string ReturnSqlString()
        {
            return ReturnType.ToSqlString();
        }

        internal override string ReturnValue()
        {
            return "NULL";
        }
    }
}
