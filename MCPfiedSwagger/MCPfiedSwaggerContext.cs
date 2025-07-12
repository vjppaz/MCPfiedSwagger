using Microsoft.AspNetCore.Mvc.Controllers;
using ModelContextProtocol.Protocol;
using System.Text.Json;

namespace MCPfiedSwagger
{
    public class MCPfiedSwaggerContext
    {
        public static MCPfiedSwaggerContext Instance { get; } = new MCPfiedSwaggerContext();

        public Dictionary<Tool, ControllerActionDescriptor> Tools { get; internal set; }
        public JsonSerializerOptions JsonSerializerOptions { get; set; } = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}
