using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Versioning;
using System;

namespace MyAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiVersion("3.0")]
    public class ProductsController : ControllerBase
    {
        // URI versioning
        [HttpGet("GetProductV1/{id}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetProductV1(int id)
        {
            // Logic to get product for version 1
            return Ok(new { Id = id, Name = "Product V1" });
        }

        [HttpGet("GetProductV2/{id}")]
        [MapToApiVersion("2.0")]
        public IActionResult GetProductV2(int id)
        {
            // Logic to get product for version 2
            return Ok(new { Id = id, Name = "Product V2" });
        }

        [HttpGet("GetProductV3/{id}")]
        [MapToApiVersion("3.0")]
        public IActionResult GetProductVx(int id)
        {
            // Logic to get product for version 2
            return Ok(new { Id = id, Name = "Product V2" });
        }

        // Header versioning
        [HttpGet("{id}")]
        public IActionResult GetProductHeaderVersion(int id)
        {
            var apiVersion = HttpContext.GetRequestedApiVersion().ToString();
            if (apiVersion == "1.0")
            {
                // Logic to get product for version 1
                return Ok(new { Id = id, Name = "Product V1" });
            }
            else if (apiVersion == "2.0")
            {
                // Logic to get product for version 2
                return Ok(new { Id = id, Name = "Product V2" });
            }
            else
            {
                return BadRequest("Invalid API version");
            }
        }
    }
}
