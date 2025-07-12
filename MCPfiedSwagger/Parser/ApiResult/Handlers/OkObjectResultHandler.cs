using Microsoft.AspNetCore.Mvc;
using ModelContextProtocol.Protocol;
using System.Text.Json;

namespace MCPfiedSwagger.Parser.ApiResult.Handlers
{
    public class OkObjectResultHandler : IApiResultHandler
    {
        public bool CanHandle(object result) => result is OkObjectResult;

        public Task<CallToolResult> HandleAsync(object result)
        {
            var objectResult = (OkObjectResult)result;
            var json = JsonSerializer.Serialize(objectResult.Value, MCPfiedSwaggerContext.Instance.JsonSerializerOptions);
            return Task.FromResult(new CallToolResult
            {
                Content = [new TextContentBlock { Text = json, Type = "text" }]
            });
        }
    }
}
