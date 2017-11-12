namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Simple class representing a schema in the database.
    /// </summary>
    internal class Schema
    {
        internal string Name { get; }

        internal Schema(string name)
        {
            Name = name;
        }

        internal string ToSqlString()
        {
            return $"CREATE SCHEMA {Name}";
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
