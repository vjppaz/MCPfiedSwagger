using Microsoft.AspNetCore.Mvc;

namespace MCPfiedSwagger.Example.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// Represents a user with basic properties.
        /// </summary>
        /// <param name="Id">The id of the user.</param>
        /// <param name="Name">The name of the user.</param>
        /// <param name="Age">The age of the user.</param>
        public record UserDto(Guid Id, string Name, int Age);

        /// <summary>
        /// Defines possible statuses for a user.
        /// </summary>
        public enum UserStatus { Active, Inactive }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetAllUsers()
        {
            return Ok(new[]
            {
                new UserDto(Guid.NewGuid(), "Alice", 30),
                new UserDto(Guid.NewGuid(), "Bob", 25)
            });
        }

        /// <summary>
        /// Retrieves a user by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        [HttpGet("{id}")]
        public ActionResult<UserDto> GetUserById(Guid id)
        {
            return Ok(new UserDto(id, "Charlie", 28));
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">The user data to create.</param>
        [HttpPost]
        public ActionResult<UserDto> CreateUser([FromBody] UserDto user)
        {
            return CreatedAtAction(nameof(GetUserById), new { id = Guid.NewGuid() }, user);
        }

        public record UpdateUserDto(string Message);

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="updatedUser">The updated user data.</param>
        [HttpPut("{id}")]
        [Produces<UpdateUserDto>()]
        public IActionResult UpdateUser(Guid id, [FromBody] UserDto updatedUser)
        {
            return Ok(new UpdateUserDto($"User {id} updated."));
        }

        /// <summary>
        /// Updates the status of a user.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <param name="status">The new user status.</param>
        [HttpPatch("{id}/status")]
        public IActionResult UpdateUserStatus(Guid id, [FromQuery] UserStatus status)
        {
            return Ok($"User {id} status set to {status}");
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(Guid id)
        {
            return Ok($"User {id} deleted.");
        }

        /// <summary>
        /// Searches for users by name and optional minimum age.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <param name="minAge">The minimum age filter (optional).</param>
        [HttpGet("search")]
        public ActionResult<IEnumerable<UserDto>> SearchUsers([FromQuery] string name, [FromQuery] int? minAge)
        {
            return Ok(new[]
            {
                new UserDto(Guid.NewGuid(), name ?? "Unknown", minAge ?? 0)
            });
        }

        /// <summary>
        /// Uploads an avatar for a specific user.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <param name="file">The avatar image file.</param>
        [HttpPost("{id}/upload-avatar")]
        public IActionResult UploadAvatar(Guid id, [FromBody] IFormFile file)
        {
            return Ok($"Avatar uploaded for user {id}.");
        }

        /// <summary>
        /// Validates a request using an authentication token from the header.
        /// </summary>
        /// <param name="token">The authentication token (X-Auth-Token).</param>
        [HttpGet("validate-token")]
        public IActionResult ValidateToken([FromHeader(Name = "X-Auth-Token")] string token)
        {
            return string.IsNullOrEmpty(token)
                ? Unauthorized("Missing token")
                : Ok("Token valid");
        }

        /// <summary>
        /// Returns metadata information about the API, including version and supported statuses.
        /// </summary>
        [HttpGet("metadata")]
        [ProducesResponseType(typeof(Dictionary<string, object>), 200)]
        public IActionResult GetMetadata()
        {
            var metadata = new Dictionary<string, object>
            {
                { "apiVersion", "1.0" },
                { "timestamp", DateTime.UtcNow },
                { "supportedStatuses", Enum.GetNames(typeof(UserStatus)) }
            };
            return Ok(metadata);
        }
    }
}