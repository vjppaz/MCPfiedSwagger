using MCPfiedSwagger.Parser.ApiResult.Handlers;
using ModelContextProtocol.Protocol;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MCPfiedSwagger.Parser.ApiResult
{
    public static class ApiResultConverter
    {
        private static readonly List<IApiResultHandler> _handlers = new()
        {
            new ObjectResultHandler(),
            new StringResultHandler(),
            new TaskResultHandler(),
            new ActionResultHandler(), //must be last as it handles all other ActionResult
            new StatusCodeResultHandler(),
            new DefaultResultHandler()
        };

        public static async Task<CallToolResult> ConvertAsync(object result, bool expectedStructuredContent)
        {
            foreach (var handler in _handlers)
            {
                if (handler.CanHandle(result))
                {
                    return await handler.HandleAsync(result, expectedStructuredContent);
                }
            }

            // Should never reach here due to DefaultResultHandler
            return new CallToolResult
            {
                IsError = true,
                Content = [new TextContentBlock { Text = "Unhandled result type.", Type = "text" }]
            };
        }

        public static CallToolResult CallToolResultFromJsonString(string json, bool expectedStructuredContent, bool? isError = null)
        {
            if (expectedStructuredContent)
            {
                return new CallToolResult
                {
                    IsError = isError,
                    StructuredContent = JsonSerializer.Deserialize<JsonNode>(json, MCPfiedSwaggerContext.Current.JsonSerializerOptions)
                };
            }

            return new CallToolResult
            {
                IsError = isError,
                Content = [new TextContentBlock { Text = json, Type = "text" }]
            };
        }
    }
}
