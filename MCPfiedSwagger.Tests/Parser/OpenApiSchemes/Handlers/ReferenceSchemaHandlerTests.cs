using MCPfiedSwagger.Parser.OpenApiSchemes.Handlers;
using Microsoft.OpenApi.Models;

namespace MCPfiedSwagger.Tests.Parser.OpenApiSchemes.Handlers
{
    public class ReferenceSchemaHandlerTests
    {
        private readonly ReferenceSchemaHandler _handler;

        public ReferenceSchemaHandlerTests()
        {
            _handler = new ReferenceSchemaHandler();
        }

        [Fact]
        public void CanHandle_WithReferenceSchema_ShouldReturnTrue()
        {
            // Arrange
            var schema = new OpenApiSchema
            {
                Reference = new OpenApiReference
                {
                    Id = "TestModel",
                    Type = ReferenceType.Schema
                }
            };

            // Act
            var canHandle = _handler.CanHandle(schema);

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public void CanHandle_WithNonReferenceSchema_ShouldReturnFalse()
        {
            // Arrange
            var schema = new OpenApiSchema { Type = "string" };

            // Act
            var canHandle = _handler.CanHandle(schema);

            // Assert
            Assert.False(canHandle);
        }

        [Fact]
        public void Convert_WithValidReference_ShouldReturnReferencedSchema()
        {
            // Arrange
            var referencedSchema = new OpenApiSchema
            {
                Type = "object",
                Description = "Referenced model",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["id"] = new OpenApiSchema { Type = "integer" }
                }
            };

            var apiDocument = new OpenApiDocument
            {
                Components = new OpenApiComponents
                {
                    Schemas = new Dictionary<string, OpenApiSchema>
                    {
                        ["TestModel"] = referencedSchema
                    }
                }
            };

            var schema = new OpenApiSchema
            {
                Reference = new OpenApiReference
                {
                    Id = "TestModel",
                    Type = ReferenceType.Schema
                }
            };

            // Act
            var result = _handler.Convert(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("object", result.Type);
            Assert.Equal("Referenced model", result.Description);
            Assert.NotNull(result.Properties);
            Assert.True(result.Properties.ContainsKey("id"));
        }

        [Fact]
        public void Convert_WithInvalidReference_ShouldReturnUnknownSchema()
        {
            // Arrange
            var apiDocument = new OpenApiDocument
            {
                Components = new OpenApiComponents
                {
                    Schemas = new Dictionary<string, OpenApiSchema>()
                }
            };

            var schema = new OpenApiSchema
            {
                Reference = new OpenApiReference
                {
                    Id = "NonExistentModel",
                    Type = ReferenceType.Schema
                }
            };

            // Act
            var result = _handler.Convert(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("unknown", result.Type);
            Assert.Equal("Reference not found", result.Description);
        }

        [Fact]
        public void Convert_WithNullReference_ShouldReturnUnknownSchema()
        {
            // Arrange
            var apiDocument = new OpenApiDocument();
            var schema = new OpenApiSchema { Reference = null };

            // Act
            var result = _handler.Convert(schema, apiDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("unknown", result.Type);
            Assert.Equal("Reference not found", result.Description);
        }
    }
}
