using Microsoft.AspNetCore.Mvc.Controllers;
using ModelContextProtocol.Protocol;
using System.Text.Json;

namespace MCPfiedSwagger
{
    public class MCPfiedSwaggerContext
    {
        public static MCPfiedSwaggerContext Current { get; } = new MCPfiedSwaggerContext();

        public Dictionary<Tool, ControllerActionDescriptor> Tools { get; internal set; } = new Dictionary<Tool, ControllerActionDescriptor>();
        public JsonSerializerOptions JsonSerializerOptions { get; set; } = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
    }
}
