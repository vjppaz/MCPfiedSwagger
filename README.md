# MCPfiedSwagger

A .NET library that bridges Swagger/OpenAPI specifications with the Model Context Protocol (MCP), enabling seamless integration of REST APIs as MCP tools.

## Features

- 🔄 **Swagger to MCP Conversion**: Automatically convert Swagger/OpenAPI specifications into MCP-compatible tools
- 🌐 **MCP Client Integration**: Built-in MCP client for communicating with MCP servers
- 📊 **Type-Safe Models**: Strongly-typed C# models for both Swagger and MCP specifications
- ⚡ **Async/Await Support**: Full asynchronous programming support
- 🔧 **Extensible Architecture**: Interface-based design for easy customization and testing

## Installation

Install the package via NuGet Package Manager:

```bash
dotnet add package MCPfiedSwagger
```

Or via Package Manager Console:

```powershell
Install-Package MCPfiedSwagger
```

## Quick Start

### Basic Usage

```csharp
using MCPfiedSwagger;
using MCPfiedSwagger.Models;

// Initialize the service
var service = new MCPSwaggerService();

// Configure MCP server connection
var mcpConfig = new MCPConfig
{
    ServerUrl = "https://your-mcp-server.com/api",
    ApiVersion = "1.0",
    TimeoutMs = 30000,
    AuthToken = "your-auth-token" // Optional
};

// Your Swagger JSON specification
string swaggerJson = File.ReadAllText("swagger.json");

// Register Swagger spec with MCP server
bool success = await service.RegisterSwaggerWithMCPAsync(swaggerJson, mcpConfig);

if (success)
{
    Console.WriteLine("Swagger specification successfully registered with MCP server!");
}
```

### Generate MCP Tools Without Server Connection

```csharp
using MCPfiedSwagger;

var service = new MCPSwaggerService();
string swaggerJson = File.ReadAllText("swagger.json");

// Generate MCP tools from Swagger spec
var mcpTools = service.GenerateMCPToolsFromSwagger(swaggerJson);

foreach (var tool in mcpTools)
{
    Console.WriteLine($"Generated MCP tool: {tool}");
}
```

### Advanced Usage with Dependency Injection

```csharp
using MCPfiedSwagger;
using MCPfiedSwagger.Interfaces;
using MCPfiedSwagger.Services;
using Microsoft.Extensions.DependencyInjection;

// Configure services
var services = new ServiceCollection();
services.AddTransient<IMCPClient, MCPClient>();
services.AddTransient<ISwaggerProcessor, SwaggerProcessor>();
services.AddTransient<MCPSwaggerService>();

var serviceProvider = services.BuildServiceProvider();

// Use the service
var mcpSwaggerService = serviceProvider.GetRequiredService<MCPSwaggerService>();
```

## API Reference

### MCPSwaggerService

The main service class providing core functionality.

#### Methods

- `RegisterSwaggerWithMCPAsync(string swaggerJson, MCPConfig mcpConfig)`: Register Swagger spec with MCP server
- `GenerateMCPToolsFromSwagger(string swaggerJson)`: Generate MCP tools from Swagger specification
- `DisconnectAsync()`: Disconnect from MCP server

### MCPConfig

Configuration class for MCP server connection.

#### Properties

- `ServerUrl`: MCP server endpoint URL
- `ApiVersion`: API version for MCP communication (default: "1.0")
- `TimeoutMs`: Request timeout in milliseconds (default: 30000)
- `AuthToken`: Optional authentication token

### SwaggerSpec

Represents a parsed Swagger/OpenAPI specification.

#### Properties

- `OpenApiVersion`: OpenAPI version (default: "3.0.0")
- `Info`: API information
- `Servers`: List of server URLs
- `Paths`: API paths and operations

## Examples

### Working with Custom Swagger Specifications

```csharp
using MCPfiedSwagger;
using MCPfiedSwagger.Models;

var service = new MCPSwaggerService();

// Create Swagger spec programmatically
var swaggerSpec = new SwaggerSpec
{
    OpenApiVersion = "3.0.1",
    Info = new ApiInfo
    {
        Title = "My API",
        Version = "1.0.0",
        Description = "A sample API"
    },
    Servers = new List<ServerInfo>
    {
        new ServerInfo { Url = "https://api.example.com", Description = "Production server" }
    }
};

// Convert to JSON and process
string swaggerJson = JsonSerializer.Serialize(swaggerSpec);
var mcpTools = service.GenerateMCPToolsFromSwagger(swaggerJson);
```

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

- 📧 Email: [Create an issue](https://github.com/vjppaz/MCPfiedSwagger/issues)
- 📖 Documentation: [Wiki](https://github.com/vjppaz/MCPfiedSwagger/wiki)
- 💬 Discussions: [GitHub Discussions](https://github.com/vjppaz/MCPfiedSwagger/discussions)

## Changelog

### Version 1.0.0
- Initial release
- Basic Swagger to MCP conversion
- MCP client implementation
- Async/await support