using MCPfiedSwagger.Models;
using MCPfiedSwagger.Parser.SchemaTypes;
using MCPfiedSwagger.Parser.SchemaTypes.Specifics;
using Microsoft.OpenApi.Models;

namespace MCPfiedSwagger.Parser
{
    public class MCPfiedSwaggerParser : IMCPfiedSwaggerParser
    {
        public Dictionary<string, ISchemaTypeParser> SchemaTypeParsers { get; set; }
        public ISchemaTypeParser DefaultSchemaTypeParser { get; set; }

        public MCPfiedSwaggerParser(List<ISchemaTypeParser> parsers, ISchemaTypeParser defaultSchemaTypeParser)
        {
            parsers = parsers ?? throw new ArgumentNullException(nameof(parsers), "Parsers cannot be null");
            SchemaTypeParsers = parsers
                .SelectMany(m => m.TypeIdentifier, (parser, type) => new { Parser = parser, Type = type })
                .ToDictionary(p => p.Type, p => p.Parser);

            DefaultSchemaTypeParser = defaultSchemaTypeParser;
        }

        // Recursively convert OpenApiSchema to a simple JSON schema object
        public McpJsonSchema ConvertSchema(OpenApiSchema schema, OpenApiDocument parentDocument)
        {
            schema = schema ?? throw new ArgumentNullException(nameof(schema), "OpenApiSchema cannot be null");

            if (SchemaTypeParsers.ContainsKey(schema.Type))
                return SchemaTypeParsers[schema.Type].Parse(schema, parentDocument, this);
            else
                return DefaultSchemaTypeParser.Parse(schema, parentDocument, this);
        }

        public static MCPfiedSwaggerParser NewInstance()
        {
            var parsers = new List<ISchemaTypeParser>
            {
                new ObjectSchemaTypeParser(),
                new IntegerSchemaTypeParser(),
                //new StringSchemaTypeParser(),
                //new BooleanSchemaTypeParser(),
                new ArraySchemaTypeParser(),
                //new DateTimeSchemaTypeParser(),
                //new DateSchemaTypeParser(),
                //new FileSchemaTypeParser(),
                //new EnumSchemaTypeParser()
            };
            var defaultParser = new DefaultSchemaTypeParser();
            var parserInstance = new MCPfiedSwaggerParser(parsers, defaultParser);

            return parserInstance;
        }
    }
}
