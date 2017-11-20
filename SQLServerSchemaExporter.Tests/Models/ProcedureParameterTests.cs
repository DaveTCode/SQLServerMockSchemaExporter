using SQLServerSchemaExporter.Base.Models;
using Xunit;

namespace SQLServerSchemaExporter.Tests.Models
{
    public class ProcedureParameterTests
    {
        [Theory]
        [InlineData("hello", "int", null, true, true, "hello int OUTPUT READONLY")]
        [InlineData("hello", "INT", null, true, true, "hello INT OUTPUT READONLY")]
        [InlineData("hello", "VARCHAR", -1, true, true, "hello VARCHAR(MAX) OUTPUT READONLY")]
        [InlineData("hello", "VARCHAR", -1, true, false, "hello VARCHAR(MAX) OUTPUT")]
        [InlineData("hello", "VARCHAR", -1, false, true, "hello VARCHAR(MAX) READONLY")]
        [InlineData("hello", "VARCHAR", -1, false, false, "hello VARCHAR(MAX)")]
        public void TestTypeSqlString(string name, string type, int? maxCharacterLength, bool isOutput, bool isReadonly, string result)
        {
            var column = new ProcedureParameter(name, new Type(type, maxCharacterLength), isOutput, isReadonly);
            Assert.Equal(result, column.ToSqlString());
        }
    }
}
