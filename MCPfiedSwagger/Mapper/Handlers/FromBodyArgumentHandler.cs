using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json;

namespace MCPfiedSwagger.Mapper.Handlers
{
    // Handler for [FromBody]
    public class FromBodyArgumentHandler : IMethodArgumentHandler
    {
        public bool CanHandle(ParameterInfo parameter, IEnumerable<KeyValuePair<string, JsonElement>> arguments)
        {
            return parameter.GetCustomAttribute<FromBodyAttribute>() is not null;
        }

        public object Handle(ParameterInfo parameter, IEnumerable<KeyValuePair<string, JsonElement>> arguments)
        {
            object instance = null;
            if (parameter.ParameterType.GetConstructor(Type.EmptyTypes) != null)
            {
                instance = Activator.CreateInstance(parameter.ParameterType);
            }

            var objArgs = new List<object>();
            foreach (var property in parameter.ParameterType.GetProperties())
            {
                var argValue = arguments.FirstOrDefault(a => a.Key == $"request.{property.Name.ToLower()}");
                var objValue = JsonSerializer.Deserialize(argValue.Value.GetRawText(), property.PropertyType, MCPfiedSwaggerContext.Current.JsonSerializerOptions);
                if (instance is not null)
                {
                    property.SetValue(instance, objValue);
                }
                else
                {
                    objArgs.Add(objValue);
                }
            }
            if (instance is null)
            {
                instance = Activator.CreateInstance(parameter.ParameterType, objArgs.ToArray());
            }
            return instance;
        }
    }


}
