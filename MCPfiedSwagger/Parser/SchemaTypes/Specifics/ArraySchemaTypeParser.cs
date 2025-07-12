using MCPfiedSwagger.Models;
using Microsoft.OpenApi.Models;

namespace MCPfiedSwagger.Parser.SchemaTypes.Specifics
{
    public class ArraySchemaTypeParser : ISchemaTypeParser
    {
        public string[] TypeIdentifier => ["array"];

        public McpJsonSchema Parse(OpenApiSchema schema, OpenApiDocument parentDocument, MCPfiedSwaggerParser mainParser)
        {
            schema = schema ?? throw new ArgumentNullException(nameof(schema), "OpenApiSchema cannot be null");
            mainParser = mainParser ?? throw new ArgumentNullException(nameof(mainParser), "MCPfiedSwaggerParser cannot be null");

            return new McpJsonSchema
            {
                Type = "array",
                Items = schema.Items != null ? mainParser.ConvertSchema(schema.Items, parentDocument) : null,
                Description = schema.Description ?? "No Description"
            };
        }
    }
}
