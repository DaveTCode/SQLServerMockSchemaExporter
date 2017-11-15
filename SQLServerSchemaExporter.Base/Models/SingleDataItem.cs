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

        public override string ToString()
        {
            return Name;
        }
    }
}
