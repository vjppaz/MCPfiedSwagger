using ModelContextProtocol.Protocol;
using System.Text.Json;

namespace MCPfiedSwagger.Parser.ApiResult.Handlers
{
    public class DefaultResultHandler : IApiResultHandler
    {
        public bool CanHandle(object result) => true;

        public Task<CallToolResult> HandleAsync(object result)
        {
            if (result is null)
            {
                return Task.FromResult(new CallToolResult
                {
                    IsError = true,
                    Content = [new TextContentBlock { Text = "No result returned from the method.", Type = "text" }]
                });
            }
            var json = JsonSerializer.Serialize(result, MCPfiedSwaggerContext.Instance.JsonSerializerOptions);
            return Task.FromResult(new CallToolResult
            {
                Content = [new TextContentBlock { Text = json, Type = "text" }]
            });
        }
    }
}
