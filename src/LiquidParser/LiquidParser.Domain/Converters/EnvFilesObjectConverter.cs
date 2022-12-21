namespace LiquidParser.Domain.Converters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fluid;
    using Fluid.Values;
    using LiquidParser.Domain.Helpers;
    using LiquidParser.Domain.Models;

    /// <summary>
    /// Fluid Object Converter for <see cref="EnvFiles"/> class.
    /// </summary>
    public class EnvFilesObjectConverter : ObjectValueBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnvFilesObjectConverter"/> class.
        /// </summary>
        /// <param name="value"><see cref="EnvFiles"/> object.</param>
        public EnvFilesObjectConverter(object value)
            : base(value)
        {
        }

        /// <inheritdoc/>
        public override ValueTask<FluidValue> GetValueAsync(string name, TemplateContext context)
        {
            Precondition.NotNull(name);
            Precondition.NotNull(context);

            EnvFiles? obj = Value as EnvFiles;
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

                string lowIndex = name.ToLower();
                if (obj.TryGetValue(lowIndex, out EnvFile? value))
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

            EnvFiles? obj = Value as EnvFiles;

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
            EnvFiles? objs = Value as EnvFiles;
            List<FluidValue> list = new List<FluidValue>();

            if (objs != null)
            {
                foreach (var obj in objs)
                {
                    list.Add(FluidValue.Create(obj.Value, context.Options));
                }
            }

            return list;
        }
    }
}
