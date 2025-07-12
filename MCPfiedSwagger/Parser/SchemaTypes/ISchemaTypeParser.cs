using MCPfiedSwagger.Models;
using Microsoft.OpenApi.Models;

namespace MCPfiedSwagger.Parser.SchemaTypes
{
    public interface ISchemaTypeParser
    {
        public string[] TypeIdentifier { get; }

        public McpJsonSchema Parse(OpenApiSchema schema, OpenApiDocument parentDocument, MCPfiedSwaggerParser mainParser);
    }
}
