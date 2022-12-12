namespace LiquidParser.Domain.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Collection of Json documents, indexed by a unique name.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class JsonDocuments
    {
        private readonly Dictionary<string, dynamic> _documents = new Dictionary<string, dynamic>();

        /// <summary>
        /// Gets the documents collection.
        /// </summary>
        public Dictionary<string, dynamic> Documents => _documents;

        /// <summary>
        /// Indexer to return Json document with given name or null when not found.
        /// </summary>
        /// <param name="name">Name of the Json document.</param>
        /// <returns>dynamic object or null.</returns>
        public dynamic? this[string name] => _documents[name];
    }
}
