using MCPfiedSwagger.Models;
using MCPfiedSwagger.Services;

namespace MCPfiedSwagger.Tests;

public class MCPSwaggerServiceTests
{
    [Fact]
    public void Version_ShouldReturnCorrectVersion()
    {
        // Arrange & Act
        var version = MCPSwaggerService.Version;

        // Assert
        Assert.Equal("1.0.0", version);
    }

    [Fact]
    public void Constructor_ShouldInitializeSuccessfully()
    {
        // Arrange & Act
        var service = new MCPSwaggerService();

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public void GenerateMCPToolsFromSwagger_WithValidJson_ShouldReturnTools()
    {
        // Arrange
        var service = new MCPSwaggerService();
        var swaggerJson = """
            {
                "openapi": "3.0.0",
                "info": {
                    "title": "Test API",
                    "version": "1.0.0"
                },
                "paths": {
                    "/users": {
                        "get": {
                            "summary": "Get users"
                        }
                    },
                    "/posts": {
                        "post": {
                            "summary": "Create post"
                        }
                    }
                }
            }
            """;

        // Act
        var tools = service.GenerateMCPToolsFromSwagger(swaggerJson);

        // Assert
        Assert.NotNull(tools);
        Assert.Equal(2, tools.Count);
    }
}
