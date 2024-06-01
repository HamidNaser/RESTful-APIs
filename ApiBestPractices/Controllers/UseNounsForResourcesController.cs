// ASP.NET Core example
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace UseNounsForResourcesController
{
    [ApiController]
    [Route("[controller]")]
    public class UsersNounsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> GetUsers()
        {
            // Logic to fetch users data from the database
            var users = new List<string>(); // Assuming users data is fetched
            return users;
        }
    }

    [ApiController]
    [Route("[controller]")]
    public class ProductsNounsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> GetProducts()
        {
            // Logic to fetch products data from the database
            var products = new List<string>(); // Assuming products data is fetched
            return products;
        }
    }
}
