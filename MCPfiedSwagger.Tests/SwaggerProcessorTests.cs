using MCPfiedSwagger.Services;
using MCPfiedSwagger.Models;

namespace MCPfiedSwagger.Tests;

public class SwaggerProcessorTests
{
    private readonly SwaggerProcessor _processor;

    public SwaggerProcessorTests()
    {
        _processor = new SwaggerProcessor();
    }

    [Fact]
    public void ParseSwaggerSpec_WithValidJson_ShouldReturnSwaggerSpec()
    {
        // Arrange
        var swaggerJson = """
            {
                "openapi": "3.0.0",
                "info": {
                    "title": "Test API",
                    "version": "1.0.0",
                    "description": "A test API"
                },
                "servers": [
                    {
                        "url": "https://api.example.com",
                        "description": "Production server"
                    }
                ],
                "paths": {
                    "/users": {
                        "get": {
                            "summary": "Get users"
                        }
                    }
                }
            }
            """;

        // Act
        var spec = _processor.ParseSwaggerSpec(swaggerJson);

        // Assert
        Assert.NotNull(spec);
        Assert.Equal("3.0.0", spec.OpenApiVersion);
        Assert.NotNull(spec.Info);
        Assert.Equal("Test API", spec.Info.Title);
        Assert.Equal("1.0.0", spec.Info.Version);
        Assert.Equal("A test API", spec.Info.Description);
        Assert.Single(spec.Servers);
        Assert.Equal("https://api.example.com", spec.Servers[0].Url);
        Assert.Single(spec.Paths);
        Assert.True(spec.Paths.ContainsKey("/users"));
    }

    [Fact]
    public void ParseSwaggerSpec_WithInvalidJson_ShouldThrowArgumentException()
    {
        // Arrange
        var invalidJson = "{ invalid json }";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _processor.ParseSwaggerSpec(invalidJson));
    }

    [Fact]
    public void GenerateMCPTools_WithSwaggerSpec_ShouldReturnTools()
    {
        // Arrange
        var spec = new SwaggerSpec
        {
            Paths = new Dictionary<string, object>
            {
                { "/users", new { } },
                { "/posts", new { } },
                { "/comments", new { } }
            }
        };

        // Act
        var tools = _processor.GenerateMCPTools(spec);

        // Assert
        Assert.NotNull(tools);
        Assert.Equal(3, tools.Count);
    }

    [Fact]
    public void ConvertToMCPFormat_WithSwaggerSpec_ShouldReturnMCPFormat()
    {
        // Arrange
        var spec = new SwaggerSpec
        {
            OpenApiVersion = "3.0.1",
            Info = new ApiInfo { Title = "Test API", Version = "1.0.0" },
            Servers = new List<ServerInfo>
            {
                new() { Url = "https://api.example.com", Description = "Production" }
            },
            Paths = new Dictionary<string, object>
            {
                { "/test", new { } }
            }
        };

        // Act
        var mcpFormat = _processor.ConvertToMCPFormat(spec);

        // Assert
        Assert.NotNull(mcpFormat);
    }
}
