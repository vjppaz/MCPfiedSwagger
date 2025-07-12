using Microsoft.AspNetCore.Mvc;
using ModelContextProtocol.Protocol;

namespace MCPfiedSwagger.Parser.ApiResult.Handlers
{
    public class BadRequestObjectResultHandler : IApiResultHandler
    {
        public bool CanHandle(object result) => result is BadRequestObjectResult;

        public Task<CallToolResult> HandleAsync(object result)
        {
            var badRequest = (BadRequestObjectResult)result;
            return Task.FromResult(new CallToolResult
            {
                IsError = true,
                Content = [new TextContentBlock { Text = badRequest.Value?.ToString() ?? "Bad Request", Type = "text" }]
            });
        }
    }
}
