namespace LiquidParser.Services
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.IO;
    using LiquidParser.Domain.Helpers;
    using LiquidParser.Domain.Models;
    using Newtonsoft.Json;

    /// <summary>
    /// The service to read the CSV files.
    /// </summary>
    public class EnvFileService
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
        /// <param name="envfiles">Env files collection to add settings to.</param>
        /// <param name="folder">Folder containing all the related env files.</param>
        public void ImportFiles(EnvFiles envfiles, string folder)
        {
            Precondition.NotNull(envfiles);
            Precondition.NotNull(folder);

            // search is case-insensitive by default
            string[] files = Directory.GetFiles(folder, "*.env");
            foreach (string file in files)
            {
                ImportingFile?.Invoke(file);

                string[] lines = File.ReadAllLines(file);
                EnvFile env = new EnvFile();
                foreach (string line in lines)
                {
                    if (line.Contains("="))
                    {
                        int pos = line.IndexOf('=');
                        string key = line.Substring(0, pos);
#pragma warning disable SA1122 // Use string.Empty for empty strings
                        string value = "";
#pragma warning restore SA1122 // Use string.Empty for empty strings
                        if (pos + 1 < line.Length)
                        {
                            value = line.Substring(pos + 1).Trim();
                        }

                        if (!env.Settings.ContainsKey(key))
                        {
                            env.Settings.Add(key, value);
                        }
                    }
                }

                string filename = PathHelpers.SanitizeFilenameToTablename(Path.GetFileNameWithoutExtension(file));
                if (string.IsNullOrEmpty(filename))
                {
                    filename = "env";
                }

                envfiles.Add(filename, env);
            }
        }
    }
}
