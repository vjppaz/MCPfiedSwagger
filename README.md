# MCPfiedSwagger 🚀

An ASP.Net extension that generates a Model Context Protocol (MCP) server, enabling seamless conversion of existing REST APIs as MCP tools.

[![NuGet](https://img.shields.io/nuget/v/MCPfiedSwagger.svg?style=flat-square&logo=nuget)](https://www.nuget.org/packages/MCPfiedSwagger)

## 📦 Installation

Install the `MCPfiedSwagger` NuGet package from your preferred NuGet source:

```shell
dotnet add package MCPfiedSwagger --version 0.0.1-draft
```

Or via the NuGet Package Manager:

```
PM> Install-Package MCPfiedSwagger -Version 0.0.1-draft
```

## ⚙️ Usage

1. **Add the MCPfiedSwagger service** in your ASP.NET Core project. In your `Program.cs`, register the service after adding controllers and Swagger:

    ```csharp
    builder.Services.AddMCPfiedSwagger();
    ```

2. **Enable the middleware** in your HTTP pipeline, after mapping controllers:

    ```csharp
    app.UseMCPfiedSwagger();
    ```

3. **Configure Swagger (optional):**  
   By default, the Swagger/OpenAPI document is named `"v1"`. To specify a different document name, pass it as an argument:

    ```csharp
    app.UseMCPfiedSwagger("v1");
    ```

4. **Specify the MCP endpoint (optional):**  
   The default MCP endpoint is `/mcp`. To change it, provide the desired path:

    ```csharp
    app.UseMCPfiedSwagger("v1", "/custom-mcp-endpoint");
    ```

## 📝 Example

A minimal `Program.cs` setup:

```csharp
using MCPfiedSwagger.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMCPfiedSwagger();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

// Enable MCP endpoint at /mcp for the "v1" Swagger document
app.UseMCPfiedSwagger();

app.Run();
```

## ▶️ Running the Example Project

1. **Restore dependencies and build:**

    ```shell
    dotnet restore
    dotnet build
    ```

2. **Run the project:**

    ```shell
    dotnet run --project MCPfiedSwagger.Example
    ```

3. **Access the Swagger UI:**  
   Open 🌐 `http://localhost:<port>/swagger` in your browser.

4. **Use the MCP endpoint:**  
   The MCP endpoint is available at 🔗 `http://localhost:<port>/mcp`.  
   You can POST MCP protocol requests to this endpoint.  
   The endpoint will process requests according to your REST API and return MCP-compliant responses.

## 💡 Notes

- The NuGet package depends on `ModelContextProtocol.AspNetCore` and `Swashbuckle.AspNetCore`.
- Make sure your project targets `.NET 9.0` or compatible.
- For more details, see the [project repository](https://github.com/vjppaz/MCPfiedSwagger).