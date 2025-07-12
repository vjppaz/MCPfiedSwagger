using ModelContextProtocol.Protocol;
using System.Text.Json;

namespace MCPfiedSwagger.Parser.ApiResult.Handlers
{
    public class TaskResultHandler : IApiResultHandler
    {
        public bool CanHandle(object result) => result is Task;

        public async Task<CallToolResult> HandleAsync(object result)
        {
            var task = (Task)result;
            await task.ConfigureAwait(false);
            var resultProperty = task.GetType().GetProperty("Result");
            var value = resultProperty?.GetValue(task);
            var json = JsonSerializer.Serialize(value, MCPfiedSwaggerContext.Instance.JsonSerializerOptions);
            return new CallToolResult
            {
                Content = [new TextContentBlock { Text = json, Type = "text" }]
            };
        }
    }
}
