
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace API.HATEOASController
{
    [ApiController]
    [Route("[controller]")]
    public class HATEOASController : ControllerBase
    {
        private readonly IUrlHelper _urlHelper; // Injecting IUrlHelper to generate hypermedia links

        public HATEOASController(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        // Static data for demonstration
        private static List<User> _users = new List<User>
        {
            new User { Id = 1, Name = "John" },
            new User { Id = 2, Name = "Alice" }
        };

        [HttpGet]
        public IActionResult GetUsers()
        {
            // Generating hypermedia links for each user
            var usersWithLinks = new List<UserWithLinks>();
            foreach (var user in _users)
            {
                var userLinks = new List<Link>
                {
                    new Link(_urlHelper.Link("GetUser", new { id = user.Id }), "self", "GET"),
                    new Link(_urlHelper.Link("UpdateUser", new { id = user.Id }), "update_user", "PUT"),
                    new Link(_urlHelper.Link("DeleteUser", new { id = user.Id }), "delete_user", "DELETE")
                    // Additional links for other actions...
                };
                usersWithLinks.Add(new UserWithLinks(user, userLinks));
            }

            return Ok(usersWithLinks);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetUser(int id)
        {
            var user = _users.Find(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var userLinks = new List<Link>
            {
                new Link(_urlHelper.Link("GetUser", new { id = user.Id }), "self", "GET"),
                new Link(_urlHelper.Link("UpdateUser", new { id = user.Id }), "update_user", "PUT"),
                new Link(_urlHelper.Link("DeleteUser", new { id = user.Id }), "delete_user", "DELETE")
                // Additional links for other actions...
            };
            var userWithLinks = new UserWithLinks(user, userLinks);

            return Ok(userWithLinks);
        }

        // Other controller actions...

        // Define your User and Link classes here...

        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Link
        {
            public string Href { get; set; }
            public string Rel { get; set; }
            public string Method { get; set; }

            public Link(string href, string rel, string method)
            {
                Href = href;
                Rel = rel;
                Method = method;
            }
        }

        public class UserWithLinks
        {
            public User User { get; set; }
            public List<Link> Links { get; set; }

            public UserWithLinks(User user, List<Link> links)
            {
                User = user;
                Links = links;
            }
        }
    }
}

