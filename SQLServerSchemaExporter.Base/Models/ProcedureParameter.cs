namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Represents a stored procedure parameter
    /// </summary>
    internal class ProcedureParameter : SingleDataItem
    {
        internal bool IsOutput { get; }

        internal bool IsReadonly { get; }

        internal ProcedureParameter(string name, string dbType, int? maxCharacterLength, bool isOutput, bool isReadonly)
            : base (name, dbType, maxCharacterLength)
        {
            IsOutput = isOutput;
            IsReadonly = isReadonly;
        }

        internal string ToSqlString()
        {
            var sqlString = $"{Name} {DbType}";
            if (MaxCharacterLength.HasValue)
            {
                sqlString += $"({(MaxCharacterLength == -1 ? "MAX" : MaxCharacterLength.Value.ToString())})";
            }

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
