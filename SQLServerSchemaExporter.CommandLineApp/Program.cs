using Microsoft.Extensions.CommandLineUtils;
using NLog;
using SQLServerSchemaExporter.Base;

namespace SQLServerSchemaExporter.CommandLineApp
{
    internal static class Program
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.Command("extract", (command) =>
            {
                command.Description = "Extract a mock schema from the database specified";
                command.HelpOption("-?|-h|--help");
                var serverOption = command.Option("-s|--server", "The SQL Server where the database lives", CommandOptionType.SingleValue);
                var dbOption = command.Option("-d|--db", "The name of the database on the server", CommandOptionType.SingleValue);
                var userOption = command.Option("-u|--user", "The SQL user if not using windows authentication", CommandOptionType.SingleValue);
                var passOption = command.Option("-p|--pass", "The password for the SQL user if not using windows authentication", CommandOptionType.SingleValue);
                var outputDirectoryOption = command.Option("-o|--output", "The directory in which to write the results", CommandOptionType.SingleValue);
                var overwriteExistingOption = command.Option("--overwrite", "Set this flag if you want to overwrite any existing files in that output directory", CommandOptionType.NoValue);

                command.OnExecute(() =>
                {
                    // Either user & pass or neither
                    if (userOption.HasValue() ^ passOption.HasValue())
                    {
                        Log.Warn("Attempted to call with either username or password but not both");
                        command.ShowHelp();
                        return -1;
                    }

                    // Check mandatory parameters or exit with help displayed
                    if (serverOption.HasValue() && dbOption.HasValue() && outputDirectoryOption.HasValue())
                    {
                        var mockSchemaCreator = new MockSchemaCreator(
                            server: serverOption.Value(),
                            db: dbOption.Value(),
                            username: userOption.HasValue() ? userOption.Value() : null,
                            password: passOption.HasValue() ? passOption.Value() : null);
                        var mockSchemaWriter = new MockSchemaWriter(
                            outputDirectory: outputDirectoryOption.Value(),
                            overwriteExistingFiles: overwriteExistingOption.HasValue());

                        var schema = mockSchemaCreator.CreateMockSchema();
                        mockSchemaWriter.WriteDatabaseSchema(schema);
                    }
                    else
                    {
                        command.ShowHelp();
                        return -1;
                    }

                    return 0;
                });

                command.Execute(args);
            });
        }
    }
}
