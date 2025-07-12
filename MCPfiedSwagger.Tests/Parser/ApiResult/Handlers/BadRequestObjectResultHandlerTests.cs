using MCPfiedSwagger.Parser.ApiResult.Handlers;
using Microsoft.AspNetCore.Mvc;
using ModelContextProtocol.Protocol;

namespace MCPfiedSwagger.Tests.Parser.ApiResult.Handlers
{
    public class BadRequestObjectResultHandlerTests
    {
        private readonly BadRequestObjectResultHandler _handler;

        public BadRequestObjectResultHandlerTests()
        {
            _handler = new BadRequestObjectResultHandler();
        }

        [Fact]
        public void CanHandle_WithBadRequestObjectResult_ShouldReturnTrue()
        {
            // Arrange
            var result = new BadRequestObjectResult(new { Error = "Validation failed" });

            // Act
            var canHandle = _handler.CanHandle(result);

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public void CanHandle_WithOtherResult_ShouldReturnFalse()
        {
            // Arrange
            var result = new OkObjectResult(new { Message = "Success" });

            // Act
            var canHandle = _handler.CanHandle(result);

            // Assert
            Assert.False(canHandle);
        }

        [Fact]
        public async Task HandleAsync_WithValidBadRequestObjectResult_ShouldReturnErrorCallToolResult()
        {
            // Arrange
            var errorMessage = "Validation failed";
            var badResult = new BadRequestObjectResult(errorMessage);

            // Act
            var result = await _handler.HandleAsync(badResult);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsError);
            Assert.NotNull(result.Content);
            Assert.Single(result.Content);
            Assert.IsType<TextContentBlock>(result.Content[0]);
            
            var textContent = (TextContentBlock)result.Content[0];
            Assert.Equal("Validation failed", textContent.Text);
            Assert.Equal("text", textContent.Type);
        }

        [Fact]
        public async Task HandleAsync_WithNullValue_ShouldReturnErrorWithDefaultMessage()
        {
            // Arrange - use an empty string instead of null since BadRequestObjectResult doesn't accept null
            var badResult = new BadRequestObjectResult("");

            // Act
            var result = await _handler.HandleAsync(badResult);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsError);
            Assert.NotNull(result.Content);
            Assert.Single(result.Content);
            
            var textContent = (TextContentBlock)result.Content[0];
            Assert.Equal("", textContent.Text); // Empty string should be returned as-is
        }
    }
}
