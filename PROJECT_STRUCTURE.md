# MCPfiedSwagger Project Structure

This document outlines the structure of the MCPfiedSwagger .NET library project.

## Project Overview

MCPfiedSwagger is a .NET 9.0 class library that bridges Swagger/OpenAPI specifications with the Model Context Protocol (MCP).

## Solution Structure

```
MCPfiedSwagger/
├── MCPfiedSwagger.sln                    # Solution file
├── README.md                             # Project documentation
├── LICENSE                               # MIT License
├── .gitignore                           # Git ignore rules
│
├── MCPfiedSwagger/                       # Main library project
│   ├── MCPfiedSwagger.csproj            # Project file with NuGet metadata
│   ├── MCPSwaggerService.cs             # Main service class
│   │
│   ├── Models/                          # Data models
│   │   ├── MCPConfig.cs                 # MCP server configuration
│   │   └── SwaggerSpec.cs               # Swagger specification models
│   │
│   ├── Interfaces/                      # Contracts and interfaces
│   │   └── IMCPClient.cs                # MCP client and processor interfaces
│   │
│   └── Services/                        # Implementation classes
│       ├── MCPClient.cs                 # MCP client implementation
│       └── SwaggerProcessor.cs          # Swagger processing logic
│
├── MCPfiedSwagger.Tests/                # Unit test project
│   ├── MCPfiedSwagger.Tests.csproj     # Test project file
│   ├── MCPSwaggerServiceTests.cs       # Tests for main service
│   └── SwaggerProcessorTests.cs        # Tests for Swagger processor
│
└── MCPfiedSwagger.Example/              # Example/demo application
    ├── MCPfiedSwagger.Example.csproj   # Console app project file
    └── Program.cs                       # Demo application code
```

## Key Components

### Core Library (`MCPfiedSwagger`)

- **MCPSwaggerService**: Main entry point for library functionality
- **Models**: Strongly-typed data models for MCP and Swagger configurations
- **Interfaces**: Contracts defining the behavior of MCP clients and processors
- **Services**: Concrete implementations of the interfaces

### Test Project (`MCPfiedSwagger.Tests`)

- Comprehensive unit tests using xUnit framework
- Tests cover all major functionality and edge cases
- Includes both positive and negative test scenarios

### Example Project (`MCPfiedSwagger.Example`)

- Console application demonstrating library usage
- Shows how to parse Swagger specs and generate MCP tools
- Provides sample code for integration scenarios

## Building the Project

```bash
# Build the entire solution
dotnet build

# Run all tests
dotnet test

# Run the example application
dotnet run --project MCPfiedSwagger.Example

# Create NuGet package
dotnet pack MCPfiedSwagger/MCPfiedSwagger.csproj
```

## Key Features

1. **Swagger to MCP Conversion**: Convert OpenAPI specs to MCP tools
2. **Type Safety**: Strongly-typed C# models throughout
3. **Async Support**: Full async/await pattern implementation
4. **Extensibility**: Interface-based design for easy customization
5. **Testing**: Comprehensive test coverage
6. **Documentation**: XML documentation comments for IntelliSense

## Dependencies

- **.NET 9.0**: Target framework
- **System.Text.Json**: For JSON serialization
- **xUnit**: For unit testing (test project only)

## Usage Examples

See the `MCPfiedSwagger.Example` project for complete usage examples, or refer to the README.md for quick start guides.
