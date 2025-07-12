using MCPfiedSwagger.Models;
using MCPfiedSwagger.Parser.SchemaTypes;
using Microsoft.OpenApi.Models;

namespace MCPfiedSwagger.Parser
{
    public interface IMCPfiedSwaggerParser
    {
        ISchemaTypeParser DefaultSchemaTypeParser { get; }
        Dictionary<string, ISchemaTypeParser> SchemaTypeParsers { get; }
        McpJsonSchema ConvertSchema(OpenApiSchema schema, OpenApiDocument parentDocument);
    }
}