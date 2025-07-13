using System.Reflection;
using System.Text.Json;

namespace MCPfiedSwagger.Mapper.Handlers
{
    // Fallback handler for missing arguments
    public class FallbackArgumentHandler : IMethodArgumentHandler
    {
        public bool CanHandle(ParameterInfo parameter, IEnumerable<KeyValuePair<string, JsonElement>> arguments)
        {
            return true;
        }

        public object Handle(ParameterInfo parameter, IEnumerable<KeyValuePair<string, JsonElement>> arguments)
        {
            return parameter.HasDefaultValue ? parameter.DefaultValue : GetDefault(parameter.ParameterType);
        }

        private static object GetDefault(Type t) => t.IsValueType ? Activator.CreateInstance(t) : null;
    }


}
