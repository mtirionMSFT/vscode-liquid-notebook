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
    public class FieldsObjectConverter : ObjectValueBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldsObjectConverter"/> class.
        /// </summary>
        /// <param name="value"><see cref="Fields"/> object.</param>
        public FieldsObjectConverter(object value)
            : base(value)
        {
        }

        /// <inheritdoc/>
        public override ValueTask<FluidValue> GetValueAsync(string name, TemplateContext context)
        {
            Precondition.NotNull(name);
            Precondition.NotNull(context);

            Fields? obj = Value as Fields;
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

                if (name.Equals(nameof(Fields.Table), System.StringComparison.OrdinalIgnoreCase))
                {
                    return Create(obj.Table, context.Options);
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

            Fields? obj = Value as Fields;

            if (obj != null)
            {
                if (index.ToNumberValue().ToString(CultureInfo.InvariantCulture) == index.ToStringValue())
                {
                    return Create(obj[(int)index.ToNumberValue()], context.Options);
                }
                else
                {
                    return Create(obj[index.ToStringValue()], context.Options);
                }
            }

            return Create(null, context.Options);
        }

        /// <inheritdoc/>
        public override IEnumerable<FluidValue> Enumerate(TemplateContext context)
        {
            Precondition.NotNull(context);
            Fields? fields = Value as Fields;
            List<FluidValue> list = new List<FluidValue>();
            if (fields != null)
            {
                foreach (Field field in fields)
                {
                    list.Add(FluidValue.Create(field, context.Options));
                }
            }

            return list;
        }
    }
}
