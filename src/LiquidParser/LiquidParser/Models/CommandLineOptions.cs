namespace LiquidParser.Models
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using CommandLine;
    using LiquidParser.Domain.Helpers;

    /// <summary>
    /// Class for command line options.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CommandlineOptions
    {
        /// <summary>
        /// Gets or sets the folder with documents.
        /// </summary>
        [Option('i', "input", Required = true, HelpText = "Folder containing the Parquet files for input.")]
        public string? InputFolder { get; set; }

        /// <summary>
        /// Gets or sets the folder with documents.
        /// </summary>
        [Option('t', "template", Required = true, HelpText = "Folder containing the templates.")]
        public string? TemplateFolder { get; set; }

        /// <summary>
        /// Gets or sets the contents to convert.
        /// </summary>
        [Option('c', "content", Required = true, HelpText = "Liquid content to convert.")]
        public string? Content { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether verbose information is shown in the output.
        /// </summary>
        [Option('v', "verbose", Required = false, HelpText = "Show verbose messages.")]
        public bool Verbose { get; set; }

        /// <summary>
        /// Check if options are valid.
        /// </summary>
        /// <returns>Valid true/false.</returns>
        public bool IsValid()
        {
            if (!Directory.Exists(InputFolder))
            {
                MessageHelper.Error($"ERROR: Input folder '{InputFolder}' doesn't exist.");
                return false;
            }

            // make sure path is full path
            InputFolder = Path.GetFullPath(InputFolder);

            if (!Directory.Exists(TemplateFolder))
            {
                MessageHelper.Error($"ERROR: Template folder '{TemplateFolder}' doesn't exist.");
                return false;
            }

            // make sure path is full path
            TemplateFolder = Path.GetFullPath(TemplateFolder);

            // base64 decode content
            if (Content != null)
            {
                Content = Encoding.UTF8.GetString(Convert.FromBase64String(Content));
            }

            return true;
        }

        /// <summary>
        /// Write verbose of this class to the output.
        /// </summary>
        public void WriteVerbose()
        {
            MessageHelper.Verbose($"Input folder     : {InputFolder}");
            MessageHelper.Verbose($"Verbose          : {Verbose}");
        }
    }
}
