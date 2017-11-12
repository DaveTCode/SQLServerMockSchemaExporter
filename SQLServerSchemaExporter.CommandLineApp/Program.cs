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
            });
        }
    }
}
