using System.Collections.Generic;

namespace SQLServerSchemaExporter.Base.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a single function in the database.
    /// </summary>
    internal class ScalarFunction : Function
    {
        private Type ReturnType { get; }

        internal ScalarFunction(string name, Schema schema, Type returnType, List<ProcedureParameter> parameters)
            : base(name, schema, parameters)
        {
            ReturnType = returnType;
        }

        protected override string ReturnSqlString()
        {
            return ReturnType.ToSqlString();
        }

        protected override string ReturnValue()
        {
            return "NULL";
        }
    }
}
