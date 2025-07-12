using MCPfiedSwagger.Models;
using Microsoft.OpenApi.Models;

namespace MCPfiedSwagger.Parser.OpenApiSchemes.Handlers
{
    public class FormatSchemaHandler : IOpenApiSchemaHandler
    {
        public bool CanHandle(OpenApiSchema schema) => schema.Format != null;

        public McpJsonSchema Convert(OpenApiSchema schema, OpenApiDocument apiDocument)
        {
            return new McpJsonSchema
            {
                Type = schema.Type ?? "string",
                Format = schema.Format,
                Description = schema.Description ?? "No Description"
            };
        }
    }
}
