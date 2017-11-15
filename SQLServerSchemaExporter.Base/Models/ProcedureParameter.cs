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
            :base (name, dbType, maxCharacterLength)
        {
            IsOutput = isOutput;
            IsReadonly = isReadonly;
        }

        internal override string ToSqlString()
        {
            var sqlString = base.ToSqlString();

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
