using MCPfiedSwagger.Mapper;
using MCPfiedSwagger.Parser.ApiResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace MCPfiedSwagger.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IMcpServerBuilder AddMCPfiedSwagger(this IServiceCollection services)
        {
            var builder = services
                .AddMcpServer()
                .WithHttpTransport(opt =>
                {
                    opt.Stateless = _stateless;
                })
                .WithCallToolHandler(CallToolHandler)
                .WithListToolsHandler(ListToolHandler);

            return builder;
        }

        private static bool _stateless = false;
        public static IMcpServerBuilder AddAuthorization(this IMcpServerBuilder builder)
        {
            _stateless = true;
            builder.Services.AddHttpContextAccessor();

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
            var controller = (ControllerBase)ActivatorUtilities.CreateInstance(serviceProvider, controllerType);
            var parameters = descriptor.MethodInfo.GetParameters();

            var args = ParameterMapper.GetMethodArguments(parameters, requestContext.Params.Arguments);
            if (_stateless)
            {
                controller.ControllerContext = GenerateControllerContext(requestContext.Services, descriptor);
                var authResult = await ValidateAuthorization(requestContext.Services, descriptor);
                if (!authResult)
                {
                    return new CallToolResult()
                    {
                        IsError = true,
                        Content = [new TextContentBlock { Text = "Unauthorized", Type = "text" }]
                    };
                }
            }
            var result = descriptor.MethodInfo.Invoke(controller, args);

            var expectedStructuredContent = tool.Key.OutputSchema != null;
            var callToolResult = await ApiResultConverter.ConvertAsync(result, expectedStructuredContent);

            return callToolResult;
        }

        private static async Task<bool> ValidateAuthorization(IServiceProvider serviceProvider, ControllerActionDescriptor descriptor)
        {
            var authorizationService = serviceProvider.GetRequiredService<IAuthorizationService>();
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

            // Authorization Check
            var authorizeAttributes = descriptor.MethodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true).Cast<AuthorizeAttribute>().ToList();
            var authorizeControllerAttributes = descriptor.ControllerTypeInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true).Cast<AuthorizeAttribute>().ToList();
            var allAttributes = authorizeAttributes.Union(authorizeControllerAttributes);

            foreach (var authAttribute in allAttributes)
            {
                var policy = authAttribute.Policy;
                var roles = authAttribute.Roles;

                AuthorizationResult authResult;

                if (!string.IsNullOrWhiteSpace(policy))
                {
                    authResult = await authorizationService.AuthorizeAsync(httpContextAccessor.HttpContext.User, policy);
                }
                else if (!string.IsNullOrWhiteSpace(roles))
                {
                    var rolesPolicy = new AuthorizationPolicyBuilder()
                                        .RequireRole(roles.Split(','))
                                        .Build();
                    authResult = await authorizationService.AuthorizeAsync(httpContextAccessor.HttpContext.User, rolesPolicy);
                }
                else
                {
                    authResult = (httpContextAccessor.HttpContext.User?.Identity.IsAuthenticated ?? false) ?
                        AuthorizationResult.Success() :
                        AuthorizationResult.Failed();
                }

                if (!authResult.Succeeded)
                {
                    return false;
                }
            }

            return true;
        }

        private static ControllerContext? GenerateControllerContext(IServiceProvider serviceProvider, ControllerActionDescriptor descriptor)
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            return new ControllerContext
            {
                ActionDescriptor = descriptor,
                HttpContext = httpContextAccessor.HttpContext
            };
        }

        private static async ValueTask<ListToolsResult> ListToolHandler(RequestContext<ListToolsRequestParams> requestContext, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            return new ListToolsResult() { Tools = MCPfiedSwaggerContext.Current.Tools.Select(m => m.Key).ToList() };
        }
    }
}
