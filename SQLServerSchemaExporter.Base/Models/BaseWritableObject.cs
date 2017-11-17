using System.IO;
using System.Text.RegularExpressions;

namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// A class implementing this interface provides methods to write a SQL
    /// string which can be stored in a .sql file and executed against a 
    /// database.
    /// </summary>
    abstract class BaseWritableObject
    {
        /// <summary>
        /// The name of the object in SQL Server.
        /// </summary>
        internal string Name { get; }

        protected BaseWritableObject(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Removes and special characters from the name to provide a version 
        /// of the name that can be used for filenames.
        /// </summary>
        protected string FileSafeName
        {
            get
            {
                return Regex.Replace(Name, "[^a-zA-Z0-9_]+", "_", RegexOptions.Compiled);
            }
        }

        /// <summary>
        /// This function returns the contents of the sql file that represents
        /// the class.
        /// </summary>
        /// <returns>Never null. A sql string which should always be executable
        /// against a SQL Server database.</returns>
        internal abstract string ToSqlFileContents();

        /// <summary>
        /// The full file path where this file will be stored relative on the 
        /// system.
        /// </summary>
        /// <returns>Never null, an absolute path.</returns>
        internal abstract FileInfo FilePath(string baseDirectory);
    }
}
