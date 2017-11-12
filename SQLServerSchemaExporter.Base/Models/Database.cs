using System.Collections.Generic;

namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Top level model which represents the entire schema of the database.
    /// </summary>
    public class Database
    {
        internal IReadOnlyCollection<TableOrView> TablesAndViews { get; }

        internal IReadOnlyCollection<Procedure> StoredProcedures { get; }

        internal IReadOnlyCollection<Schema> Schemas { get; }
        
        internal Database(IEnumerable<Schema> schemas,
            IEnumerable<TableOrView> tablesAndviews, 
            IEnumerable<Procedure> procedures)
        {
            Schemas = new List<Schema>(schemas).AsReadOnly();
            TablesAndViews = new List<TableOrView>(tablesAndviews).AsReadOnly();
            StoredProcedures = new List<Procedure>(procedures).AsReadOnly();
        }
    }
}
