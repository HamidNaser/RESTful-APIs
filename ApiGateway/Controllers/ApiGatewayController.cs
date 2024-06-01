// ASP.NET Core example
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Api.ApiGatewayController
{
    [ApiController]
    [Route("[controller]")]
    public class ApiGatewayController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> GetUsers()
        {
            // Logic to fetch users data from the database
            var users = new List<string>(); // Assuming users data is fetched
            return users;
        }
    }
}
