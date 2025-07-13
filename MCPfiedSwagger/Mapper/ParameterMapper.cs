using MCPfiedSwagger.Mapper.Handlers;
using System.Reflection;
using System.Text.Json;

namespace MCPfiedSwagger.Mapper
{
    public class ParameterMapper
    {
        // Replace the GetMethodArguments method in IServiceCollectionExtensions with the following:

        private static readonly List<IMethodArgumentHandler> _argumentHandlers = new()
        {
            new FromHeaderArgumentHandler(),
            new FromBodyArgumentHandler(),
            new DefaultArgumentHandler(),
            new FallbackArgumentHandler()
        };

        public static object[] GetMethodArguments(ParameterInfo[] parameters, IEnumerable<KeyValuePair<string, JsonElement>> arguments)
        {
            var args = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                var param = parameters[i];
                foreach (var handler in _argumentHandlers)
                {
                    if (handler.CanHandle(param, arguments))
                    {
                        args[i] = handler.Handle(param, arguments);
                        break;
                    }
                }
            }
            return args;
        }
    }
}
