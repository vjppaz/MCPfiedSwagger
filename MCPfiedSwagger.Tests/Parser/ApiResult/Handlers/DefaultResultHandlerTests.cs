using MCPfiedSwagger.Parser.ApiResult.Handlers;
using ModelContextProtocol.Protocol;

namespace MCPfiedSwagger.Tests.Parser.ApiResult.Handlers
{
    public class DefaultResultHandlerTests
    {
        private readonly DefaultResultHandler _handler;

        public DefaultResultHandlerTests()
        {
            _handler = new DefaultResultHandler();
        }

        [Fact]
        public void CanHandle_WithAnyResult_ShouldReturnTrue()
        {
            // Arrange & Act & Assert
            Assert.True(_handler.CanHandle("string"));
            Assert.True(_handler.CanHandle(42));
            Assert.True(_handler.CanHandle(new { Test = "value" }));
            Assert.True(_handler.CanHandle(new List<int> { 1, 2, 3 }));
        }

        [Fact]
        public void CanHandle_WithNullResult_ShouldReturnTrue()
        {
            // Arrange
            object? result = null;

            // Act
            var canHandle = _handler.CanHandle(result!);

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public async Task HandleAsync_WithNullResult_ShouldReturnErrorResult()
        {
            // Arrange
            object? nullResult = null;

            // Act
            var result = await _handler.HandleAsync(nullResult!, false);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsError);
            Assert.NotNull(result.Content);
            Assert.Single(result.Content);
            Assert.IsType<TextContentBlock>(result.Content[0]);

            var textContent = (TextContentBlock)result.Content[0];
            Assert.Equal("No result returned.", textContent.Text);
            Assert.Equal("text", textContent.Type);
        }

        [Fact]
        public async Task HandleAsync_WithValidObject_ShouldReturnJsonSerializedResult()
        {
            // Arrange
            var testObject = new { Name = "Test", Value = 42, IsActive = true };

            // Act
            var result = await _handler.HandleAsync(testObject, false);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsError != true);
            Assert.NotNull(result.Content);
            Assert.Single(result.Content);
            Assert.IsType<TextContentBlock>(result.Content[0]);

            var textContent = (TextContentBlock)result.Content[0];
            Assert.Contains("Test", textContent.Text);
            Assert.Contains("42", textContent.Text);
            Assert.Contains("true", textContent.Text);
            Assert.Equal("text", textContent.Type);
        }

        [Fact]
        public async Task HandleAsync_WithPrimitiveType_ShouldReturnJsonSerializedResult()
        {
            // Arrange
            var intResult = 123;

            // Act
            var result = await _handler.HandleAsync(intResult, false);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsError != true);
            Assert.NotNull(result.Content);
            Assert.Single(result.Content);
            Assert.IsType<TextContentBlock>(result.Content[0]);

            var textContent = (TextContentBlock)result.Content[0];
            Assert.Equal("123", textContent.Text);
            Assert.Equal("text", textContent.Type);
        }

        [Fact]
        public async Task HandleAsync_WithCollection_ShouldReturnJsonSerializedResult()
        {
            // Arrange
            var listResult = new List<string> { "first", "second", "third" };

            // Act
            var result = await _handler.HandleAsync(listResult, false);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsError != true);
            Assert.NotNull(result.Content);
            Assert.Single(result.Content);
            Assert.IsType<TextContentBlock>(result.Content[0]);

            var textContent = (TextContentBlock)result.Content[0];
            Assert.Contains("first", textContent.Text);
            Assert.Contains("second", textContent.Text);
            Assert.Contains("third", textContent.Text);
            Assert.Equal("text", textContent.Type);
        }
    }
}
