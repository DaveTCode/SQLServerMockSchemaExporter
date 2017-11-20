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
            // Iterate over each of the entries in the database schema and 
            // write them to file ensuring that the directory exists.
            foreach (var writableEntry in database.GetWritableEntries())
            {
                var filePath = writableEntry.FilePath(_outputDirectory);
                Directory.CreateDirectory(filePath.Directory.FullName);

                if ((!filePath.Exists || _overwriteExistingFiles))
                {
                    using (var stream = filePath.CreateText())
                    {
                        stream.WriteLine(writableEntry.ToSqlFileContents());
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

            // TODO - Combine the following 3 queries into a single xpath expression. Possible?
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

            // Add a wildcard entry for each schema in the database
            // We don't add a single wildcard entry because it will catch the
            // built sql file in bin/*.sql
            foreach(XmlNode directoriesNode in document.SelectNodes("/ns:Project/ns:ItemGroup", manager))
            {
                foreach(var schema in database.Schemas)
                {
                    var element = document.CreateElement("Build", document.DocumentElement.NamespaceURI);
                    element.SetAttribute("Include", $"{schema.Name}\\**\\*.sql");
                    directoriesNode.AppendChild(element);
                }
            }

            document.Save(Path.Combine(_outputDirectory, database.Name) + ".sqlproj");
        }
    }
}
