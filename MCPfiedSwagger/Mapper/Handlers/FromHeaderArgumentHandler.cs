using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json;

namespace MCPfiedSwagger.Mapper.Handlers
{
    // Handler for [FromHeader]
    public class FromHeaderArgumentHandler : IMethodArgumentHandler
    {
        public bool CanHandle(ParameterInfo parameter, IEnumerable<KeyValuePair<string, JsonElement>> arguments)
        {
            return parameter.GetCustomAttribute<FromHeaderAttribute>() is not null;
        }

        public object Handle(ParameterInfo parameter, IEnumerable<KeyValuePair<string, JsonElement>> arguments)
        {
            var fromHeader = parameter.GetCustomAttribute<FromHeaderAttribute>();
            var arg = arguments.FirstOrDefault(a => a.Key == fromHeader.Name);
            if (arg.Value.ValueKind == JsonValueKind.Null || arg.Value.ValueKind == JsonValueKind.Undefined)
            {
                return parameter.HasDefaultValue ? parameter.DefaultValue : GetDefault(parameter.ParameterType);
            }
            return JsonSerializer.Deserialize(arg.Value.GetRawText(), parameter.ParameterType, MCPfiedSwaggerContext.Current.JsonSerializerOptions);
        }

        private static object GetDefault(Type t) => t.IsValueType ? Activator.CreateInstance(t) : null;
    }


}
