namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Represents a stored procedure parameter
    /// </summary>
    internal class ProcedureParameter
    {
        private string Name { get; }

        private Type Type { get; }
        
        private bool IsReadonly { get; }

        internal bool IsOutput { get; }

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
