using SQLServerSchemaExporter.Base.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SQLServerSchemaExporter.Base.DB
{
    internal class DBLayer
    {
        private readonly string _connectionString;

        /// <summary>
        /// Create a db layer using windows authentication against the 
        /// specific server/database.
        /// 
        /// Note that the user that this application runs as must then have 
        /// db_owner level permissions on the specified database.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="db"></param>
        internal DBLayer(string server, string db)
        {
            _connectionString = $"Server={server};Database={db};Trusted_Connection=True;";
        }

        /// <summary>
        /// Create a db layer using a specified SQL username and password.
        /// 
        /// Note that the user must have db_owner level permissions on the
        /// specified database. 
        /// 
        /// @@@TODO - what are the real minimum requirements here?
        /// </summary>
        /// <param name="server">The SQL Server name/address</param>
        /// <param name="db">The database we are extracting</param>
        /// <param name="user">The login on the SQL Server</param>
        /// <param name="pass">The password for the user</param>
        internal DBLayer(string server, string db, string user, string pass)
        {
            _connectionString = $"Server={server};Database={db};User Id={user};Password={pass};";
        }

        /// <summary>
        /// For a given table this returns the list of columns in that table
        /// in a form that means they can be reconstructed.
        /// </summary>
        /// <param name="schema">The schema where the table lives</param>
        /// <param name="table">The table name in the database</param>
        /// <returns>A list of columns. Never null.</returns>
        private List<Column> GetTableColumns(Schema schema, string table)
        {
            var columns = new List<Column>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand($@"
                SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE, COLUMN_DEFAULT
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_NAME = @table AND TABLE_SCHEMA = @schema", connection))
            {
                command.Parameters.AddWithValue("@table", table);
                command.Parameters.AddWithValue("@schema", schema.Name);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columns.Add(new Column(
                            name: reader.GetString(0),
                            dbType: reader.GetString(1),
                            maxCharacterLength: reader.IsDBNull(2) ? (int?) null : reader.GetInt32(2),
                            isNullable: reader.GetString(3) == "NO",
                            columnDefault: reader.IsDBNull(4) ? null : reader.GetString(4)
                        ));
                    }
                }
            }

            return columns;
        }

        /// <summary>
        /// Get the full set of tables and views in the specified schema
        /// along with all column definitions.
        /// </summary>
        /// <param name="schema">Only returns tables in this schema</param>
        /// <returns>The list of tables. Never null.</returns>
        internal List<TableOrView> GetTablesAndViews(Schema schema)
        {
            var tables = new List<TableOrView>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand($@"
                SELECT TABLE_NAME
                FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_SCHEMA = @schema", connection))
            {
                command.Parameters.AddWithValue("@schema", schema.Name);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tableName = reader.GetString(0);

                        tables.Add(new TableOrView(
                            name: tableName, 
                            schema: schema,
                            columns: GetTableColumns(schema, tableName)
                        ));
                    }
                }
            }

            return tables;
        }

        /// <summary>
        /// Get a list of all the schema in th database.
        /// 
        /// @@@TODO - This will currently return schemas that we don't want to recreate (e.g. db_owner)
        /// </summary>
        /// <returns></returns>
        internal List<Schema> GetSchemas()
        {
            var schemas = new List<Schema>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand($@"
                SELECT SCHEMA_NAME 
                FROM INFORMATION_SCHEMA.SCHEMATA", connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        schemas.Add(new Schema(
                            name: reader.GetString(0)
                        ));
                    }
                }
            }

            return schemas;
        }

        /// <summary>
        /// For a given stored procedure this returns the list of parameters
        /// in a form that means they can be reconstructed.
        /// </summary>
        /// <param name="schema">The schema where the procedure lives</param>
        /// <param name="table">The procedures name in the database</param>
        /// <returns>A list of 0-n parameters. Never null.</returns>
        private List<ProcedureParameter> GetProcedureParameters(Schema schema, string procedureName)
        {
            var schemas = new List<ProcedureParameter>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand($@"
                SELECT PARAMETER_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, PARAMETER_MODE, 
                       USER_DEFINED_TYPE_SCHEMA, USER_DEFINED_TYPE_NAME
                FROM   INFORMATION_SCHEMA.PARAMETERS
                WHERE  SPECIFIC_NAME = @procedureName AND SPECIFIC_SCHEMA = @schema", connection))
            {
                command.Parameters.AddWithValue("@procedureName", procedureName);
                command.Parameters.AddWithValue("@schema", schema.Name);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var dbType = reader.GetString(1);
                        var isReadonly = false;

                        // Table type parameters have the parameter type stored in a different column
                        if (dbType == "table type")
                        {
                            dbType = $"{reader.GetString(4)}.{reader.GetString(5)}";
                            isReadonly = true;
                        }

                        schemas.Add(new ProcedureParameter(
                            name: reader.GetString(0),
                            dbType: dbType,
                            maxCharacterLength: reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                            isOutput: reader.GetString(3) != "IN",
                            isReadonly: isReadonly
                        ));
                    }
                }
            }

            return schemas;
        }

        /// <summary>
        /// Get a list of all stored procedures in a specified schema of a 
        /// database.
        /// </summary>
        /// <param name="schema">The schema to search</param>
        /// <returns>A list of 0-n procedures, never null</returns>
        internal List<Procedure> GetStoredProcedures(Schema schema)
        {
            var procedures = new List<Procedure>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand($@"
                SELECT ROUTINE_NAME
                FROM   INFORMATION_SCHEMA.ROUTINES
                WHERE  ROUTINE_TYPE = 'PROCEDURE' AND ROUTINE_SCHEMA = @schema", connection))
            {
                command.Parameters.AddWithValue("@schema", schema.Name);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var procedureName = reader.GetString(0);

                        procedures.Add(new Procedure(
                            name: procedureName,
                            schema: schema,
                            parameters: GetProcedureParameters(schema, procedureName)
                        ));
                    }
                }
            }

            return procedures;
        }

        /// <summary>
        /// Get the settings which are defined on a per database level.
        /// </summary>
        /// <returns>A settings object, never null.</returns>
        internal Settings GetDbSettings()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(@"SELECT CONVERT(VARCHAR, SERVERPROPERTY('collation'))", connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    reader.Read();
                    var collation = reader.GetString(0);

                    return new Settings(collation);
                }
            }
        }

        internal List<TableType> GetTableTypes(Schema schema)
        {
            var tableTypes = new List<TableType>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(@"
                SELECT DOMAIN_NAME
                FROM INFORMATION_SCHEMA.DOMAINS
                WHERE DATA_TYPE = 'table type' AND DOMAIN_SCHEMA = @schema
                ", connection))
            {
                command.Parameters.AddWithValue("@schema", schema.Name);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tableName = reader.GetString(0);

                        tableTypes.Add(new TableType(
                            name: tableName,
                            schema: schema,
                            columns: GetTableTypeColumns(tableName)
                        ));
                    }
                }
            }

            return tableTypes;
        }

        private List<Column> GetTableTypeColumns(string tableTypeName)
        {
            var columns = new List<Column>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(@"
                SELECT c.name ,
                       st.name ,
                       COLUMNPROPERTY(c.object_id, c.name, 'charmaxlen') ,
                       c.is_nullable
                FROM   sys.table_types AS tt
                    INNER JOIN sys.columns AS c ON c.object_id = tt.type_table_object_id
                    INNER JOIN sys.systypes AS ST ON ST.xtype = c.system_type_id
                WHERE  tt.name = @tableTypeName;
                ", connection))
            {
                command.Parameters.AddWithValue("@tableTypeName", tableTypeName);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var procedureName = reader.GetString(0);

                        columns.Add(new Column(
                            name: reader.GetString(0),
                            dbType: reader.GetString(1),
                            maxCharacterLength: reader.IsDBNull(2) ? (int?) null : reader.GetInt32(2),
                            isNullable: reader.GetBoolean(3),
                            columnDefault: null
                        ));
                    }
                }
            }

            return columns;
        }
    }
}
