using MCPfiedSwagger.Models;
using Microsoft.OpenApi.Models;

namespace MCPfiedSwagger.Parser.OpenApiSchemes.Handlers
{
    public class ObjectSchemaHandler : IOpenApiSchemaHandler
    {
        public bool CanHandle(OpenApiSchema schema) => schema?.Type == "object";

        public McpJsonSchema Convert(OpenApiSchema schema, OpenApiDocument apiDocument)
        {
            var props = new Dictionary<string, McpJsonSchema>();
            if (schema.Properties != null)
            {
                foreach (var prop in schema.Properties)
                {
                    var converted = OpenApiSchemaConverter.ConvertSchema(prop.Value, apiDocument) as McpJsonSchema;
                    if (converted != null)
                        props[prop.Key] = converted;
                }
            }
            return new McpJsonSchema
            {
                Type = "object",
                Properties = props,
                Description = schema.Description ?? "No Description",
                Required = schema.Required?.ToArray()
            };
        }
    }
}
