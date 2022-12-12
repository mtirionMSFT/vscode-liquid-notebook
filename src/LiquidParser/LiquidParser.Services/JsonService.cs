namespace LiquidParser.Services
{
    using System.Dynamic;
    using System.IO;
    using LiquidParser.Domain.Helpers;
    using LiquidParser.Domain.Models;
    using Newtonsoft.Json;

    /// <summary>
    /// The service to read the CSV files.
    /// </summary>
    public class JsonService
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
        /// Import all Json files in the given folder.
        /// </summary>
        /// <param name="documents">Documents collection to add JsonDocument to.</param>
        /// <param name="folder">Folder containing all the related CSV files.</param>
        public void ImportFiles(JsonDocuments documents, string folder)
        {
            Precondition.NotNull(documents);
            Precondition.NotNull(folder);

            // search is case-insensitive by default
            string[] files = Directory.GetFiles(folder, "*.json");
            foreach (string file in files)
            {
                ImportingFile?.Invoke(file);

                string json = File.ReadAllText(file);
                dynamic? obj = JsonConvert.DeserializeObject<ExpandoObject>(json);
                if (obj != null)
                {
                    // add document with sanitized filename as index name
                    documents.Documents.Add(
                        PathHelpers.SanitizeFilenameToTablename(Path.GetFileNameWithoutExtension(file)),
                        obj);
                }
            }
        }
    }
}
