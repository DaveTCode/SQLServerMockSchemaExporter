using SQLServerSchemaExporter.Base.DB;
using SQLServerSchemaExporter.Base.Models;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("SQLServerSchemaExporter.Tests")]
namespace SQLServerSchemaExporter.Base
{
    /// <summary>
    /// Provides a public interface for retrieving the schema of a database 
    /// for inspection of writing out using MockSchemaWriter.
    /// </summary>
    public class MockSchemaCreator
    {
        private readonly DbLayer _dbLayer;

        private readonly string _dbName;

        public MockSchemaCreator(string server, string db, string username, string password)
        {
            _dbName = db;
            _dbLayer = (username == null) ? new DbLayer(server, db) : new DbLayer(server, db, username, password);
        }

        public Database CreateMockSchema()
        {
            var databaseSettings = _dbLayer.GetDbSettings();
            var schemas = _dbLayer.GetSchemas();
            var tables = schemas.SelectMany(schema => _dbLayer.GetTablesAndViews(schema));
            var tableTypes = schemas.SelectMany(schema => _dbLayer.GetTableTypes(schema));
            var procedures = schemas.SelectMany(schema => _dbLayer.GetStoredProcedures(schema));
            var functions = schemas.SelectMany(schema => _dbLayer.GetFunctions(schema));

            return new Database(_dbName, databaseSettings, schemas, tables, procedures, tableTypes, functions);
        }
    }
}
