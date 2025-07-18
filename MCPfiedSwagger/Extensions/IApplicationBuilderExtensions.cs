using MCPfiedSwagger.Models;
using MCPfiedSwagger.Parser.OpenApiSchemes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using ModelContextProtocol.Protocol;
using Swashbuckle.AspNetCore.Swagger;
using System.Text.Json;

namespace MCPfiedSwagger.Extensions
{
    public static partial class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMCPfiedSwagger(this IApplicationBuilder app, string documentName = "v1", string pattern = "/mcp")
        {
            app = app ?? throw new ArgumentNullException(nameof(app));

            var provider = app.ApplicationServices.GetRequiredService<IApiDescriptionGroupCollectionProvider>();
            var swaggerProvider = app.ApplicationServices.GetRequiredService<ISwaggerProvider>();
            var actionProvider = app.ApplicationServices.GetRequiredService<IActionDescriptorCollectionProvider>();

            var swaggerDoc = swaggerProvider.GetSwagger(documentName);

            foreach (var path in swaggerDoc.Paths)
            {
                foreach (var operation in path.Value.Operations)
                {
                    var tool = CreateToolDefinition(path, operation, swaggerDoc, app.ApplicationServices);
                    var descriptor = GetControllerActionDescriptor(path, operation, actionProvider);

                    MCPfiedSwaggerContext.Current.Tools.Add(tool, descriptor);
                }
            }

            if (app is IEndpointRouteBuilder appBuilder)
            {
                appBuilder.MapMcp(pattern);
            }
            else
            {
                throw new InvalidOperationException("The application builder does not support endpoint routing. Ensure you are using IEndpointRouteBuilder.");
            }

            return app;
        }

        private static ControllerActionDescriptor GetControllerActionDescriptor(KeyValuePair<string, OpenApiPathItem> path, KeyValuePair<OperationType, OpenApiOperation> operation, IActionDescriptorCollectionProvider actionProvider)
        {
            var controllerActionDescriptors = actionProvider.ActionDescriptors.Items.OfType<ControllerActionDescriptor>();
            var descriptor = controllerActionDescriptors.FirstOrDefault(ad =>
                            ad.AttributeRouteInfo?.Template != null &&
                            $"/{ad.AttributeRouteInfo.Template.TrimEnd('/')}".Equals(path.Key.TrimEnd('/'), StringComparison.OrdinalIgnoreCase) &&
                            ad.ActionConstraints?.OfType<HttpMethodActionConstraint>()
                                .Any(ac => ac.HttpMethods.Contains(operation.Key.ToString(), StringComparer.OrdinalIgnoreCase)) == true);
            return descriptor;
        }

        private static Tool CreateToolDefinition(KeyValuePair<string, OpenApiPathItem> path, KeyValuePair<OperationType, OpenApiOperation> operation, OpenApiDocument apiDocument, IServiceProvider serviceProvider)
        {
            // Replace the selected code in UseSwaggerMCPed with the following
            var method = operation.Key.ToString();
            var endpoint = path.Key.TrimStart('/');
            var summary = operation.Value.Summary ?? "No summary";
            var outputDescription = operation.Value.Responses?.FirstOrDefault().Value?.Description ?? "Response";
            var input = CreateInputSchema(operation.Value, apiDocument);
            var okResponse = operation.Value.Responses?.FirstOrDefault(m => m.Key == "200").Value;
            var output = CreateOutputSchema(okResponse, apiDocument);

            return new Tool
            {
                Description = summary,
                Name = $"{method}_{endpoint}",
                InputSchema = input,
                Title = $"{method.ToUpper()} {endpoint}",
                OutputSchema = output
            };
        }

        private static JsonElement CreateInputSchema(OpenApiOperation operation, OpenApiDocument apiDocument)
        {
            var result = new McpJsonSchema();
            var schema = new OpenApiSchema()
            {
                Type = "object",
                Properties = operation.Parameters.ToDictionary(
                    p => p.Name,
                    p =>
                    {
                        p.Schema.Description = p.Description;
                        return p.Schema;
                    }),
                Description = "Input parameters schema",
            };
            var requestBody = operation.RequestBody?.Content?.FirstOrDefault(m => m.Key == "application/json").Value?.Schema;
            if (requestBody is not null)
            {
                var bodySchema = requestBody.Reference == null ? requestBody : apiDocument.Components.Schemas[requestBody.Reference.Id];
                foreach (var bodyProperty in bodySchema.Properties)
                {
                    var key = $"request.{bodyProperty.Key}";
                    if (!schema.Properties.ContainsKey(key))
                    {
                        schema.Properties.Add(key, bodyProperty.Value);
                    }
                }
            }

            result = OpenApiSchemaConverter.ConvertSchema(schema, apiDocument);
            return result.ToJsonElement();
        }

        private static JsonElement? CreateOutputSchema(OpenApiResponse? openApiResponse, OpenApiDocument apiDocument)
        {

            if (openApiResponse == null) return null;

            // Try to get the first schema from the response content (e.g., "application/json")
            var schema = openApiResponse.Content?.FirstOrDefault(m => m.Key == "application/json").Value?.Schema;
            if (schema == null) return null;


            //TODO: workaround, current version doesn't support array schema yet, refer to https://github.com/modelcontextprotocol/inspector/issues/552
            if (schema.Type == "array")
            {
                return null;
            }

            var jsonSchema = OpenApiSchemaConverter.ConvertSchema(schema, apiDocument);
            return jsonSchema.ToJsonElement();
        }
    }
}
