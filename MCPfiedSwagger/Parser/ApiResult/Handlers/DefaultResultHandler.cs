using ModelContextProtocol.Protocol;
using System.Text.Json;

namespace MCPfiedSwagger.Parser.ApiResult.Handlers
{
    public class DefaultResultHandler : IApiResultHandler
    {
        public bool CanHandle(object result) => true;

        public Task<CallToolResult> HandleAsync(object result, bool expectedStructuredContent)
        {
            if (result is null)
            {
                return Task.FromResult(new CallToolResult
                {
                    IsError = true,
                    Content = [new TextContentBlock { Text = "No result returned.", Type = "text" }]
                });
            }

            var json = JsonSerializer.Serialize(result, MCPfiedSwaggerContext.Current.JsonSerializerOptions);
            return Task.FromResult(ApiResultConverter.CallToolResultFromJsonString(json, expectedStructuredContent));
        }
    }
}
