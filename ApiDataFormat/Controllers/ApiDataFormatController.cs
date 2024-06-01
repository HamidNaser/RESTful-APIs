using Microsoft.AspNetCore.Mvc;
namespace API.ApiDataFormat
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = new
            {
                id = id,
                username = "admin",
                role = "admin",
                active = true
            };

            return Ok(user); // Will return JSON by default, XML if requested
        }
    }
}

