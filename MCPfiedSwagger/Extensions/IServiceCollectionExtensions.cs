using MCPfiedSwagger.Mapper;
using MCPfiedSwagger.Parser.ApiResult;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace MCPfiedSwagger.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IMcpServerBuilder AddMCPfiedSwagger(this IServiceCollection services)
        {
            var builder = services.AddMcpServer()
                .WithHttpTransport()
                .WithCallToolHandler(CallToolHandler)
                .WithListToolsHandler(ListToolHandler);

            return builder;
        }

        private static async ValueTask<CallToolResult> CallToolHandler(RequestContext<CallToolRequestParams> requestContext, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            var tool = MCPfiedSwaggerContext.Current.Tools.ToList().FirstOrDefault(m => m.Key.Name == requestContext.Params.Name);
            var descriptor = tool.Value;
            var serviceProvider = requestContext.Services;

            if (descriptor == null)
            {
                return new CallToolResult()
                {
                    IsError = true,
                    Content = [new TextContentBlock { Text = $"Tool doesn't have an associated descriptor", Type = "text" }]
                };
            }

            var controllerType = descriptor.ControllerTypeInfo.AsType();
            var controller = ActivatorUtilities.CreateInstance(serviceProvider, controllerType);
            var parameters = descriptor.MethodInfo.GetParameters();

            var args = ParameterMapper.GetMethodArguments(parameters, requestContext.Params.Arguments);
            var result = descriptor.MethodInfo.Invoke(controller, args);

            var expectedStructuredContent = tool.Key.OutputSchema != null;
            var callToolResult = await ApiResultConverter.ConvertAsync(result, expectedStructuredContent);

            return callToolResult;
        }

        private static async ValueTask<ListToolsResult> ListToolHandler(RequestContext<ListToolsRequestParams> requestContext, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            return new ListToolsResult() { Tools = MCPfiedSwaggerContext.Current.Tools.Select(m => m.Key).ToList() };
        }
    }
}
