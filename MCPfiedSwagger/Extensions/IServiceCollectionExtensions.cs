using MCPfiedSwagger.Parser.ApiResult;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using System.Reflection;
using System.Text.Json;

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

            var tool = MCPfiedSwaggerContext.Instance.Tools.ToList().FirstOrDefault(m => m.Key.Name == requestContext.Params.Name);
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

            var args = GetMethodArguments(parameters, requestContext.Params.Arguments);
            var result = descriptor.MethodInfo.Invoke(controller, args);
            var callToolResult = await ApiResultConverter.ConvertAsync(result);

            return callToolResult;
        }

        private static async ValueTask<ListToolsResult> ListToolHandler(RequestContext<ListToolsRequestParams> requestContext, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            return new ListToolsResult() { Tools = MCPfiedSwaggerContext.Instance.Tools.Select(m => m.Key).ToList() };
        }

        private static object[] GetMethodArguments(ParameterInfo[] parameters, IEnumerable<KeyValuePair<string, JsonElement>> arguments)
        {
            var args = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                var param = parameters[i];
                var arg = arguments.FirstOrDefault(a => a.Key == param.Name);
                if (arg.Value.ValueKind == JsonValueKind.Null)
                {
                    args[i] = param.HasDefaultValue ? param.DefaultValue : GetDefault(param.ParameterType);
                }
                else
                {
                    args[i] = arg.Value is JsonElement jsonElem
                        ? JsonSerializer.Deserialize(jsonElem.GetRawText(), param.ParameterType, MCPfiedSwaggerContext.Instance.JsonSerializerOptions)
                        : Convert.ChangeType(arg.Value, param.ParameterType);
                }
            }

            static object GetDefault(Type t) => t.IsValueType ? Activator.CreateInstance(t) : null;
            return args;
        }
    }
}
