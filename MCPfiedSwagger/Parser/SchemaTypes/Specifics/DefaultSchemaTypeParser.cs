using MCPfiedSwagger.Models;
using Microsoft.OpenApi.Models;

namespace MCPfiedSwagger.Parser.SchemaTypes.Specifics
{
    public class DefaultSchemaTypeParser : ISchemaTypeParser
    {
        public string[] TypeIdentifier => [string.Empty];
        public McpJsonSchema Parse(OpenApiSchema schema, OpenApiDocument parentDocument, MCPfiedSwaggerParser mainParser)
        {
            if (schema == null) throw new ArgumentNullException(nameof(schema), "OpenApiSchema cannot be null");

            if (schema.Reference != null)
            {
                return mainParser.ConvertSchema(parentDocument.Components.Schemas[schema.Reference.Id], parentDocument);
            }
            else
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
}
