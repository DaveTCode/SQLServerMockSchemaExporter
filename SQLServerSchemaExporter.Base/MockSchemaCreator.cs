using SQLServerSchemaExporter.Base.DB;
using SQLServerSchemaExporter.Base.Models;
using System.Linq;

namespace SQLServerSchemaExporter.Base
{
    /// <summary>
    /// Provides a public interface for retrieving the schema of a database 
    /// for inspection of writing out using MockSchemaWriter.
    /// </summary>
    public class MockSchemaCreator
    {
        private readonly DBLayer _dbLayer;

        public MockSchemaCreator(string server, string db, string username, string password)
        {
            _dbLayer = (username == null) ? new DBLayer(server, db) : new DBLayer(server, db, username, password);
        }

        public Database CreateMockSchema()
        {
            var schemas = _dbLayer.GetSchemas();
            var tables = schemas.SelectMany(schema => _dbLayer.GetTablesAndViews(schema));
            var procedures = schemas.SelectMany(schema => _dbLayer.GetStoredProcedures(schema));

            return new Database(schemas, tables, procedures);
        }
    }
}
