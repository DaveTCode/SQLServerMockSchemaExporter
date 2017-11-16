using SQLServerSchemaExporter.Base.Models;
using System.IO;
using System.Xml;
using System.Reflection;

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

            // TODO - refactor so we don't need to refer to each type in the database definition by hand.
            foreach (var schema in database.Schemas)
            {
                var schemaDirectory = Path.Combine(_outputDirectory, schema.Name);
                var schemaDefinitionPath = Path.Combine(securityDirectory, schema.Name) + ".sql";
                Directory.CreateDirectory(Path.Combine(schemaDirectory, "Tables"));
                Directory.CreateDirectory(Path.Combine(schemaDirectory, "Procedures"));
                Directory.CreateDirectory(Path.Combine(schemaDirectory, "TableTypes"));
                Directory.CreateDirectory(Path.Combine(schemaDirectory, "Functions"));

                if ((!File.Exists(schemaDefinitionPath) || _overwriteExistingFiles) && 
                    !schema.IsDefaultSchema) // The default schemas will already exist so must not be recreated
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

            foreach (var tableType in database.TableTypes)
            {
                var tableTypeDefinitionPath = Path.Combine(_outputDirectory, tableType.Schema.Name, "TableTypes", tableType.Name) + ".sql";
                if (!File.Exists(tableTypeDefinitionPath) || _overwriteExistingFiles)
                {
                    using (var stream = File.CreateText(tableTypeDefinitionPath))
                    {
                        stream.Write(tableType.ToSqlString());
                    }
                }
            }

            foreach (var function in database.Functions)
            {
                var tableTypeDefinitionPath = Path.Combine(_outputDirectory, function.Schema.Name, "Functions", function.Name) + ".sql";
                if (!File.Exists(tableTypeDefinitionPath) || _overwriteExistingFiles)
                {
                    using (var stream = File.CreateText(tableTypeDefinitionPath))
                    {
                        stream.Write(function.ToSqlString());
                    }
                }
            }

            WriteProjectFile(database);
        }

        private void WriteProjectFile(Database database)
        {
            var document = new XmlDocument();
            document.Load(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                "ProjectFileTemplate.sqlproj.tpl"));
            var navigator = document.CreateNavigator();
            var manager = new XmlNamespaceManager(navigator.NameTable);
            manager.AddNamespace("ns", "http://schemas.microsoft.com/developer/msbuild/2003");
            foreach(XmlNode nameNode in document.SelectNodes("/ns:Project/ns:PropertyGroup/ns:Name", manager))
            {
                nameNode.InnerText = database.Name;
            }

            foreach(XmlNode nameNode in document.SelectNodes("/ns:Project/ns:PropertyGroup/ns:RootNamespace", manager))
            {
                nameNode.InnerText = database.Name;
            }

            foreach(XmlNode nameNode in document.SelectNodes("/ns:Project/ns:PropertyGroup/ns:AssemblyName", manager))
            {
                nameNode.InnerText = database.Name;
            }

            foreach(XmlNode nameNode in document.SelectNodes("/ns:Project/ns:PropertyGroup/ns:DefaultCollation", manager))
            {
                nameNode.InnerText = database.Settings.Collation;
            }

            document.Save(Path.Combine(_outputDirectory, database.Name) + ".sqlproj");
        }
    }
}
