namespace LiquidParser.Domain.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Collection of settings, indexed by a unique name.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class EnvFile
    {
        private readonly Dictionary<string, string> _settings = new Dictionary<string, string>();

        /// <summary>
        /// Gets the settings collection.
        /// </summary>
        public Dictionary<string, string> Settings => _settings;

        /// <summary>
        /// Indexer to return the value for the setting with the given name or null when not found.
        /// </summary>
        /// <param name="name">Name of the setting.</param>
        /// <returns>value or null.</returns>
        public string this[string name] => _settings[name];
    }
}
