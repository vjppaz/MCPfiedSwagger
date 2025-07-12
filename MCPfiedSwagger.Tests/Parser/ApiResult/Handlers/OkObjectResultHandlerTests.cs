using MCPfiedSwagger.Parser.ApiResult.Handlers;
using Microsoft.AspNetCore.Mvc;
using ModelContextProtocol.Protocol;

namespace MCPfiedSwagger.Tests.Parser.ApiResult.Handlers
{
    public class OkObjectResultHandlerTests
    {
        private readonly OkObjectResultHandler _handler;

        public OkObjectResultHandlerTests()
        {
            _handler = new OkObjectResultHandler();
        }

        [Fact]
        public void CanHandle_WithOkObjectResult_ShouldReturnTrue()
        {
            // Arrange
            var result = new OkObjectResult(new { Message = "Success" });

            // Act
            var canHandle = _handler.CanHandle(result);

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public void CanHandle_WithOtherResult_ShouldReturnFalse()
        {
            // Arrange
            var result = new BadRequestObjectResult(new { Error = "Bad request" });

            // Act
            var canHandle = _handler.CanHandle(result);

            // Assert
            Assert.False(canHandle);
        }

        [Fact]
        public void CanHandle_WithNullResult_ShouldReturnFalse()
        {
            // Arrange
            object? result = null;

            // Act
            var canHandle = _handler.CanHandle(result!);

            // Assert
            Assert.False(canHandle);
        }

        [Fact]
        public async Task HandleAsync_WithValidOkObjectResult_ShouldReturnSuccessCallToolResult()
        {
            // Arrange
            var testData = new { Name = "Test", Id = 123 };
            var okResult = new OkObjectResult(testData);

            // Act
            var result = await _handler.HandleAsync(okResult);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsError != true); // IsError is nullable, not set means success
            Assert.NotNull(result.Content);
            Assert.Single(result.Content);
            Assert.IsType<TextContentBlock>(result.Content[0]);
            
            var textContent = (TextContentBlock)result.Content[0];
            Assert.Contains("Test", textContent.Text);
            Assert.Contains("123", textContent.Text);
            Assert.Equal("text", textContent.Type);
        }

        [Fact]
        public async Task HandleAsync_WithNullValue_ShouldReturnNullString()
        {
            // Arrange
            var okResult = new OkObjectResult(null);

            // Act
            var result = await _handler.HandleAsync(okResult);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsError != true); // IsError is nullable, not set means success
            Assert.NotNull(result.Content);
            Assert.Single(result.Content);
            
            var textContent = (TextContentBlock)result.Content[0];
            Assert.Equal("null", textContent.Text);
        }
    }
}
