using SQLServerSchemaExporter.Base.Models;
using Xunit;

namespace SQLServerSchemaExporter.Tests.Models
{
    public class ColumnTests
    {
        [Theory]
        [InlineData("hello", "int", null, "[hello] int")]
        [InlineData("hello", "INT", null, "[hello] INT")]
        [InlineData("hello", "VARCHAR", -1, "[hello] VARCHAR(MAX)")]
        public void TestBasicSqlString(string name, string type, int? maxCharacterLength, string result)
        {
            var column = new Column(name, new Type(type, maxCharacterLength), true, null);
            Assert.Equal(result, column.ToSqlString());
        }

        [Theory]
        [InlineData("hello", "int", null, "[hello] int NOT NULL")]
        [InlineData("hello", "INT", null, "[hello] INT NOT NULL")]
        [InlineData("hello", "VARCHAR", -1, "[hello] VARCHAR(MAX) NOT NULL")]
        public void TestNonNull(string name, string type, int? maxCharacterLength, string result)
        {
            var column = new Column(name, new Type(type, maxCharacterLength), false, null);
            Assert.Equal(result, column.ToSqlString());
        }

        [Theory]
        [InlineData("hello", "int", null, "1", "[hello] int NOT NULL DEFAULT (1)")]
        [InlineData("hello", "INT", null, "NULL", "[hello] INT NOT NULL DEFAULT (NULL)")]
        [InlineData("hello", "VARCHAR", -1, "'what'", "[hello] VARCHAR(MAX) NOT NULL DEFAULT ('what')")]
        public void TestDefaultValue(string name, string type, int? maxCharacterLength, string defaultValue, string result)
        {
            var column = new Column(name, new Type(type, maxCharacterLength), false, defaultValue);
            Assert.Equal(result, column.ToSqlString());
        }
    }
}
