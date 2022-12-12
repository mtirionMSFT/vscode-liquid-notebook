namespace LiquidParser.Domain.Converters
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Fluid;
    using Fluid.Values;
    using LiquidParser.Domain.Helpers;
    using LiquidParser.Domain.Models;

    /// <summary>
    /// Fluid Object Converter for <see cref="Fields"/> class.
    /// </summary>
    public class JsonDocumentsConverter : ObjectValueBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDocumentsConverter"/> class.
        /// </summary>
        /// <param name="value"><see cref="Fields"/> object.</param>
        public JsonDocumentsConverter(object value)
            : base(value)
        {
        }

        /// <inheritdoc/>
        public override ValueTask<FluidValue> GetValueAsync(string name, TemplateContext context)
        {
            Precondition.NotNull(name);
            Precondition.NotNull(context);

            JsonDocuments? obj = Value as JsonDocuments;
            if (obj != null)
            {
                switch (name)
                {
                    case "size":
                        return Create(obj.Documents.Count, context.Options);
                    case "first":
                        return Create(obj.Documents.FirstOrDefault(), context.Options);
                    case "last":
                        return Create(obj.Documents.LastOrDefault(), context.Options);
                }

                string lowIndex = name.ToLower();
                if (obj.Documents.TryGetValue(lowIndex, out dynamic? value))
                {
                    return Create(value, context.Options);
                }

                // nothing else, so use the name as index to the fields collection.
                return Create(obj[name], context.Options);
            }

            return Create(null, context.Options);
        }

        /// <summary>
        /// Handle an index on this class.
        /// </summary>
        /// <param name="index">Index value.</param>
        /// <param name="context">Template context.</param>
        /// <returns>A <see cref="Table"/> with the given name.</returns>>
        public override ValueTask<FluidValue> GetIndexAsync(FluidValue index, TemplateContext context)
        {
            Precondition.NotNull(index);
            Precondition.NotNull(context);

            JsonDocuments? obj = Value as JsonDocuments;

            if (obj != null)
            {
                return Create(obj[index.ToStringValue()], context.Options);
            }

            return Create(null, context.Options);
        }

        /// <inheritdoc/>
        public override IEnumerable<FluidValue> Enumerate(TemplateContext context)
        {
            Precondition.NotNull(context);
            JsonDocuments? docs = Value as JsonDocuments;
            List<FluidValue> list = new List<FluidValue>();

            /*
            //if (docs != null)
            //{
            //    foreach (dynamic doc in docs)
            //    {
            //        list.Add(FluidValue.Create(doc, context.Options));
            //    }
            //}
            */

            return list;
        }
    }
}
