using ModelContextProtocol.Protocol;

namespace MCPfiedSwagger.Parser.ApiResult.Handlers
{
    public class StringResultHandler : IApiResultHandler
    {
        public bool CanHandle(object result) => result is string;

        public Task<CallToolResult> HandleAsync(object result, bool expectedStructuredContent)
        {
            return Task.FromResult(new CallToolResult
            {
                Content = [new TextContentBlock { Text = (string)result, Type = "text" }]
            });
        }
    }
}
