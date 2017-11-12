using Microsoft.Extensions.CommandLineUtils;
using System;

namespace SQLServerSchemaExporter.CommandLineApp
{
    class Program
    {
        static void Main(string[] args)
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
                        command.ShowHelp();
                        return -1;
                    }

                    // Check mandatory parameters or exit with help displayed
                    if (serverOption.HasValue() && dbOption.HasValue() && outputDirectoryOption.HasValue())
                    {
                        // TODO - Delegate responsibility to the rest of the app
                    }
                    else
                    {
                        command.ShowHelp();
                        return -1;
                    }

                    return 0;
                });
            });
        }
    }
}
