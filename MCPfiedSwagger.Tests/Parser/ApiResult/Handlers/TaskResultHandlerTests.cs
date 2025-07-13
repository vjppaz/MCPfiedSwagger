using MCPfiedSwagger.Parser.ApiResult.Handlers;
using ModelContextProtocol.Protocol;

namespace MCPfiedSwagger.Tests.Parser.ApiResult.Handlers
{
    public class TaskResultHandlerTests
    {
        private readonly TaskResultHandler _handler;

        public TaskResultHandlerTests()
        {
            _handler = new TaskResultHandler();
        }

        [Fact]
        public void CanHandle_WithTaskResult_ShouldReturnTrue()
        {
            // Arrange
            var result = Task.FromResult("test");

            // Act
            var canHandle = _handler.CanHandle(result);

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public void CanHandle_WithCompletedTask_ShouldReturnTrue()
        {
            // Arrange
            var result = Task.CompletedTask;

            // Act
            var canHandle = _handler.CanHandle(result);

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public void CanHandle_WithNonTaskResult_ShouldReturnFalse()
        {
            // Arrange
            var result = "not a task";

            // Act
            var canHandle = _handler.CanHandle(result);

            // Assert
            Assert.False(canHandle);
        }

        [Fact]
        public async Task HandleAsync_WithTaskOfString_ShouldReturnSerializedResult()
        {
            // Arrange
            var taskResult = Task.FromResult("Hello from task");

            // Act
            var result = await _handler.HandleAsync(taskResult, false);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsError != true);
            Assert.NotNull(result.Content);
            Assert.Single(result.Content);
            Assert.IsType<TextContentBlock>(result.Content[0]);

            var textContent = (TextContentBlock)result.Content[0];
            Assert.Contains("Hello from task", textContent.Text);
            Assert.Equal("text", textContent.Type);
        }

        [Fact]
        public async Task HandleAsync_WithTaskOfObject_ShouldReturnJsonSerializedResult()
        {
            // Arrange
            var testObject = new { Name = "Test", Value = 42 };
            var taskResult = Task.FromResult(testObject);

            // Act
            var result = await _handler.HandleAsync(taskResult, false);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsError != true);
            Assert.NotNull(result.Content);
            Assert.Single(result.Content);
            Assert.IsType<TextContentBlock>(result.Content[0]);

            var textContent = (TextContentBlock)result.Content[0];
            Assert.Contains("Test", textContent.Text);
            Assert.Contains("42", textContent.Text);
            Assert.Equal("text", textContent.Type);
        }

        [Fact]
        public async Task HandleAsync_WithCompletedTask_ShouldHandleNoResult()
        {
            // Arrange
            var taskResult = Task.CompletedTask;

            // Act
            var result = await _handler.HandleAsync(taskResult, false);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsError != true);
            Assert.NotNull(result.Content);
            Assert.Single(result.Content);
            Assert.IsType<TextContentBlock>(result.Content[0]);

            var textContent = (TextContentBlock)result.Content[0];
            // Should serialize empty object since Task.CompletedTask has no meaningful result
            Assert.Equal("{}", textContent.Text);
        }
    }
}
