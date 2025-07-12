using MCPfiedSwagger.Models;
using Microsoft.OpenApi.Models;

namespace MCPfiedSwagger.Parser.OpenApiSchemes.Handlers
{
    public class DefaultSchemaHandler : IOpenApiSchemaHandler
    {
        public bool CanHandle(OpenApiSchema schema) => true;

        public McpJsonSchema Convert(OpenApiSchema schema, OpenApiDocument apiDocument)
        {
            return new McpJsonSchema
            {
                Type = schema.Type ?? "string",
                Description = schema.Description ?? "No Description"
            };
        }
    }
}
