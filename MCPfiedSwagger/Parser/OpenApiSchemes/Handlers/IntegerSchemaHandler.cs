using MCPfiedSwagger.Models;
using Microsoft.OpenApi.Models;

namespace MCPfiedSwagger.Parser.OpenApiSchemes.Handlers
{
    public class IntegerSchemaHandler : IOpenApiSchemaHandler
    {
        public bool CanHandle(OpenApiSchema schema) => schema.Type == "integer" || schema.Type == "long";

        public McpJsonSchema Convert(OpenApiSchema schema, OpenApiDocument apiDocument)
        {
            return new McpJsonSchema
            {
                Type = "number",
                Description = schema.Description ?? "No Description"
            };
        }
    }
}
