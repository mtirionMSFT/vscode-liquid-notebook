namespace LiquidParser
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using CommandLine;
    using LiquidParser.Domain.Helpers;
    using LiquidParser.Domain.Models;
    using LiquidParser.Models;
    using LiquidParser.Services;

    /// <summary>
    /// Main entry class for the console application.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class Program
    {
        private static int returnvalue;

        /// <summary>
        /// Main entry point of the application.
        /// </summary>
        /// <param name="args">Commandline parameters.</param>
        private static int Main(string[] args)
        {
            OSPlatformHelper.EnsureSupportedOSPlatforms();

            Parser.Default.ParseArguments<CommandlineOptions>(args)
                   .WithParsed<CommandlineOptions>(RunLogic)
                   .WithNotParsed(HandleErrors);

            return returnvalue;
        }

        /// <summary>
        /// Handle errors in commandline parsing.
        /// </summary>
        /// <param name="errors">List of errors.</param>
        private static void HandleErrors(IEnumerable<Error> errors)
        {
            returnvalue = 1;
        }

        /// <summary>
        /// The main logic of the console application.
        /// </summary>
        /// <param name="options">Commandline options.</param>
        private static void RunLogic(CommandlineOptions options)
        {
            if (options.Verbose)
            {
                options.WriteVerbose();
            }

            if (!options.IsValid())
            {
                returnvalue = 1;
                return;
            }

            try
            {
                Model model = new Model();

                // read all parquet files first
                ParquetService parquet = new ParquetService();
                CSVService csv = new CSVService();

                if (options.InputFolder != null)
                {
                    parquet.ImportFiles(model, options.InputFolder);
                    csv.ImportFiles(model, options.InputFolder);
                }

                ParserService parser = new ParserService();
                string output = string.Empty;
                if (options.Content != null)
                {
                    try
                    {
                        output = parser.Render(model, options.Content, options.TemplateFolder);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex.Message);
                    }
                }

                Console.WriteLine(output);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                returnvalue = 1;
                return;
            }
        }
    }
}
