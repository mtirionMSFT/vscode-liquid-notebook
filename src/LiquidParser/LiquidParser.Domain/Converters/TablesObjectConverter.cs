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
    /// Fluid Object Converter for <see cref="Tables"/> class.
    /// </summary>
    public class TablesObjectConverter : ObjectValueBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TablesObjectConverter"/> class.
        /// </summary>
        /// <param name="value"><see cref="Tables"/> object.</param>
        public TablesObjectConverter(object value)
            : base(value)
        {
        }

        /// <inheritdoc/>
        public override ValueTask<FluidValue> GetValueAsync(string name, TemplateContext context)
        {
            Precondition.NotNull(name);
            Precondition.NotNull(context);

            Tables? obj = Value as Tables;
            if (obj != null)
            {
                switch (name)
                {
                    case "size":
                        return Create(obj.Count, context.Options);
                    case "first":
                        return Create(obj.FirstOrDefault(), context.Options);
                    case "last":
                        return Create(obj.LastOrDefault(), context.Options);
                }

                // otherwise we'll return a null value.
                return Create(null, context.Options);
            }

            return Create(null, context.Options);
        }

        /// <summary>
        /// Handle an index on this class, which is the element number or the table name.
        /// </summary>
        /// <param name="index">Index value.</param>
        /// <param name="context">Template context.</param>
        /// <returns>A <see cref="Table"/> with the given name.</returns>
        public override ValueTask<FluidValue> GetIndexAsync(FluidValue index, TemplateContext context)
        {
            Precondition.NotNull(index);
            Precondition.NotNull(context);

            Tables? obj = Value as Tables;
            if (obj != null)
            {
                if (index.ToNumberValue().ToString(CultureInfo.InvariantCulture) == index.ToStringValue())
                {
                    // it's a number
                    return Create(obj[(int)index.ToNumberValue()], context.Options);
                }
                else
                {
                    // it's a string, so the table name
                    return Create(obj[index.ToStringValue()], context.Options);
                }
            }

            return Create(null, context.Options);
        }

        /// <inheritdoc/>
        public override IEnumerable<FluidValue> Enumerate(TemplateContext context)
        {
            Precondition.NotNull(context);

            Tables? tables = Value as Tables;
            List<FluidValue> list = new List<FluidValue>();
            if (tables != null)
            {
                foreach (Table table in tables)
                {
                    list.Add(FluidValue.Create(table, context.Options));
                }
            }

            return list.ToArray();
        }
    }
}
