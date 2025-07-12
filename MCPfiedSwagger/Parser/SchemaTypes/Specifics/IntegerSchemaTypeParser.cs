using MCPfiedSwagger.Models;
using Microsoft.OpenApi.Models;

namespace MCPfiedSwagger.Parser.SchemaTypes.Specifics
{
    internal class IntegerSchemaTypeParser : ISchemaTypeParser
    {
        public string[] TypeIdentifier => ["integer", "long"];
        public McpJsonSchema Parse(OpenApiSchema schema, OpenApiDocument parentDocument, MCPfiedSwaggerParser mainParser)
        {
            if (schema == null) throw new ArgumentNullException(nameof(schema), "OpenApiSchema cannot be null");
            return new McpJsonSchema
            {
                Type = "number",
                Description = schema.Description ?? "No Description"
            };
        }
    }
}
