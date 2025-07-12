using MCPfiedSwagger.Parser.OpenApiSchemes.Handlers;
using Microsoft.OpenApi.Models;

namespace MCPfiedSwagger.Tests.Parser.OpenApiSchemes.Handlers
{
    public class ArraySchemaHandlerTests
    {
        private readonly ArraySchemaHandler _handler;

        public ArraySchemaHandlerTests()
        {
            _handler = new ArraySchemaHandler();
        }

        [Fact]
        public void CanHandle_WithArraySchema_ShouldReturnTrue()
        {
            // Arrange
            var schema = new OpenApiSchema { Type = "array" };

            // Act
            var canHandle = _handler.CanHandle(schema);

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public void CanHandle_WithNonArraySchema_ShouldReturnFalse()
        {
            // Arrange
            var schema = new OpenApiSchema { Type = "object" };

            // Act
            var canHandle = _handler.CanHandle(schema);

            // Assert
            Assert.False(canHandle);
        }

        [Fact]
        public void Convert_WithArraySchemaWithItems_ShouldReturnValidMcpJsonSchema()
        {
            // Arrange
            var schema = new OpenApiSchema
            {
                Type = "array",
                Description = "Test array",
                Items = new OpenApiSchema { Type = "string", Description = "String item" }
            };
            var apiDocument = new OpenApiDocument();

            // Act
            var result = _handler.Convert(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("array", result.Type);
            Assert.Equal("Test array", result.Description);
            Assert.NotNull(result.Items);
            Assert.Equal("string", result.Items.Type);
            Assert.Equal("String item", result.Items.Description);
        }

        [Fact]
        public void Convert_WithArraySchemaNoItems_ShouldReturnArrayWithNullItems()
        {
            // Arrange
            var schema = new OpenApiSchema
            {
                Type = "array",
                Description = "Array without items"
            };
            var apiDocument = new OpenApiDocument();

            // Act
            var result = _handler.Convert(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("array", result.Type);
            Assert.Equal("Array without items", result.Description);
            Assert.Null(result.Items);
        }

        [Fact]
        public void Convert_WithArraySchemaNoDescription_ShouldUseDefaultDescription()
        {
            // Arrange
            var schema = new OpenApiSchema
            {
                Type = "array",
                Items = new OpenApiSchema { Type = "integer" }
            };
            var apiDocument = new OpenApiDocument();

            // Act
            var result = _handler.Convert(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("array", result.Type);
            Assert.Equal("No Description", result.Description);
            Assert.NotNull(result.Items);
            Assert.Equal("number", result.Items.Type); // IntegerSchemaHandler returns "number"
        }
    }
}
