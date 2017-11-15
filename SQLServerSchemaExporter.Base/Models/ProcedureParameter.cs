namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Represents a stored procedure parameter
    /// </summary>
    internal class ProcedureParameter
    {
        internal string Name { get; }

        internal Type Type { get; }

        internal bool IsOutput { get; }

        internal bool IsReadonly { get; }

        internal ProcedureParameter(string name, Type type, bool isOutput, bool isReadonly)
        {
            Name = name;
            Type = type;
            IsOutput = isOutput;
            IsReadonly = isReadonly;
        }

        internal string ToSqlString()
        {
            var sqlString = $"{Name} {Type.ToSqlString()}";

            if (IsOutput)
            {
                sqlString += " OUTPUT";
            }

            if (IsReadonly)
            {
                sqlString += " READONLY";
            }

            return sqlString;
        }
    }
}
