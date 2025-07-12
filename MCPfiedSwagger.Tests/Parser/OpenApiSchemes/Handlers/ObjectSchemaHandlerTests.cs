using MCPfiedSwagger.Parser.OpenApiSchemes.Handlers;
using Microsoft.OpenApi.Models;

namespace MCPfiedSwagger.Tests.Parser.OpenApiSchemes.Handlers
{
    public class ObjectSchemaHandlerTests
    {
        private readonly ObjectSchemaHandler _handler;

        public ObjectSchemaHandlerTests()
        {
            _handler = new ObjectSchemaHandler();
        }

        [Fact]
        public void CanHandle_WithObjectSchema_ShouldReturnTrue()
        {
            // Arrange
            var schema = new OpenApiSchema { Type = "object" };

            // Act
            var canHandle = _handler.CanHandle(schema);

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public void CanHandle_WithNonObjectSchema_ShouldReturnFalse()
        {
            // Arrange
            var schema = new OpenApiSchema { Type = "string" };

            // Act
            var canHandle = _handler.CanHandle(schema);

            // Assert
            Assert.False(canHandle);
        }

        [Fact]
        public void Convert_WithObjectSchemaAndProperties_ShouldReturnValidMcpJsonSchema()
        {
            // Arrange
            var schema = new OpenApiSchema
            {
                Type = "object",
                Description = "Test object",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["name"] = new OpenApiSchema { Type = "string", Description = "Name field" },
                    ["age"] = new OpenApiSchema { Type = "integer", Description = "Age field" }
                },
                Required = new HashSet<string> { "name" }
            };
            var apiDocument = new OpenApiDocument();

            // Act
            var result = _handler.Convert(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("object", result.Type);
            Assert.Equal("Test object", result.Description);
            Assert.NotNull(result.Properties);
            Assert.Equal(2, result.Properties.Count);
            Assert.True(result.Properties.ContainsKey("name"));
            Assert.True(result.Properties.ContainsKey("age"));
            Assert.NotNull(result.Required);
            Assert.Single(result.Required);
            Assert.Equal("name", result.Required[0]);
        }

        [Fact]
        public void Convert_WithObjectSchemaNoProperties_ShouldReturnEmptyObjectSchema()
        {
            // Arrange
            var schema = new OpenApiSchema
            {
                Type = "object",
                Description = "Empty object"
            };
            var apiDocument = new OpenApiDocument();

            // Act
            var result = _handler.Convert(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("object", result.Type);
            Assert.Equal("Empty object", result.Description);
            Assert.NotNull(result.Properties);
            Assert.Empty(result.Properties);
        }

        [Fact]
        public void Convert_WithObjectSchemaNoDescription_ShouldUseDefaultDescription()
        {
            // Arrange
            var schema = new OpenApiSchema
            {
                Type = "object"
            };
            var apiDocument = new OpenApiDocument();

            // Act
            var result = _handler.Convert(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("object", result.Type);
            Assert.Equal("No Description", result.Description);
        }
    }
}
