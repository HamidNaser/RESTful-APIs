
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace API.UsePluralsForCollectionsController
{
    [ApiController]
    [Route("[controller]")]
    public class UsePluralsForCollectionsUsersController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> GetUsers()
        {
            // Logic to fetch users data from the database
            var users = new List<string>(); // Assuming users data is fetched
            users.Add("1st_User");
            users.Add("2nd_User");
            return users;
        }
    }

    [ApiController]
    [Route("[controller]")]
    public class UsePluralsForCollectionsUserController : ControllerBase  // Singular form
    {
        [HttpGet("{id}")]
        public string GetUser(int id)
        {
            // Logic to fetch user data from the database
            var user = "User " + id.ToString(); // Assuming user data is fetched
            return user;
        }
    }
}