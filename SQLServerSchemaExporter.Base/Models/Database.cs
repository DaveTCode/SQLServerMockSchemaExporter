using System.Collections.Generic;

namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Top level model which represents the entire schema of the database.
    /// </summary>
    public class Database
    {
        internal string Name { get; }

        internal Settings Settings { get; }

        internal IReadOnlyCollection<TableOrView> TablesAndViews { get; }

        internal IReadOnlyCollection<Procedure> StoredProcedures { get; }

        internal IReadOnlyCollection<Schema> Schemas { get; }

        internal IReadOnlyCollection<TableType> TableTypes { get; }

        internal IReadOnlyCollection<Function> Functions { get; }
        
        internal Database(string name,
            Settings settings,
            IEnumerable<Schema> schemas,
            IEnumerable<TableOrView> tablesAndviews, 
            IEnumerable<Procedure> procedures,
            IEnumerable<TableType> tableTypes,
            IEnumerable<Function> functions)
        {
            Name = name;
            Settings = settings;
            Schemas = new List<Schema>(schemas).AsReadOnly();
            TablesAndViews = new List<TableOrView>(tablesAndviews).AsReadOnly();
            StoredProcedures = new List<Procedure>(procedures).AsReadOnly();
            TableTypes = new List<TableType>(tableTypes).AsReadOnly();
            Functions = new List<Function>(functions).AsReadOnly();
        }
    }
}
