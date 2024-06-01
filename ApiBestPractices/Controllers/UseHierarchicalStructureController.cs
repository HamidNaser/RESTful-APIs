using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Api.UseHierarchicalStructureController
{
    [ApiController]
    [Route("[controller]")]
    public class UseHierarchicalStructureUsersController : ControllerBase
    {
        // GET /users
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            var users = new List<User>
            {
                new User(1, "John"),
                new User(2, "Alice"),
                new User(3, "Bob"),
                new User(4, "Emily"),
                new User(5, "Michael")
            };
            return users;
        }

        // GET /users/1
        [HttpGet("{userId}")]
        public User GetUser(int userId)
        {
            var user = new User(1, "John");

            return user;
        }

        // GET /users/1/orders
        [HttpGet("{userId}/orders")]
        public IEnumerable<Order> GetUserOrders(int userId)
        {
            var orders = new List<Order>
            {
                new Order(1, "ORD123"),
                new Order(2, "ORD456"),
                new Order(3, "ORD789"),
                new Order(4, "ORD012"),
                new Order(5, "ORD345")
            };

            return orders;
        }
    }

    public class User
    {
        public User(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }

        public Order(int id, string orderNumber)
        {
            Id = id;
            OrderNumber = orderNumber;
        }
    }
}