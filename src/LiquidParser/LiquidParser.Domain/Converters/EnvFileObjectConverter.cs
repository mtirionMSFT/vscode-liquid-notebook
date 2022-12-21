namespace LiquidParser.Domain.Converters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Fluid;
    using Fluid.Values;
    using LiquidParser.Domain.Helpers;
    using LiquidParser.Domain.Models;

    /// <summary>
    /// Fluid Object Converter for <see cref="EnvFile"/> class.
    /// </summary>
    public class EnvFileObjectConverter : ObjectValueBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnvFileObjectConverter"/> class.
        /// </summary>
        /// <param name="value"><see cref="EnvFile"/> object.</param>
        public EnvFileObjectConverter(object value)
            : base(value)
        {
        }

        /// <inheritdoc/>
        public override ValueTask<FluidValue> GetValueAsync(string name, TemplateContext context)
        {
            Precondition.NotNull(name);
            Precondition.NotNull(context);

            EnvFile? obj = Value as EnvFile;
            if (obj != null)
            {
                switch (name)
                {
                    case "size":
                        return Create(obj.Settings.Count, context.Options);
                    case "first":
                        return Create(obj.Settings.FirstOrDefault(), context.Options);
                    case "last":
                        return Create(obj.Settings.LastOrDefault(), context.Options);
                }

                string lowIndex = name;
                if (obj.Settings.TryGetValue(lowIndex, out string? value))
                {
                    return Create(value, context.Options);
                }
                else
                {
                    lowIndex = lowIndex.Replace("__", ":");
                    if (obj.Settings.TryGetValue(lowIndex, out string? value2))
                    {
                        return Create(value2, context.Options);
                    }
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

            EnvFile? obj = Value as EnvFile;

            if (obj != null)
            {
                string name = index.ToStringValue();
                if (obj.Settings.TryGetValue(name, out string? value))
                {
                    return Create(value, context.Options);
                }
                else
                {
                    name = name.Replace(":", "__");
                    if (obj.Settings.TryGetValue(name, out string? value2))
                    {
                        return Create(value2, context.Options);
                    }
                }
            }

            return Create(null, context.Options);
        }

        /// <inheritdoc/>
        public override IEnumerable<FluidValue> Enumerate(TemplateContext context)
        {
            Precondition.NotNull(context);
            EnvFile? objs = Value as EnvFile;
            List<FluidValue> list = new List<FluidValue>();

            if (objs != null)
            {
                foreach (var obj in objs.Settings)
                {
                    list.Add(FluidValue.Create(obj.Value, context.Options));
                }
            }

            return list;
        }
    }
}
