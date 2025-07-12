namespace MCPfiedSwagger.Models;

public class McpJsonSchema
{
    public string Type { get; set; }
    public string Format { get; set; }
    public string Description { get; set; }
    public McpJsonSchema Items { get; set; }
    public Dictionary<string, McpJsonSchema> Properties { get; set; }
    public string[] Required { get; set; }
}
