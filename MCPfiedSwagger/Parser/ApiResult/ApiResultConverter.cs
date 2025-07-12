using MCPfiedSwagger.Parser.ApiResult.Handlers;
using ModelContextProtocol.Protocol;

namespace MCPfiedSwagger.Parser.ApiResult
{
    public static class ApiResultConverter
    {
        private static readonly List<IApiResultHandler> _handlers = new()
        {
            new OkObjectResultHandler(),
            new BadRequestObjectResultHandler(),
            new StringResultHandler(),
            new TaskResultHandler(),
            new DefaultResultHandler()
        };

        public static async Task<CallToolResult> ConvertAsync(object result)
        {
            foreach (var handler in _handlers)
            {
                if (handler.CanHandle(result))
                {
                    return await handler.HandleAsync(result);
                }
            }
            // Should never reach here due to DefaultResultHandler
            return new CallToolResult
            {
                IsError = true,
                Content = [new TextContentBlock { Text = "Unhandled result type.", Type = "text" }]
            };
        }
    }
}
