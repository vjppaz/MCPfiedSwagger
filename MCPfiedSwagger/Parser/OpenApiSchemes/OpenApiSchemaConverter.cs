using MCPfiedSwagger.Models;
using MCPfiedSwagger.Parser.OpenApiSchemes.Handlers;
using Microsoft.OpenApi.Models;

namespace MCPfiedSwagger.Parser.OpenApiSchemes
{
    public static class OpenApiSchemaConverter
    {
        private static readonly List<IOpenApiSchemaHandler> _handlers = new()
            {
                new ObjectSchemaHandler(),
                new ArraySchemaHandler(),
                new IntegerSchemaHandler(),
                new ReferenceSchemaHandler(),
                new FormatSchemaHandler(),
                new DefaultSchemaHandler()
            };

        public static McpJsonSchema ConvertSchema(OpenApiSchema schema, OpenApiDocument apiDocument)
        {
            foreach (var handler in _handlers)
            {
                if (handler.CanHandle(schema))
                {
                    return handler.Convert(schema, apiDocument);
                }
            }
            return new McpJsonSchema
            {
                Type = "unknown",
                Description = "Unhandled schema type"
            };
        }
    }
}
