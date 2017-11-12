namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Represents a stored procedure parameter
    /// </summary>
    internal class ProcedureParameter : SingleDataItem
    {
        internal bool IsOutput { get; }

        internal ProcedureParameter(string name, string dbType, int? maxCharacterLength, bool isOutput)
            :base (name, dbType, maxCharacterLength)
        {
            IsOutput = isOutput;
        }

        internal override string ToSqlString()
        {
            var sqlString = base.ToSqlString();

            if (IsOutput)
            {
                sqlString += " OUTPUT";
            }

            return sqlString;
        }
    }
}
