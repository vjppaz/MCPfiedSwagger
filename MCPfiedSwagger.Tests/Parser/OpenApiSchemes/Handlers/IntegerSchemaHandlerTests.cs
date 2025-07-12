using MCPfiedSwagger.Parser.OpenApiSchemes.Handlers;
using Microsoft.OpenApi.Models;

namespace MCPfiedSwagger.Tests.Parser.OpenApiSchemes.Handlers
{
    public class IntegerSchemaHandlerTests
    {
        private readonly IntegerSchemaHandler _handler;

        public IntegerSchemaHandlerTests()
        {
            _handler = new IntegerSchemaHandler();
        }

        [Fact]
        public void CanHandle_WithIntegerSchema_ShouldReturnTrue()
        {
            // Arrange
            var schema = new OpenApiSchema { Type = "integer" };

            // Act
            var canHandle = _handler.CanHandle(schema);

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public void CanHandle_WithLongSchema_ShouldReturnTrue()
        {
            // Arrange
            var schema = new OpenApiSchema { Type = "long" };

            // Act
            var canHandle = _handler.CanHandle(schema);

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public void CanHandle_WithNonIntegerSchema_ShouldReturnFalse()
        {
            // Arrange
            var schema = new OpenApiSchema { Type = "string" };

            // Act
            var canHandle = _handler.CanHandle(schema);

            // Assert
            Assert.False(canHandle);
        }

        [Fact]
        public void Convert_WithIntegerSchema_ShouldReturnNumberMcpJsonSchema()
        {
            // Arrange
            var schema = new OpenApiSchema
            {
                Type = "integer",
                Description = "Test integer field"
            };
            var apiDocument = new OpenApiDocument();

            // Act
            var result = _handler.Convert(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("number", result.Type);
            Assert.Equal("Test integer field", result.Description);
        }

        [Fact]
        public void Convert_WithLongSchema_ShouldReturnNumberMcpJsonSchema()
        {
            // Arrange
            var schema = new OpenApiSchema
            {
                Type = "long",
                Description = "Test long field"
            };
            var apiDocument = new OpenApiDocument();

            // Act
            var result = _handler.Convert(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("number", result.Type);
            Assert.Equal("Test long field", result.Description);
        }

        [Fact]
        public void Convert_WithNoDescription_ShouldUseDefaultDescription()
        {
            // Arrange
            var schema = new OpenApiSchema { Type = "integer" };
            var apiDocument = new OpenApiDocument();

            // Act
            var result = _handler.Convert(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("number", result.Type);
            Assert.Equal("No Description", result.Description);
        }
    }
}
