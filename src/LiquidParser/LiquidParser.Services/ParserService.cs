namespace LiquidParser.Services
{
    using System;
    using System.Collections.Generic;
    using Fluid;
    using LiquidParser.Domain.Converters;
    using LiquidParser.Domain.Exceptions;
    using LiquidParser.Domain.Models;
    using Microsoft.Extensions.FileProviders;

    /// <summary>
    /// The service to process a liquid template with the provided content.
    /// </summary>
    public class ParserService
    {
        /// <summary>
        /// Renders a string as a template.
        /// </summary>
        /// <param name="data">Data model to provide to the template.</param>
        /// <param name="documents">Json documents to provide to the template.</param>
        /// <param name="envfiles">Environment files.</param>
        /// <param name="templateContent">The template content.</param>
        /// <param name="rootFolder">Root folder to use for includes (optional).</param>
        /// <returns>A rendered template.</returns>
        public string Render(
            Model data,
            JsonDocuments documents,
            EnvFiles envfiles,
            string templateContent,
            string? rootFolder = null)
        {
            var parser = new FluidParser();

            if (string.IsNullOrEmpty(templateContent))
            {
                return string.Empty;
            }

            // validating the template first
            if (parser.TryParse(templateContent, out IFluidTemplate template, out string error))
            {
                // now do the actual parsing
                TemplateOptions options = new TemplateOptions();
                options.MemberAccessStrategy = new UnsafeMemberAccessStrategy();

                if (rootFolder != null)
                {
                    // add file provider for includes
                    options.FileProvider = new PhysicalFileProvider(rootFolder);
                }

                // add necessary object converters
                options.ValueConverters.Add(o => o is Model m ? new ModelObjectConverter(m) : null);
                options.ValueConverters.Add(o => o is Tables ts ? new TablesObjectConverter(ts) : null);
                options.ValueConverters.Add(o => o is Table t ? new TableObjectConverter(t) : null);
                options.ValueConverters.Add(o => o is Record r ? new RecordObjectConverter(r) : null);
                options.ValueConverters.Add(o => o is Fields fs ? new FieldsObjectConverter(fs) : null);
                options.ValueConverters.Add(o => o is EnvFile e ? new EnvFileObjectConverter(e) : null);

                var ctx = new TemplateContext(new { }, options, true);

                // provide access to all tables through "model"
                ctx.SetValue("_model", data);

                // provide access to environment variables through "env"
                ctx.SetValue("_env", Environment.GetEnvironmentVariables());

                // provide access to all tables also by table name
                foreach (Table table in data.Tables)
                {
                    ctx.SetValue(table.Name, table);
                }

                // provide access to all JSON documents by filename
                foreach (KeyValuePair<string, dynamic> doc in documents.Documents)
                {
                    ctx.SetValue(doc.Key, (object)doc.Value);
                }

                // provide access to all env files by filename
                foreach (KeyValuePair<string, EnvFile> envFile in envfiles)
                {
                    ctx.SetValue(envFile.Key, envFile.Value);
                }

                try
                {
                    return template.Render(ctx);
                }
                catch (Exception ex)
                {
                    throw new ParserException(ex.Message, ex);
                }
            }
            else
            {
                throw new ParserException($"Parse error: {error}");
            }
        }
    }
}
