using MCPfiedSwagger.Models;
using Microsoft.OpenApi.Models;

namespace MCPfiedSwagger.Parser.OpenApiSchemes
{
    public interface IOpenApiSchemaHandler
    {
        bool CanHandle(OpenApiSchema schema);
        McpJsonSchema Convert(OpenApiSchema schema, OpenApiDocument apiDocument);
    }
}
