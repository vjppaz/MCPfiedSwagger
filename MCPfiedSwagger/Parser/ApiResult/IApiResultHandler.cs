using ModelContextProtocol.Protocol;

namespace MCPfiedSwagger.Parser.ApiResult
{
    public interface IApiResultHandler
    {
        bool CanHandle(object result);
        Task<CallToolResult> HandleAsync(object result, bool expectedStructuredContent);
    }
}
