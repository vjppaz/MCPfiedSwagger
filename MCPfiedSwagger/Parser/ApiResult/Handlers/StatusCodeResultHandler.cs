using Microsoft.AspNetCore.Mvc;
using ModelContextProtocol.Protocol;

namespace MCPfiedSwagger.Parser.ApiResult.Handlers
{
    public class StatusCodeResultHandler : IApiResultHandler
    {
        public bool CanHandle(object result) => result is StatusCodeResult;

        public Task<CallToolResult> HandleAsync(object result, bool expectedStructuredContent)
        {
            var status = (StatusCodeResult)result;

            return Task.FromResult(new CallToolResult
            {
                IsError = status.StatusCode >= 400,
                Content = [new TextContentBlock { Text = $"Status Code: {status.StatusCode}", Type = "text" }]
            });
        }
    }
}
