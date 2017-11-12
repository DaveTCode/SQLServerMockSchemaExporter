using SQLServerSchemaExporter.Base.Models;
using System.IO;

namespace SQLServerSchemaExporter.Base
{
    /// <summary>
    /// Provides a public interface for writing the entirety of the database 
    /// schema out to a directory of the callers choice.
    /// 
    /// This output is the same form as expected by SSDT when importing a
    /// database.
    /// </summary>
    public class MockSchemaWriter
    {
        private readonly string _outputDirectory;
        private readonly bool _overwriteExistingFiles;

        public MockSchemaWriter(string outputDirectory, bool overwriteExistingFiles)
        {
            _outputDirectory = outputDirectory;
            _overwriteExistingFiles = overwriteExistingFiles;
        }

        public void WriteDatabaseSchema(Database database)
        {
            var securityDirectory = Path.Combine(_outputDirectory, "Security");
            Directory.CreateDirectory(securityDirectory);

            foreach (var schema in database.Schemas)
            {
                var schemaDirectory = Path.Combine(_outputDirectory, schema.Name);
                var schemaDefinitionPath = Path.Combine(securityDirectory, schema.Name) + ".sql";
                Directory.CreateDirectory(Path.Combine(schemaDirectory, "Tables"));
                Directory.CreateDirectory(Path.Combine(schemaDirectory, "Procedures"));

                if (!File.Exists(schemaDefinitionPath) || _overwriteExistingFiles)
                {
                    using (var stream = File.CreateText(schemaDefinitionPath))
                    {
                        stream.WriteLine(schema.ToSqlString());
                    }
                }
            }

            foreach (var tableOrView in database.TablesAndViews)
            {
                var tableDefinitionPath = Path.Combine(_outputDirectory, tableOrView.Schema.Name, "Tables", tableOrView.Name) + ".sql";
                if (!File.Exists(tableDefinitionPath) || _overwriteExistingFiles)
                {
                    using (var stream = File.CreateText(tableDefinitionPath))
                    {
                        stream.Write(tableOrView.ToSqlString());
                    }
                }
            }

            foreach (var procedure in database.StoredProcedures)
            {
                var procedureDefinitionPath = Path.Combine(_outputDirectory, procedure.Schema.Name, "Procedures", procedure.Name) + ".sql";
                if (!File.Exists(procedureDefinitionPath) || _overwriteExistingFiles)
                {
                    using (var stream = File.CreateText(procedureDefinitionPath))
                    {
                        stream.Write(procedure.ToSqlString());
                    }
                }
            }
        }
    }
}
