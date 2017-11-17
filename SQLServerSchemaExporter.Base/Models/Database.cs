using System.Collections.Generic;
using System.Linq;

namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Top level model which represents the entire schema of the database.
    /// </summary>
    public class Database
    {
        internal string Name { get; }

        internal Settings Settings { get; }

        internal IReadOnlyCollection<Schema> Schemas { get; }

        private IReadOnlyCollection<TableOrView> TablesAndViews { get; }

        private IReadOnlyCollection<Procedure> StoredProcedures { get; }

        private IReadOnlyCollection<TableType> TableTypes { get; }

        private IReadOnlyCollection<Function> Functions { get; }
        
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

        /// <summary>
        /// Get every entry in this database that can be described in a single 
        /// sql file.
        /// </summary>
        /// <returns>An enumerable iterating over the list of all
        /// entries.</returns>
        internal IEnumerable<BaseWritableObject> GetWritableEntries()
        {
            foreach (var tableOrView in TablesAndViews)
                yield return tableOrView;

            foreach (var procedure in StoredProcedures)
                yield return procedure;

            foreach (var schema in Schemas.Where(s => !s.IsDefaultSchema))
                yield return schema;

            foreach (var tableType in TableTypes)
                yield return tableType;

            foreach (var function in Functions)
                yield return function;
        }
    }
}
