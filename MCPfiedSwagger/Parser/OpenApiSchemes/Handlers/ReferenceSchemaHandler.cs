using MCPfiedSwagger.Models;
using Microsoft.OpenApi.Models;

namespace MCPfiedSwagger.Parser.OpenApiSchemes.Handlers
{
    public class ReferenceSchemaHandler : IOpenApiSchemaHandler
    {
        public bool CanHandle(OpenApiSchema schema) => schema.Reference != null;

        public McpJsonSchema Convert(OpenApiSchema schema, OpenApiDocument apiDocument)
        {
            if (schema.Reference != null && apiDocument.Components.Schemas.TryGetValue(schema.Reference.Id, out var referencedSchema))
            {
                return OpenApiSchemaConverter.ConvertSchema(referencedSchema, apiDocument) as McpJsonSchema;
            }
            return new McpJsonSchema
            {
                Type = "unknown",
                Description = "Reference not found"
            };
        }
    }
}
