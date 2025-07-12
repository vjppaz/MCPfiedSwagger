using MCPfiedSwagger.Models;
using Microsoft.OpenApi.Models;

namespace MCPfiedSwagger.Parser.SchemaTypes.Specifics
{
    public class ObjectSchemaTypeParser : ISchemaTypeParser
    {
        public string[] TypeIdentifier => ["object"];

        public McpJsonSchema Parse(OpenApiSchema schema, OpenApiDocument parentDocument, MCPfiedSwaggerParser mainParser)
        {
            var props = new Dictionary<string, McpJsonSchema>();
            if (schema.Properties != null)
            {
                foreach (var prop in schema.Properties)
                {
                    props[prop.Key] = mainParser.ConvertSchema(prop.Value, parentDocument);
                }
            }

            return new McpJsonSchema
            {
                Type = "object",
                Properties = props,
                Description = schema.Description ?? "No Description",
                Required = props.Where(m => m.Value.Required.Any()).Select(m => m.Key).ToArray();
            };
        }
    }
}
