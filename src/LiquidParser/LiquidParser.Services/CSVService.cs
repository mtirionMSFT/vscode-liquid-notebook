namespace LiquidParser.Services
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using LiquidParser.Domain.Helpers;
    using LiquidParser.Domain.Models;

    /// <summary>
    /// The service to read the CSV files.
    /// </summary>
    public class CSVService
    {
        /// <summary>
        /// Delegate for notify caller which file is being read.
        /// </summary>
        /// <param name="filename">Filename being read.</param>
        public delegate void FileNotifierDelegate(string filename);

        /// <summary>
        /// Public event being fired when a file is imported.
        /// </summary>
        public event FileNotifierDelegate? ImportingFile;

        /// <summary>
        /// Import all CSV files in the given folder.
        /// </summary>
        /// <param name="model">Model to add data to.</param>
        /// <param name="folder">Folder containing all the related CSV files.</param>
        public void ImportFiles(Model model, string folder)
        {
            Precondition.NotNull(model);
            Precondition.NotNull(folder);

            // search is case-insensitive by default
            string[] files = Directory.GetFiles(folder, "*.csv");
            foreach (string file in files)
            {
                ImportingFile?.Invoke(file);

                string[] lines = File.ReadAllLines(file);

                Table? table = null;
                foreach (string line in lines)
                {
                    if (table == null)
                    {
                        // first line is taken as the header row with the field names
                        string[] fieldnames = line.Split(',');
                        table = model.Tables.Add(
                            PathHelpers.SanitizeFilenameToTablename(Path.GetFileNameWithoutExtension(file)),
                            fieldnames);
                    }
                    else
                    {
                        // rest of the lines are treated as records with fields
                        string[] values = line.Split(',');
                        table.Records.AddValues(values);
                    }
                }
            }
        }

        /// <summary>
        /// Export it.
        /// </summary>
        /// <param name="table">Table.</param>
        /// <param name="file">Filename.</param>
        [ExcludeFromCodeCoverage]
        public void ExportFile(Table table, string file)
        {
            Precondition.NotNull(table);
            Precondition.Requires(table.Records.Any());
            Precondition.NotNull(file);

            List<string> lines = new List<string>();

            // first the header line with field names
            lines.Add(string.Join(',', table.FieldNames));

            // now all the data
            foreach (Record record in table.Records)
            {
                lines.Add(string.Join(',', record.GetValues()));
            }

            // write the file
            File.WriteAllLines(file, lines);
        }
    }
}
