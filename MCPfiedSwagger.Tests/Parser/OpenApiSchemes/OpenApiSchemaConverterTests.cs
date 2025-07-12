using MCPfiedSwagger.Models;
using MCPfiedSwagger.Parser.OpenApiSchemes;
using Microsoft.OpenApi.Models;

namespace MCPfiedSwagger.Tests.Parser.OpenApiSchemes
{
    public class OpenApiSchemaConverterTests
    {
        [Fact]
        public void ConvertSchema_WithObjectSchema_ShouldReturnObjectMcpJsonSchema()
        {
            // Arrange
            var schema = new OpenApiSchema
            {
                Type = "object",
                Description = "Test object schema",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["name"] = new OpenApiSchema { Type = "string", Description = "Name property" },
                    ["age"] = new OpenApiSchema { Type = "integer", Description = "Age property" }
                },
                Required = new HashSet<string> { "name" }
            };
            var apiDocument = new OpenApiDocument();

            // Act
            var result = OpenApiSchemaConverter.ConvertSchema(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("object", result.Type);
            Assert.Equal("Test object schema", result.Description);
            Assert.NotNull(result.Properties);
            Assert.Equal(2, result.Properties.Count);
            Assert.True(result.Properties.ContainsKey("name"));
            Assert.True(result.Properties.ContainsKey("age"));
            Assert.NotNull(result.Required);
            Assert.Single(result.Required);
            Assert.Equal("name", result.Required[0]);
        }

        [Fact]
        public void ConvertSchema_WithArraySchema_ShouldReturnArrayMcpJsonSchema()
        {
            // Arrange
            var schema = new OpenApiSchema
            {
                Type = "array",
                Description = "Test array schema",
                Items = new OpenApiSchema { Type = "string" }
            };
            var apiDocument = new OpenApiDocument();

            // Act
            var result = OpenApiSchemaConverter.ConvertSchema(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("array", result.Type);
            Assert.Equal("Test array schema", result.Description);
            Assert.NotNull(result.Items);
            Assert.Equal("string", result.Items.Type);
        }

        [Fact]
        public void ConvertSchema_WithStringSchema_ShouldReturnStringMcpJsonSchema()
        {
            // Arrange
            var schema = new OpenApiSchema
            {
                Type = "string",
                Description = "Test string schema"
            };
            var apiDocument = new OpenApiDocument();

            // Act
            var result = OpenApiSchemaConverter.ConvertSchema(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("string", result.Type);
            Assert.Equal("Test string schema", result.Description);
        }

        [Fact]
        public void ConvertSchema_WithIntegerSchema_ShouldReturnIntegerMcpJsonSchema()
        {
            // Arrange
            var schema = new OpenApiSchema
            {
                Type = "integer",
                Description = "Test integer schema"
            };
            var apiDocument = new OpenApiDocument();

            // Act
            var result = OpenApiSchemaConverter.ConvertSchema(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("number", result.Type); // IntegerSchemaHandler returns "number"
            Assert.Equal("Test integer schema", result.Description);
        }

        [Fact]
        public void ConvertSchema_WithUnknownSchema_ShouldReturnUnknownMcpJsonSchema()
        {
            // Arrange
            var schema = new OpenApiSchema
            {
                Type = "unknown_type",
                Description = "Unknown schema type"
            };
            var apiDocument = new OpenApiDocument();

            // Act
            var result = OpenApiSchemaConverter.ConvertSchema(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("unknown_type", result.Type); // DefaultSchemaHandler preserves the type
            Assert.Equal("Unknown schema type", result.Description);
        }

        [Fact]
        public void ConvertSchema_WithEmptySchema_ShouldReturnDefaultSchema()
        {
            // Arrange
            var schema = new OpenApiSchema(); // Empty schema, no type set
            var apiDocument = new OpenApiDocument();

            // Act
            var result = OpenApiSchemaConverter.ConvertSchema(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("string", result.Type); // DefaultSchemaHandler defaults to string
            Assert.Equal("No Description", result.Description);
        }
    }
}
