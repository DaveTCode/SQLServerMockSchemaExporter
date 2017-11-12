namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Represents the common elements between all single fields (like a 
    /// parameter or a table/view column)
    /// </summary>
    abstract class SingleDataItem
    {
        internal string Name { get; }

        internal string DbType { get; }

        internal int? MaxCharacterLength { get; }

        internal SingleDataItem(string name, string dbType, int? maxCharacterLength)
        {
            Name = name;
            DbType = dbType;
            MaxCharacterLength = maxCharacterLength;
        }

        /// <summary>
        /// Calculate the sql string required to create this column in the database.
        /// </summary>
        internal virtual string ToSqlString()
        {
            var sqlString = $"[{Name}] {DbType}";
            if (MaxCharacterLength.HasValue)
            {
                sqlString += $"({(MaxCharacterLength == -1 ? "MAX" : MaxCharacterLength.Value.ToString())})";
            }

            return sqlString;
        }

        public override string ToString()
        {
            return ToSqlString();
        }
    }
}
