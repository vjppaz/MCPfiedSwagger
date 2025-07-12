using MCPfiedSwagger.Models;
using Microsoft.OpenApi.Models;

namespace MCPfiedSwagger.Parser.OpenApiSchemes.Handlers
{
    public class ArraySchemaHandler : IOpenApiSchemaHandler
    {
        public bool CanHandle(OpenApiSchema schema) => schema.Type == "array";

        public McpJsonSchema Convert(OpenApiSchema schema, OpenApiDocument apiDocument)
        {
            return new McpJsonSchema
            {
                Type = "array",
                Items = schema.Items != null ? OpenApiSchemaConverter.ConvertSchema(schema.Items, apiDocument) as McpJsonSchema : null,
                Description = schema.Description ?? "No Description"
            };
        }
    }
}
