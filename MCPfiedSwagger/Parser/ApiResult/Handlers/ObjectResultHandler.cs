using Microsoft.AspNetCore.Mvc;
using ModelContextProtocol.Protocol;
using System.Net;
using System.Text.Json;

namespace MCPfiedSwagger.Parser.ApiResult.Handlers
{
    public class ObjectResultHandler : IApiResultHandler
    {
        public bool CanHandle(object result) => result is ObjectResult;

        public Task<CallToolResult> HandleAsync(object result, bool expectedStructuredContent)
        {
            var objectResult = (ObjectResult)result;

            var data = objectResult.Value ?? $"{(HttpStatusCode)objectResult.StatusCode}";
            var isError = objectResult.StatusCode >= 400;
            if (data is string)
            {
                return Task.FromResult(ApiResultConverter.CallToolResultFromJsonString($"{data}", expectedStructuredContent, isError));
            }
            else
            {
                var json = JsonSerializer.Serialize(data, MCPfiedSwaggerContext.Current.JsonSerializerOptions);
                return Task.FromResult(ApiResultConverter.CallToolResultFromJsonString(json, expectedStructuredContent, isError));
            }
        }
    }
}
