using System.Reflection;
using System.Text.Json;

namespace MCPfiedSwagger.Mapper.Handlers
{
    // Handler for default argument mapping (by name)
    public class DefaultArgumentHandler : IMethodArgumentHandler
    {
        public bool CanHandle(ParameterInfo parameter, IEnumerable<KeyValuePair<string, JsonElement>> arguments)
        {
            var arg = arguments.FirstOrDefault(a => a.Key == parameter.Name);
            return arg.Value.ValueKind != JsonValueKind.Undefined;
        }

        public object Handle(ParameterInfo parameter, IEnumerable<KeyValuePair<string, JsonElement>> arguments)
        {
            var arg = arguments.FirstOrDefault(a => a.Key == parameter.Name);
            if (arg.Value.ValueKind == JsonValueKind.Null || arg.Value.ValueKind == JsonValueKind.Undefined)
            {
                return parameter.HasDefaultValue ? parameter.DefaultValue : GetDefault(parameter.ParameterType);
            }
            return JsonSerializer.Deserialize(arg.Value.GetRawText(), parameter.ParameterType, MCPfiedSwaggerContext.Current.JsonSerializerOptions);
        }

        private static object GetDefault(Type t) => t.IsValueType ? Activator.CreateInstance(t) : null;
    }


}
