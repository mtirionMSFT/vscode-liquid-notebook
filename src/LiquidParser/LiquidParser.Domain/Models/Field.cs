namespace LiquidParser.Domain.Models
{
    using LiquidParser.Domain.Helpers;

    /// <summary>
    /// Field definition.
    /// </summary>
    public class Field
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Field"/> class.
        /// </summary>
        /// <param name="table">Parent <see cref="Table"/>.</param>
        /// <param name="name">Name of the field.</param>
        public Field(Table table, string name)
        {
            Precondition.NotNull(table);
            Precondition.Requires(!string.IsNullOrWhiteSpace(name));

            Table = table;
            Name = name;
            Value = string.Empty;
        }

        /// <summary>
        /// Gets the table.
        /// </summary>
        public Table Table { get; private set; }

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the value of the field.
        /// </summary>
        public object Value { get; set; } = string.Empty;
    }
}
