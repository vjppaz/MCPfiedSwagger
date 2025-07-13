using MCPfiedSwagger.Parser.ApiResult;
using Microsoft.AspNetCore.Mvc;
using ModelContextProtocol.Protocol;

namespace MCPfiedSwagger.Tests.Parser.ApiResult
{
    public class ApiResultConverterTests
    {
        [Fact]
        public async Task ConvertAsync_WithOkObjectResult_ShouldReturnSuccessCallToolResult()
        {
            // Arrange
            var testObject = new { Message = "Test", Value = 42 };
            var okResult = new OkObjectResult(testObject);

            // Act
            var result = await ApiResultConverter.ConvertAsync(okResult, false);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsError != true);
            Assert.NotNull(result.Content);
            Assert.Single(result.Content);
            Assert.IsType<TextContentBlock>(result.Content[0]);
            var textContent = (TextContentBlock)result.Content[0];
            Assert.Contains("Test", textContent.Text);
            Assert.Contains("42", textContent.Text);
        }

        [Fact]
        public async Task ConvertAsync_WithBadRequestObjectResult_ShouldReturnErrorCallToolResult()
        {
            // Arrange
            var errorObject = new { Error = "Validation failed" };
            var badResult = new BadRequestObjectResult(errorObject);

            // Act
            var result = await ApiResultConverter.ConvertAsync(badResult, false);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsError);
            Assert.NotNull(result.Content);
            Assert.Single(result.Content);
            Assert.IsType<TextContentBlock>(result.Content[0]);
            var textContent = (TextContentBlock)result.Content[0];
            Assert.Contains("Validation failed", textContent.Text);
        }

        [Fact]
        public async Task ConvertAsync_WithStringResult_ShouldReturnTextCallToolResult()
        {
            // Arrange
            var stringResult = "Simple string response";

            // Act
            var result = await ApiResultConverter.ConvertAsync(stringResult, false);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsError != true);
            Assert.NotNull(result.Content);
            Assert.Single(result.Content);
            Assert.IsType<TextContentBlock>(result.Content[0]);
            var textContent = (TextContentBlock)result.Content[0];
            Assert.Equal("Simple string response", textContent.Text);
        }

        [Fact]
        public async Task ConvertAsync_WithTask_ShouldReturnAppropriateResult()
        {
            // Arrange
            var taskResult = Task.FromResult("Task completed");

            // Act
            var result = await ApiResultConverter.ConvertAsync(taskResult, false);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsError != true);
            Assert.NotNull(result.Content);
            Assert.Single(result.Content);
            Assert.IsType<TextContentBlock>(result.Content[0]);
            var textContent = (TextContentBlock)result.Content[0];
            Assert.Contains("Task completed", textContent.Text);
        }

        [Fact]
        public async Task ConvertAsync_WithUnknownType_ShouldReturnDefaultResult()
        {
            // Arrange
            var unknownResult = new { UnknownProperty = "test" };

            // Act
            var result = await ApiResultConverter.ConvertAsync(unknownResult, false);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsError != true); // DefaultResultHandler returns non-error
            Assert.NotNull(result.Content);
            Assert.Single(result.Content);
            Assert.IsType<TextContentBlock>(result.Content[0]);
        }
    }
}
