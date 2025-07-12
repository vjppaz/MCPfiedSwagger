using MCPfiedSwagger.Parser.OpenApiSchemes.Handlers;
using Microsoft.OpenApi.Models;

namespace MCPfiedSwagger.Tests.Parser.OpenApiSchemes.Handlers
{
    public class DefaultSchemaHandlerTests
    {
        private readonly DefaultSchemaHandler _handler;

        public DefaultSchemaHandlerTests()
        {
            _handler = new DefaultSchemaHandler();
        }

        [Fact]
        public void CanHandle_WithAnySchema_ShouldReturnTrue()
        {
            // Arrange & Act & Assert
            Assert.True(_handler.CanHandle(new OpenApiSchema { Type = "string" }));
            Assert.True(_handler.CanHandle(new OpenApiSchema { Type = "object" }));
            Assert.True(_handler.CanHandle(new OpenApiSchema { Type = "unknown" }));
            Assert.True(_handler.CanHandle(new OpenApiSchema()));
        }

        [Fact]
        public void Convert_WithKnownType_ShouldReturnSchemaWithSameType()
        {
            // Arrange
            var schema = new OpenApiSchema
            {
                Type = "boolean",
                Description = "Test boolean field"
            };
            var apiDocument = new OpenApiDocument();

            // Act
            var result = _handler.Convert(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("boolean", result.Type);
            Assert.Equal("Test boolean field", result.Description);
        }

        [Fact]
        public void Convert_WithNullType_ShouldDefaultToString()
        {
            // Arrange
            var schema = new OpenApiSchema
            {
                Description = "Schema without type"
            };
            var apiDocument = new OpenApiDocument();

            // Act
            var result = _handler.Convert(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("string", result.Type);
            Assert.Equal("Schema without type", result.Description);
        }

        [Fact]
        public void Convert_WithNoDescription_ShouldUseDefaultDescription()
        {
            // Arrange
            var schema = new OpenApiSchema { Type = "number" };
            var apiDocument = new OpenApiDocument();

            // Act
            var result = _handler.Convert(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("number", result.Type);
            Assert.Equal("No Description", result.Description);
        }

        [Fact]
        public void Convert_WithEmptySchema_ShouldReturnDefaultStringSchema()
        {
            // Arrange
            var schema = new OpenApiSchema();
            var apiDocument = new OpenApiDocument();

            // Act
            var result = _handler.Convert(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("string", result.Type);
            Assert.Equal("No Description", result.Description);
        }
    }
}
