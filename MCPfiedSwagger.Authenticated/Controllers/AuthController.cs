using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MCPfiedSwagger.Authenticated.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            // Validate user credentials (this is just a placeholder)
            if (model.Username == "test" && model.Password == "password")
            {
                var claims = new[]
                {
                new Claim(ClaimTypes.Name, model.Username)
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mcpfiedswaggerverylongsecurekey123456789"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "my mcpfiedswagger example",
                    audience: "mcpfied devs",
                    claims: claims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return Unauthorized();
        }
    }
}
