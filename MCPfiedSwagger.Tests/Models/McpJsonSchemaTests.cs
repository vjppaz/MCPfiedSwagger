using MCPfiedSwagger.Models;

namespace MCPfiedSwagger.Tests.Models
{
    public class McpJsonSchemaTests
    {
        [Fact]
        public void ToJsonElement_SerializesSimpleSchema_Correctly()
        {
            var schema = new McpJsonSchema
            {
                Type = "string",
                Format = "date-time",
                Description = "A date-time string"
            };

            var jsonElement = schema.ToJsonElement();

            Assert.Equal("string", jsonElement.GetProperty("type").GetString());
            Assert.Equal("date-time", jsonElement.GetProperty("format").GetString());
            Assert.Equal("A date-time string", jsonElement.GetProperty("description").GetString());
        }

        [Fact]
        public void ToJsonElement_SerializesWithPropertiesAndRequired_Correctly()
        {
            var schema = new McpJsonSchema
            {
                Type = "object",
                Properties = new Dictionary<string, McpJsonSchema>
                    {
                        { "id", new McpJsonSchema { Type = "integer", Format = "int32" } },
                        { "name", new McpJsonSchema { Type = "string" } }
                    },
                Required = new[] { "id" }
            };

            var jsonElement = schema.ToJsonElement();

            Assert.Equal("object", jsonElement.GetProperty("type").GetString());
            Assert.True(jsonElement.TryGetProperty("properties", out var props));
            Assert.True(props.TryGetProperty("id", out var idProp));
            Assert.Equal("integer", idProp.GetProperty("type").GetString());
            Assert.True(props.TryGetProperty("name", out var nameProp));
            Assert.Equal("string", nameProp.GetProperty("type").GetString());
            Assert.True(jsonElement.TryGetProperty("required", out var requiredProp));
            Assert.Contains(requiredProp.EnumerateArray(), e => e.GetString() == "id");
        }

        [Fact]
        public void ToJsonElement_SerializesArraySchema_Correctly()
        {
            var schema = new McpJsonSchema
            {
                Type = "array",
                Items = new McpJsonSchema { Type = "string" }
            };

            var jsonElement = schema.ToJsonElement();

            Assert.Equal("array", jsonElement.GetProperty("type").GetString());
            Assert.True(jsonElement.TryGetProperty("items", out var itemsProp));
            Assert.Equal("string", itemsProp.GetProperty("type").GetString());
        }
    }
}
