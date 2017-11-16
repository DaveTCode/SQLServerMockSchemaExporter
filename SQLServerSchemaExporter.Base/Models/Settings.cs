namespace SQLServerSchemaExporter.Base.Models
{
    /// <summary>
    /// Top level model which represents any settings which are defined on the 
    /// database itself.
    /// </summary>
    public class Settings
    {
        internal string Collation { get; }

        internal Settings(string collation)
        {
            Collation = collation;
        }
    }
}
