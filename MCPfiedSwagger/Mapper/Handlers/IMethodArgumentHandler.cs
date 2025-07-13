using System.Reflection;
using System.Text.Json;

namespace MCPfiedSwagger.Mapper.Handlers
{
    // Add this new interface and handler classes at an appropriate location in your project, e.g., MCPfiedSwagger/Parser/MethodArguments/IMethodArgumentHandler.cs

    public interface IMethodArgumentHandler
    {
        bool CanHandle(ParameterInfo parameter, IEnumerable<KeyValuePair<string, JsonElement>> arguments);
        object Handle(ParameterInfo parameter, IEnumerable<KeyValuePair<string, JsonElement>> arguments);
    }


}
