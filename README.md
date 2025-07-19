# MCPfiedSwagger 🚀

An ASP.NET extension that generates a Model Context Protocol (MCP) server, enabling seamless conversion of existing REST APIs as MCP tools, now with enhanced authorization support and improved schema descriptions.

[![NuGet](https://img.shields.io/nuget/v/MCPfiedSwagger.svg?style=flat-square\&logo=nuget)](https://www.nuget.org/packages/MCPfiedSwagger)

## 📦 Installation

Install the `MCPfiedSwagger` NuGet package from your preferred NuGet source:

```shell
dotnet add package MCPfiedSwagger
```

Or via the NuGet Package Manager:

```
PM> Install-Package MCPfiedSwagger
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

## 🔒 Authorization Support

Authorization support is now integrated. You can enable stateless authorization easily:

```csharp
builder.Services.AddMCPfiedSwagger()
    .AddAuthorization();
```

The authorization process validates controller actions based on `Authorize` attributes and handles unauthorized requests gracefully.

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

### With Authentication Example

1. **Restore dependencies and build:**

```shell
dotnet restore
dotnet build
```

2. **Run the authentication example project:**

```shell
dotnet run --project MCPfiedSwagger.Authenticated
```

3. **Access the Swagger UI:**
   Open 🌐 `http://localhost:<port>/swagger` in your browser.

4. **Test Authorization:**

   * Authenticate via `POST /api/Auth/login` with username `test` and password `password`.
   * Use the received JWT token as your authorization header when connecting to your MCP endpoint.

### Standard Example

Run the original example project:

```shell
dotnet run --project MCPfiedSwagger.Example
```

* **Swagger UI:** 🌐 `http://localhost:<port>/swagger`
* **MCP Endpoint:** 🔗 `http://localhost:<port>/mcp`

## 💡 Notes

* The NuGet package depends on `ModelContextProtocol.AspNetCore` and `Swashbuckle.AspNetCore`.
* Ensure your project targets `.NET 9.0` or compatible.
* For more details, see the [project repository](https://github.com/vjppaz/MCPfiedSwagger).
* Streamable HTTP MCP does not work on `https` for some reason. Still trying to investigate the issue I encountered.