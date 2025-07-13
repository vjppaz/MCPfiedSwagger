using MCPfiedSwagger.Parser.ApiResult.Handlers;
using ModelContextProtocol.Protocol;

namespace MCPfiedSwagger.Tests.Parser.ApiResult.Handlers
{
    public class StringResultHandlerTests
    {
        private readonly StringResultHandler _handler;

        public StringResultHandlerTests()
        {
            _handler = new StringResultHandler();
        }

        [Fact]
        public void CanHandle_WithStringResult_ShouldReturnTrue()
        {
            // Arrange
            var result = "Test string";

            // Act
            var canHandle = _handler.CanHandle(result);

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public void CanHandle_WithNonStringResult_ShouldReturnFalse()
        {
            // Arrange
            var result = 42;

            // Act
            var canHandle = _handler.CanHandle(result);

            // Assert
            Assert.False(canHandle);
        }

        [Fact]
        public async Task HandleAsync_WithValidString_ShouldReturnTextCallToolResult()
        {
            // Arrange
            var stringResult = "Hello, World!";

            // Act
            var result = await _handler.HandleAsync(stringResult, false);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsError != true);
            Assert.NotNull(result.Content);
            Assert.Single(result.Content);
            Assert.IsType<TextContentBlock>(result.Content[0]);

            var textContent = (TextContentBlock)result.Content[0];
            Assert.Equal("Hello, World!", textContent.Text);
            Assert.Equal("text", textContent.Type);
        }

        [Fact]
        public async Task HandleAsync_WithEmptyString_ShouldReturnEmptyTextResult()
        {
            // Arrange
            var stringResult = "";

            // Act
            var result = await _handler.HandleAsync(stringResult, false);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsError != true);
            Assert.NotNull(result.Content);
            Assert.Single(result.Content);

            var textContent = (TextContentBlock)result.Content[0];
            Assert.Equal("", textContent.Text);
            Assert.Equal("text", textContent.Type);
        }
    }
}
