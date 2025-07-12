using MCPfiedSwagger.Parser.OpenApiSchemes.Handlers;
using Microsoft.OpenApi.Models;

namespace MCPfiedSwagger.Tests.Parser.OpenApiSchemes.Handlers
{
    public class FormatSchemaHandlerTests
    {
        private readonly FormatSchemaHandler _handler;

        public FormatSchemaHandlerTests()
        {
            _handler = new FormatSchemaHandler();
        }

        [Fact]
        public void CanHandle_WithFormatSchema_ShouldReturnTrue()
        {
            // Arrange
            var schema = new OpenApiSchema
            {
                Type = "string",
                Format = "date-time"
            };

            // Act
            var canHandle = _handler.CanHandle(schema);

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public void CanHandle_WithNonFormatSchema_ShouldReturnFalse()
        {
            // Arrange
            var schema = new OpenApiSchema { Type = "string" };

            // Act
            var canHandle = _handler.CanHandle(schema);

            // Assert
            Assert.False(canHandle);
        }

        [Fact]
        public void Convert_WithDateTimeFormat_ShouldReturnSchemaWithFormat()
        {
            // Arrange
            var schema = new OpenApiSchema
            {
                Type = "string",
                Format = "date-time",
                Description = "DateTime field"
            };
            var apiDocument = new OpenApiDocument();

            // Act
            var result = _handler.Convert(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("string", result.Type);
            Assert.Equal("date-time", result.Format);
            Assert.Equal("DateTime field", result.Description);
        }

        [Fact]
        public void Convert_WithEmailFormat_ShouldReturnSchemaWithFormat()
        {
            // Arrange
            var schema = new OpenApiSchema
            {
                Type = "string",
                Format = "email",
                Description = "Email field"
            };
            var apiDocument = new OpenApiDocument();

            // Act
            var result = _handler.Convert(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("string", result.Type);
            Assert.Equal("email", result.Format);
            Assert.Equal("Email field", result.Description);
        }

        [Fact]
        public void Convert_WithNoType_ShouldDefaultToString()
        {
            // Arrange
            var schema = new OpenApiSchema
            {
                Format = "uuid",
                Description = "UUID field"
            };
            var apiDocument = new OpenApiDocument();

            // Act
            var result = _handler.Convert(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("string", result.Type);
            Assert.Equal("uuid", result.Format);
            Assert.Equal("UUID field", result.Description);
        }

        [Fact]
        public void Convert_WithNoDescription_ShouldUseDefaultDescription()
        {
            // Arrange
            var schema = new OpenApiSchema
            {
                Type = "number",
                Format = "float"
            };
            var apiDocument = new OpenApiDocument();

            // Act
            var result = _handler.Convert(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("number", result.Type);
            Assert.Equal("float", result.Format);
            Assert.Equal("No Description", result.Description);
        }
    }
}
