using Microsoft.AspNetCore.Mvc;
using ModelContextProtocol.Protocol;
using System.Text.Json;

namespace MCPfiedSwagger.Parser.ApiResult.Handlers
{
    public class ActionResultHandler : IApiResultHandler
    {
        public bool CanHandle(object result) => result?.GetType()?.FullName?.StartsWith("Microsoft.AspNetCore.Mvc.ActionResult") ?? false;

        public Task<CallToolResult> HandleAsync(object result, bool expectedStructuredContent)
        {
            var objectResult = (dynamic)result;
            var okObjectResult = objectResult.Result as ObjectResult;
            var json = JsonSerializer.Serialize(okObjectResult?.Value ?? (expectedStructuredContent ? new { } : string.Empty), MCPfiedSwaggerContext.Current.JsonSerializerOptions);
            return Task.FromResult(ApiResultConverter.CallToolResultFromJsonString(json, expectedStructuredContent));
        }
    }
}
