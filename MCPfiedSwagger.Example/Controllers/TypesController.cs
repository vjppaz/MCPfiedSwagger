using Microsoft.AspNetCore.Mvc;
using static MCPfiedSwagger.Example.Controllers.UsersController;

namespace MCPfiedSwagger.Example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypesController : ControllerBase
    {
        /// <summary>
        /// Returns all supported primitive types through query parameters.
        /// </summary>
        /// <param name="bigNumber">A 64-bit integer (long).</param>
        /// <param name="pi">A 32-bit floating point number (float).</param>
        /// <param name="e">A 64-bit floating point number (double).</param>
        /// <param name="money">A high-precision decimal value.</param>
        /// <param name="isActive">A boolean value.</param>
        /// <param name="grade">A single character.</param>
        /// <param name="ageByte">A byte-sized age value.</param>
        /// <param name="shortId">A 16-bit integer (short).</param>
        [HttpGet("primitives")]
        public IActionResult GetPrimitives(
    [FromQuery] long bigNumber,
    [FromQuery] float pi,
    [FromQuery] double e,
    [FromQuery] decimal money,
    [FromQuery] bool isActive,
    [FromQuery] char grade,
    [FromQuery] byte ageByte,
    [FromQuery] short shortId)
        {
            return Ok(new
            {
                bigNumber,
                pi,
                e,
                money,
                isActive,
                grade,
                ageByte,
                shortId
            });
        }

        /// <summary>
        /// Returns nullable variants of common data types through query parameters.
        /// </summary>
        /// <param name="score">Nullable integer score.</param>
        /// <param name="isEnabled">Nullable boolean flag.</param>
        /// <param name="since">Nullable DateTime value.</param>
        /// <param name="weight">Nullable double precision number.</param>
        /// <param name="userId">Nullable GUID value.</param>
        /// <param name="status">Nullable enumeration value representing user status.</param>
        [HttpGet("nullable")]
        public IActionResult GetNullableTypes(
    [FromQuery] int? score,
    [FromQuery] bool? isEnabled,
    [FromQuery] DateTime? since,
    [FromQuery] double? weight,
    [FromQuery] Guid? userId,
    [FromQuery] UserStatus? status)
        {
            return Ok(new
            {
                score,
                isEnabled,
                since,
                weight,
                userId,
                status
            });
        }
    }
}
