# RESTful API Overview and Implementation Guide

## Table of Contents
1. [What is a RESTful API?](#what-is-a-restful-api)
2. [Main Principles of REST](#main-principles-of-rest)
3. [Common HTTP Methods](#common-http-methods)
4. [Endpoints](#endpoints)
5. [Design and Best Practices](#design-and-best-practices)
6. [Error Handling and Status Codes](#error-handling-and-status-codes)
7. [Tools and Technologies](#tools-and-technologies)
8. [Advanced Topics](#advanced-topics)
9. [API Gateway Example with Ocelot](#api-gateway-example-with-ocelot)
10. [Authentication and Authorization](#authentication-and-authorization)
11. [Data Formats](#data-formats)
12. [Versioning](#versioning)
13. [Pagination and Filtering](#pagination-and-filtering)
14. [Caching](#caching)
15. [Cross-Origin Resource Sharing (CORS)](#cross-origin-resource-sharing-cors)
16. [Testing](#testing)
17. [Performance Optimization](#performance-optimization)
18. [Documentation](#documentation)
19. [Microservices Architecture](#microservices-architecture)
20. [Real-world Scenarios](#real-world-scenarios)
21. [Industry Trends](#industry-trends)

---

# What is a RESTful API?
A RESTful API adheres to the principles of REST (Representational State Transfer), using standard HTTP methods and URL structures to provide access to resources.

# Main Principles of REST
- **Statelessness**: Each request from a client to a server must contain all the information needed to understand and process the request.
- **Client-Server Architecture**: Separation of client and server concerns.
- **Uniform Interface**: Standard methods (GET, POST, PUT, DELETE) and URIs for resource access.
- **Resource-Based**: Everything is a resource, identified by URIs.
- **Cacheable**: Responses must define themselves as cacheable or not to prevent clients from reusing stale data.
- **Layered System**: Architecture composed of hierarchical layers to enable scalability and manageability.



### 1. Statelessness:
- **Definition**: Statelessness in REST means that the server does not store any client context between requests. Each request from a client to a server must contain all the information necessary to understand and process the request, including authentication credentials, session state, and any other relevant data.
- **Importance**: Statelessness simplifies the server logic and enhances scalability by allowing servers to handle requests independently, without needing to maintain session state for each client. It also improves fault tolerance because there is no need to recover session data in case of server failures.
- **Example**: In a stateless RESTful API, authentication credentials are typically sent with each request (e.g., using tokens or API keys), and the server does not store any information about the client's session.

### 2. Client-Server Architecture:
- **Definition**: The client-server architecture in REST involves separating the concerns of the client and the server. The client is responsible for the presentation layer and user interface, while the server is responsible for storing and managing data, application logic, and security.
- **Importance**: This separation of concerns promotes modularity, making it easier to evolve and scale both the client and server components independently. It also allows clients and servers to be developed and maintained by different teams or organizations, promoting flexibility and interoperability.
- **Example**: In a web application, the client (browser) handles the presentation logic, such as rendering HTML and processing user input, while the server (backend) manages data storage, business logic, and authentication.

### 3. Uniform Interface:
- **Definition**: The uniform interface principle defines a standardized way for clients and servers to interact. It includes using standard HTTP methods (GET, POST, PUT, DELETE) for resource manipulation and identifying resources with URIs.
- **Importance**: A uniform interface simplifies the architecture and makes it easier to understand and use the API. It allows clients and servers to communicate effectively without requiring prior knowledge of each other's implementation details, promoting loose coupling and interoperability.
- **Example**: By adhering to the uniform interface, clients can perform CRUD (Create, Read, Update, Delete) operations on resources using standard HTTP methods, such as GET for retrieving data, POST for creating data, PUT for updating data, and DELETE for deleting data.

### 4. Resource-Based:
- **Definition**: In REST, everything is considered a resource, which can be any entity or concept that can be uniquely identified, such as objects, data, or services. Resources are identified by URIs (Uniform Resource Identifiers).
- **Importance**: Treating everything as a resource provides a consistent and intuitive way to model the system. It allows clients to interact with resources using standard HTTP methods and URIs, simplifying the API design and making it more predictable and understandable.
- **Example**: In a social media platform, users, posts, comments, and likes can all be considered resources, each identified by a unique URI (e.g., `/users`, `/posts`, `/comments`).

### 5. Cacheable:
- **Definition**: The cacheable principle states that responses from the server should indicate whether they can be cached by clients or intermediary caches. Caching allows clients to reuse previously fetched representations of resources, reducing latency and improving performance.
- **Importance**: Caching can significantly reduce server load and network traffic by serving cached responses instead of generating them from scratch for each request. It also improves the user experience by reducing latency and making the application more responsive.
- **Example**: In an e-commerce application, product information that rarely changes, such as product descriptions and images, can be marked as cacheable, allowing clients to store and reuse them for subsequent requests.

### 6. Layered System:
- **Definition**: The layered system principle involves organizing the architecture into multiple hierarchical layers, with each layer performing a specific function and hiding the complexity of the layers below it. This enables scalability, fault tolerance, and modifiability.
- **Importance**: Layering promotes separation of concerns and encapsulation, making the system easier to understand, maintain, and scale. It also allows for the introduction of new features or technologies in one layer without affecting other layers, enhancing flexibility and agility.
- **Example**: In a web application, the architecture may consist of multiple layers, such as presentation layer (UI), application layer (business logic), data access layer (database), and infrastructure layer (networking). Each layer communicates with adjacent layers through well-defined interfaces, making it easy to replace or upgrade individual components.

By adhering to these main principles of REST, developers can design robust, scalable, and maintainable APIs that promote interoperability, flexibility, and performance.

<h2 id="common-http-methods" style="color: blue;">Common HTTP Methods</h2>
- **GET**: Retrieve a resource or a list of resources.
- **POST**: Create a new resource.
- **PUT**: Update an existing resource.
- **DELETE**: Delete a resource.
- **PATCH**: Apply partial modifications to a resource.

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

public class CsvRecord
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Value { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class CashingController : ControllerBase
{
    private readonly string _csvFilePath = "data\\data.csv"; // Path to your CSV file

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CsvRecord>>> GetAllRecords()
    {
        var records = await ReadCsvFileAsync();
        return Ok(records);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CsvRecord>> GetRecordById(int id)
    {
        var records = await ReadCsvFileAsync();
        var record = records.FirstOrDefault(r => r.Id == id);
        if (record == null)
        {
            return NotFound();
        }

        return Ok(record);
    }

    [HttpPost]
    public async Task<ActionResult<CsvRecord>> AddRecord([FromBody] CsvRecord newRecord)
    {
        if (newRecord == null)
        {
            return BadRequest();
        }

        var records = await ReadCsvFileAsync();
        newRecord.Id = records.Max(r => r.Id) + 1;
        records.Add(newRecord);
        await WriteCsvFileAsync(records);

        return CreatedAtAction(nameof(GetRecordById), new { id = newRecord.Id }, newRecord);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRecord(int id, [FromBody] CsvRecord updatedRecord)
    {
        if (updatedRecord == null || updatedRecord.Id != id)
        {
            return BadRequest();
        }

        var records = await ReadCsvFileAsync();
        var existingRecord = records.FirstOrDefault(r => r.Id == id);
        if (existingRecord == null)
        {
            return NotFound();
        }

        existingRecord.Name = updatedRecord.Name;
        existingRecord.Value = updatedRecord.Value;
        await WriteCsvFileAsync(records);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecord(int id)
    {
        var records = await ReadCsvFileAsync();
        var record = records.FirstOrDefault(r => r.Id == id);
        if (record == null)
        {
            return NotFound();
        }

        records.Remove(record);
        await WriteCsvFileAsync(records);

        return NoContent();
    }

    [HttpPut("batch")]
    public async Task<IActionResult> UpdateBatchRecords([FromBody] IEnumerable<CsvRecord> updatedRecords)
    {
        if (updatedRecords == null || !updatedRecords.Any())
        {
            return BadRequest();
        }

        var records = await ReadCsvFileAsync();

        foreach (var updatedRecord in updatedRecords)
        {
            var existingRecord = records.FirstOrDefault(r => r.Id == updatedRecord.Id);
            if (existingRecord != null)
            {
                existingRecord.Name = updatedRecord.Name;
                existingRecord.Value = updatedRecord.Value;
            }
        }

        await WriteCsvFileAsync(records);

        return NoContent();
    }

    private async Task<List<CsvRecord>> ReadCsvFileAsync()
    {
        var records = new List<CsvRecord>();
        using (var reader = new StreamReader(_csvFilePath))
        {
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                var values = line.Split(',');
                if (values[0] == "Id") continue; // Skip the header row

                var record = new CsvRecord
                {
                    Id = int.Parse(values[0]),
                    Name = values[1],
                    Value = int.Parse(values[2])
                };

                records.Add(record);
            }
        }
        return records;
    }

    private async Task WriteCsvFileAsync(IEnumerable<CsvRecord> records)
    {
        using (var writer = new StreamWriter(_csvFilePath))
        {
            // Write the header
            await writer.WriteLineAsync("Id,Name,Value");

            // Write each record
            foreach (var record in records)
            {
                var line = $"{record.Id},{record.Name},{record.Value}";
                await writer.WriteLineAsync(line);
            }
        }
    }
}
```

# Endpoints
Endpoints are specific URLs that clients interact with to access or manipulate resources. For example, `/api/users` might be an endpoint for user resources.

 

# Design and Best Practices : 
## 1. Use nouns for resources (`/users`, `/products`).

```csharp
// ASP.NET Core example
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
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
    public class ProductsController : ControllerBase
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
```

# Design and Best Practices : 
## 2. Use plurals for collections (`/users` vs `/user`).

```csharp
// ASP.NET Core example
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
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
    public class UserController : ControllerBase  // Singular form
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
```

# Design and Best Practices : 
## 3. Use hierarchical structure for nested resources (`/users/1/orders`).

```csharp
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        // GET /users
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            // Logic to fetch users data from the database
            var users = new List<User>(); // Assuming users data is fetched
            return users;
        }

        // GET /users/1
        [HttpGet("{userId}")]
        public User GetUser(int userId)
        {
            // Logic to fetch user data from the database based on userId
            var user = new User(); // Assuming user data is fetched
            return user;
        }

        // GET /users/1/orders
        [HttpGet("{userId}/orders")]
        public IEnumerable<Order> GetUserOrders(int userId)
        {
            // Logic to fetch orders data for the specified user from the database
            var orders = new List<Order>(); // Assuming orders data is fetched
            return orders;
        }
    }

    public class User
    {
        // Properties of the User class
        public int Id { get; set; }
        public string Name { get; set; }
        // Other properties...
    }

    public class Order
    {
        // Properties of the Order class
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        // Other properties...
    }
}
```
In this example, 
1.  The UsersController handles hierarchical structure with nested resources. 
2.  The endpoint /users returns a collection of users, /users/{userId} returns an individual user.
3.  The endpoint /users/{userId}/orders returns orders associated with a specific user. 
4.  This follows the hierarchical structure where orders is nested under users.


# Design and Best Practices : 
## 4. Implement HATEOAS

Implementing HATEOAS (Hypermedia As The Engine Of Application State) involves including hypermedia links in API responses to guide clients on how to navigate the API and discover available actions or resources dynamically. 

```csharp
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUrlHelper _urlHelper; // Injecting IUrlHelper to generate hypermedia links

        public UsersController(IUrlHelper urlHelper)
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
            };
            var userWithLinks = new UserWithLinks(user, userLinks);

            return Ok(userWithLinks);
        }


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
```

In this example:

1.  The `GetUsers` and `GetUser` actions return user data along with hypermedia links. These links provide information on how clients can interact with the API, such as retrieving, updating,    
    or deleting users.

2.  The `IUrlHelper` service is injected into the controller to generate URLs for hypermedia links.

3.  The `UserWithLinks` class is used to encapsulate user data along with hypermedia links.

4.  The `Link` class represents a hypermedia link, containing the URL, the relation type (e.g., "self", "update_user"), and the HTTP method.

5.  This example demonstrates how HATEOAS can be implemented to provide clients with dynamic navigation and discoverability within the API.

# Versioning

### Common API Versioning Approaches

#### 1. URI Path Versioning
**Example:** `https://api.example.com/v1/users`

**Pros:**
- Easy to understand and implement
- Clearly indicates version

**Cons:**
- Clutters URLs with many versions
- Requires client updates for version changes

#### 2. Query Parameter Versioning
**Example:** `https://api.example.com/users?version=1`

**Pros:**
- Simple to add/modify in URLs
- Doesn’t alter URL structure

**Cons:**
- Less visible/intuitive
- Can complicate caching

#### 3. Header Versioning
**Example:** `GET /users` with header `Accept: application/vnd.example.v1+json`

**Pros:**
- Clean URL structure
- Flexible for multiple versions

**Cons:**
- Less visible/discoverable
- More complex for clients to set headers

#### 4. Content Negotiation (Media Type Versioning)
**Example:** `Accept: application/vnd.example.v1+json`

**Pros:**
- Clean URLs
- Uses HTTP’s content negotiation

**Cons:**
- Complex media type management
- Less intuitive for some developers

#### 5. Semantic Versioning
**Example:** `https://api.example.com/v1.2.3/users`

**Pros:**
- Clear change impact
- Manages and communicates changes effectively

**Cons:**
- Many active versions
- Clients handle version ranges

### Choosing the Right Approach

- **Clarity:** URI Path Versioning and Semantic Versioning are straightforward.
- **Simplicity:** Query Parameter Versioning and URI Path Versioning are easy to implement.
- **Flexibility:** Header and Content Negotiation offer flexibility for multiple versions.
- **Visibility:** URI Path Versioning is highly visible.

**Best Practice:** Combine approaches, e.g., URI Path for major versions and Header for minor changes, to balance clarity, flexibility, and simplicity.

Below code demonstrating how to handle versioning gracefully using three common approaches: URI versioning, header versioning, and query parameter versioning. We'll implement these approaches in an ASP.NET Core API controller:

```csharp
using Microsoft.AspNetCore.Mvc;
using System;

namespace MyAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")] // Default API version
    [ApiVersion("2.0")]
    public class ProductsController : ControllerBase
    {
        // URI versioning
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetProductV1(int id)
        {
            return Ok(new { Id = id, Name = "Product V1" });
        }

        [HttpGet("{id}")]
        [MapToApiVersion("2.0")]
        public IActionResult GetProductV2(int id)
        {
            return Ok(new { Id = id, Name = "Product V2" });
        }

        // Header versioning
        [HttpGet("{id}")]
        public IActionResult GetProductHeaderVersion(int id)
        {
            var apiVersion = HttpContext.GetRequestedApiVersion().ToString();
            if (apiVersion == "1.0")
            {
                return Ok(new { Id = id, Name = "Product V1" });
            }
            else if (apiVersion == "2.0")
            {
                return Ok(new { Id = id, Name = "Product V2" });
            }
            else
            {
                return BadRequest("Invalid API version");
            }
        }

        // Query parameter versioning
        [HttpGet("{id}")]
        public IActionResult GetProductQueryVersion(int id, ApiVersion version)
        {
            if (version.MajorVersion == 1)
            {
                return Ok(new { Id = id, Name = "Product V1" });
            }
            else if (version.MajorVersion == 2)
            {
                return Ok(new { Id = id, Name = "Product V2" });
            }
            else
            {
                return BadRequest("Invalid API version");
            }
        }
    }
}
```

In this example:

1.  URI versioning: Different versions of the `GetProduct` endpoint are mapped to different HTTP methods based on the requested API version.

2.  Header versioning: The API version is extracted from the request headers, and the appropriate logic is executed based on the version.

3.  Query parameter versioning: The API version is extracted from the query parameters, and the appropriate logic is executed based on the version.

### Backward Compatibility
Ensure backward compatibility and provide clear migration paths for clients using older versions.

```csharp
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace MyAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class ProductsController : ControllerBase
    {
        // Product data for demonstration
        private static Dictionary<int, string> _productsV1 = new Dictionary<int, string>
        {
            { 1, "Product A" },
            { 2, "Product B" },
            { 3, "Product C" }
        };

        private static Dictionary<int, string> _productsV2 = new Dictionary<int, string>
        {
            { 1, "Product X" },
            { 2, "Product Y" },
            { 3, "Product Z" }
        };

        // Endpoint for getting product details
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id, ApiVersion version)
        {
            // Check if the requested version is supported
            if (version.MajorVersion == 1)
            {
                // Check if the product exists in version 1 data
                if (_productsV1.ContainsKey(id))
                {
                    return Ok(new { Id = id, Name = _productsV1[id] });
                }
                else
                {
                    return NotFound();
                }
            }
            else if (version.MajorVersion == 2)
            {
                // Check if the product exists in version 2 data
                if (_productsV2.ContainsKey(id))
                {
                    return Ok(new { Id = id, Name = _productsV2[id] });
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                // Handle unsupported API version
                return BadRequest("Unsupported API version");
            }
        }

        // Endpoint for providing migration path guidance
        [HttpGet("migration")]
        public IActionResult MigrationGuide(ApiVersion version)
        {
            if (version.MajorVersion == 1)
            {
                // Provide guidance for migrating to version 2
                return Ok("Please migrate to API version 2 for improved features and performance.");
            }
            else if (version.MajorVersion == 2)
            {
                // Inform clients that they are using the latest version
                return Ok("You are currently using the latest version of the API (version 2).");
            }
            else
            {
                // Handle unsupported API version
                return BadRequest("Unsupported API version");
            }
        }
    }
}
```

In this example:

1.  The `GetProduct` endpoint retrieves product details based on the requested API version. It checks if the requested version is supported and then serves the appropriate data accordingly.

2.  The `MigrationGuide` endpoint provides guidance for clients using older versions, directing them to migrate to the latest version for improved features and performance.

By providing clear migration paths and handling backward compatibility gracefully, you ensure that clients using older versions of your API can smoothly transition to newer versions without encountering disruptions or errors.

# Error Handling and Status Codes
- **200 OK**: The request was successful.
- **201 Created**: A resource was successfully created.
- **204 No Content**: The request was successful, but there is no content to return.
- **400 Bad Request**: The request was invalid or cannot be otherwise served.
- **401 Unauthorized**: Authentication is required and has failed or not been provided.
- **403 Forbidden**: The server understood the request but refuses to authorize it.
- **404 Not Found**: The requested resource could not be found.
- **500 Internal Server Error**: An error occurred on the server.

# Advanced Topics
- **Asynchronous Processing**: Use background processing for long-running tasks, implement asynchronous endpoints using async/await in C#, and use message queues (e.g., RabbitMQ, AWS SQS) for task delegation.
- **Filtering and Sorting**: Use query parameters to specify filtering and sorting criteria.
- **Pagination**: Use query parameters such as `limit` and `offset` or `page` and `size`.
- **Idempotency**: Ensure that making the same request multiple times has the same effect as making it once.
- **Scalability**: Use statelessness, caching, load balancing, and microservices to scale APIs.
- **Security**: Use HTTPS, proper authentication and authorization, validate all input data, use security headers, and implement rate limiting.

# API Gateway Example with Ocelot

Using an API gateway helps manage, secure, and monitor traffic. Here’s an example using **Ocelot** in an ASP.NET Core environment.

### Step-by-Step Guide

1. **Create an ASP.NET Core Web Application**:
   ```bash
   dotnet new webapi -n ApiGateway
   cd ApiGateway
   dotnet add package Ocelot
   dotnet add package Microsoft.Extensions.Configuration.Json
   ```

2. **Configure Ocelot**:
   - Create `ocelot.json` in the root directory:
     ```json
     {
       "Routes": [
         {
           "DownstreamPathTemplate": "/api/users",
           "DownstreamScheme": "http",
           "DownstreamHostAndPorts": [
             {
               "Host": "localhost",
               "Port": 5001
             }
           ],
           "UpstreamPathTemplate": "/gateway/users",
           "UpstreamHttpMethod": [ "GET", "POST", "PUT", "DELETE" ]
         },
         {
           "DownstreamPathTemplate": "/api/products",
           "DownstreamScheme": "http",
           "DownstreamHostAndPorts": [
             {
               "Host": "localhost",
               "Port": 5002
             }
           ],
           "UpstreamPathTemplate": "/gateway/products",
           "UpstreamHttpMethod": [ "GET", "POST", "PUT", "DELETE" ]
         }
       ],
       "GlobalConfiguration": {
         "BaseUrl": "http://localhost:5000"
       }
     }
     ```

3. **Modify `Startup.cs`**:
   ```csharp
   public class Startup
   {
       public IConfiguration Configuration { get; }

       public Startup(IConfiguration configuration)
       {
           Configuration = configuration;
       }

       public void ConfigureServices(IServiceCollection services)
       {
           services.AddOcelot(Configuration);
       }

       public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
       {
           if (env.IsDevelopment())
           {
               app.UseDeveloperExceptionPage();
           }

           app.UseRouting();

           app.UseEndpoints(endpoints =>
           {
               endpoints.MapControllers();
           });

           app.UseOcelot().Wait();
       }
   }
   ```

4. **Modify `Program.cs`**:
   ```csharp
   public class Program
   {
       public static void Main(string[] args)
       {
           CreateHostBuilder(args).Build().Run();
       }

       public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   config.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
               })
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               });
   }
   ```

**Create Backend Services**:
   - Create two ASP.NET Core Web API projects running on different ports (e.g., `5001` and `5002`).

**Run the Gateway and Backend Services**:
   - Start the API Gateway project and the backend services.

**Test the API Gateway**:
   - Use Postman, curl, or any HTTP client to test the routes exposed by the API gateway.


# Authentication and Authorization

Securing RESTful APIs is essential to ensure that only authorized users can access your services and resources. This section covers how to implement authentication and authorization in your API, with code examples from our implementation.

### How to Secure RESTful APIs?

Securing RESTful APIs typically involves:
1. **Authentication**: Verifying the identity of a user or system.
2. **Authorization**: Determining if the authenticated user or system has permission to access a specific resource.

### Common Authentication Methods

1. **Basic Authentication**: Uses a username and password encoded in Base64.
2. **Token-Based Authentication**: Uses tokens like JSON Web Tokens (JWT) for authentication.
3. **OAuth**: A standard for token-based authentication and authorization.

### Handling Authorization for Different Types of Users or Roles

Authorization can be handled by assigning roles to users and defining which roles can access which resources. In this implementation, we use JWT for both authentication and authorization.

### Implementation Details

#### 1. JWT Authentication Controller

**AuthenticationController.cs**

This controller handles user login and token generation.

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authenication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public AuthController(IConfiguration configuration)
        {
            _secretKey = configuration["AppSettings:JwtSecretKey"];
            _issuer = configuration["AppSettings:Issuer"];
            _audience = configuration["AppSettings:Audience"];
        }

        private readonly List<User> _users = new List<User>
        {
            new User { Id = 1, Username = "admin", Password = "admin123", Role = "admin" },
            new User { Id = 2, Username = "user", Password = "user123", Role = "user" }
        };

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin model)
        {
            var user = AuthenticateUser(model.Username, model.Password);
            if (user == null)
                return Unauthorized();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = JwtHelper.GetJwtToken(
                user.Username,
                _secretKey,
                _issuer,
                _audience,
                TimeSpan.FromMinutes(60),
                claims.ToArray());

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expires = token.ValidTo
            });
        }

        private User AuthenticateUser(string username, string password)
        {
            return _users.Find(u => u.Username == username && u.Password == password);
        }
    }
}
```

#### 2. JWT Helper

**JwtHelper.cs**

This class generates JWT tokens.

```csharp
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication
{
    public class JwtHelper
    {
        public static JwtSecurityToken GetJwtToken(
            string username,
            string secretKey,
            string issuer,
            string audience,
            TimeSpan expiration,
            Claim[] additionalClaims = null)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (additionalClaims != null)
            {
                var claimList = new List<Claim>(claims);
                claimList.AddRange(additionalClaims);
                claims = claimList.ToArray();
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: DateTime.UtcNow.Add(expiration),
                claims: claims,
                signingCredentials: creds
            );
        }
    }
}
```

#### 3. Protected Controller

**ProtectedController.cs**

This controller demonstrates how to secure endpoints with role-based authorization.

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Security.Claims;

namespace Authenication
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProtectedController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProtectedController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult GetProtectedData()
        {
            var userClaims = HttpContext.User.Claims;
            var username = userClaims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var role = userClaims?.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (role != "admin")
            {
                return Forbid();
            }

            return Ok($"Hello, {username}! This is protected data for users with 'admin' role.");
        }
    }
}
```

#### 4. Program Configuration

**Program.cs**

Configures JWT authentication in the ASP.NET Core application.

```csharp
using Authenication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var secretKey = configuration["AppSettings:JwtSecretKey"];
var issuer = configuration["AppSettings:Issuer"];
var audience = configuration["AppSettings:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

### Using the API

Here’s how you can do it with different tools:

### Using Postman

#### Step 1: Login to Get Token

1. **Open Postman** and create a new request.
2. **Set the request type** to `POST`.
3. **Enter the request URL**: `https://localhost:7051/api/auth/login`.
4. **Go to the Headers tab** and ensure `Content-Type` is set to `application/json`.
5. **Go to the Body tab**, select `raw`, and enter the following JSON:

    ```json
    {
        "username": "admin",
        "password": "admin123"
    }
    ```

6. **Click Send** to send the request.

7. **Copy the token** from the response. It will look something like this:

    ```json
    {
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6ImFkbWluIiwibmJmIjoxNzE3MTAyODg5LCJleHAiOjE3MTcxMDY0ODksImlhdCI6MTcxNzEwMjg4OX0.4kjBIAZPbhQKiqKkjHnS58rVmYdftsfCxsqHz2DyGKE"
    }
    ```

#### Step 2: Access Protected Data

1. **Create a new request** in Postman.
2. **Set the request type** to `GET`.
3. **Enter the request URL**: `https://localhost:7051/api/protected`.
4. **Go to the Headers tab** and add a new header:
    - **Key**: `Authorization`
    - **Value**: `Bearer {eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6ImFkbWluIiwibmJmIjoxNzE3MTAyODg5LCJleHAiOjE3MTcxMDY0ODksImlhdCI6MTcxNzEwMjg4OX0.4kjBIAZPbhQKiqKkjHnS58rVmYdftsfCxsqHz2DyGKE}` 

5. **Click Send** to send the request.

6. **Check the response**. If the token is valid and the user has the correct role, you should see a response similar to:

    ```json
    {
        "message": "Hello, admin! This is protected data for users with 'admin' role."
    }
    ```

By following these steps in Postman, you can authenticate and access protected endpoints in your RESTful API.

### Using C#

```csharp
using System.Text.Json;
using Newtonsoft.Json.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Step 1: Authenticate and obtain JWT token
            var token = await AuthenticateAndGetToken();

            // Step 2: Call the protected endpoint with the obtained token
            await CallProtectedEndpoint(token);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    static async Task<string> AuthenticateAndGetToken()
    {
        using (var httpClient = new HttpClient())
        {
            var model = new { Username = "admin", Password = "admin123" };

            var modelJson = JsonSerializer.Serialize(model);
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7051/api/Auth/login");

            var content = new StringContent(modelJson, null, "application/json");

            request.Content = content;

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadAsStringAsync();

            return token;
        }
    }

    static async Task CallProtectedEndpoint(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return;
        }

        var jsonObject = JObject.Parse(token);
        if (jsonObject == null)
        {
            return;
        }

        string tokenValue = jsonObject["token"].ToString();

        using (var httpClient = new HttpClient())
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7051/api/protected");

                request.Headers.Add("Authorization", $"Bearer {tokenValue}");

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Protected Endpoint Response: " + content);
                }
                else
                {
                    Console.WriteLine("Failed to access protected endpoint. Status code: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}
```

This section provides a clear and structured overview of implementing authentication and authorization in your API, ensuring secure access to resources based on user roles.


# Data Formats

In RESTful APIs, data is typically exchanged between clients and servers in structured formats. The two most commonly used data formats are JSON (JavaScript Object Notation) and XML (eXtensible Markup Language).

### Commonly Used Data Formats in RESTful APIs

1. **JSON (JavaScript Object Notation)**
2. **XML (eXtensible Markup Language)**

### JSON vs. XML

| Feature                         | JSON                                                      | XML                                                      |
|---------------------------------|-----------------------------------------------------------|----------------------------------------------------------|
| **Syntax**                      | Lightweight and easy to read/write                        | More verbose and complex                                  |
| **Data Interchange**            | Native support in JavaScript and most modern languages    | Supported in all programming languages                    |
| **Parsing Speed**               | Faster parsing due to lightweight syntax                  | Slower parsing due to more complex structure              |
| **Data Types**                  | Supports basic data types: string, number, object, array  | Supports mixed content, attributes, and complex structures|
| **Readability**                 | More human-readable due to concise syntax                 | Less readable due to verbose tags                         |
| **Schema Validation**           | Less rigid, schemas like JSON Schema can be used          | Built-in schema validation with DTD or XSD                |
| **Usage in Web APIs**           | Widely used in modern web APIs                            | Used in older or enterprise systems                       |
| **Tooling and Libraries**       | Extensive tooling and libraries available                 | Extensive tooling and libraries available                 |

### When to Choose JSON

- **Modern Web Development**: JSON is the default format for modern web development due to its lightweight nature and native support in JavaScript.
- **Performance**: JSON is faster to parse, making it suitable for high-performance applications.
- **Readability**: JSON is more readable and easier to work with for developers.
- **Interoperability**: JSON is widely supported across various platforms and languages.

### When to Choose XML

- **Complex Data Structures**: XML is better suited for representing complex data structures, including mixed content and attributes.
- **Legacy Systems**: XML might be preferred when interacting with legacy systems or enterprise environments that already use XML.
- **Schema Validation**: XML's built-in schema validation (using DTD or XSD) ensures data integrity and structure.
- **Extensibility**: XML's extensible nature makes it suitable for applications where data format might evolve over time.

### Example: JSON and XML Representations

Here are examples of the same data represented in both JSON and XML formats.

#### JSON Example

```json
{
    "user": {
        "id": 1,
        "username": "admin",
        "role": "admin",
        "active": true
    }
}
```

#### XML Example

```xml
<user>
    <id>1</id>
    <username>admin</username>
    <role>admin</role>
    <active>true</active>
</user>
```

### Example Code for Returning Data in JSON and XML

Here's how you can configure an ASP.NET Core API to return data in both JSON and XML formats.

**Program.cs**

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using System.Reflection;
using RESTful_APIs;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddControllers().AddXmlSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services
    .AddHttpContextAccessor()
    .AddRouting()
    .AddConnectServicesAndRepositories();

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyHeader()
           .AllowAnyMethod()
           .SetIsOriginAllowed((host) => true)
           .AllowCredentials(); ;
}));



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OpenAI V1");
    });
}
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.UseCors("CorsPolicy");


app.UseAuthorization();

app.MapControllers();

app.Run();
```

**Controller Example**

```csharp
using Microsoft.AspNetCore.Mvc;

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
```

In the above example, the `AddXmlSerializerFormatters` method is used to enable XML formatting in addition to the default JSON formatting. The controller returns a user object, which can be returned in either JSON or XML format based on the client's `Accept` header.

### Using the API

To get either JSON or XML responses from your API, you need to set the `Accept` header in your HTTP request to indicate which format you prefer. Here’s how you can do it with different tools:

### Using Postman

1. Open Postman and enter the URL of your API endpoint (e.g., `https://localhost:7051/api/users/1`).
2. Select the HTTP method (e.g., GET).
3. Go to the **Headers** tab.
4. Add a header with the key `Accept` and set the value to `application/json` or `application/xml` depending on the desired format.
5. Click **Send**.

### Using C#

#### To get JSON or XML:
```csharp
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Step 1: Json Format
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync("https://localhost:7051/api/users/1");
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }

            // Step 2: Xml Format
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
                var response = await client.GetAsync("https://localhost:7051/api/users/1");
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}
```

By setting the `Accept` header in your HTTP request to either `application/json` or `application/xml`, you can specify the desired response format from the API. This allows you to easily switch between receiving data in JSON or XML format based on your application's requirements.


# Pagination and Filtering
How do you implement pagination and filtering in RESTful APIs? Why is it important, and what are some best practices?

# Caching
What is caching, and how can it be implemented in RESTful APIs? Discuss the benefits and challenges of caching in the context of API development.

# Cross-Origin Resource Sharing (CORS)
What is CORS, and how do you handle it in RESTful APIs? What security implications should you consider when dealing with CORS?

# Testing
How do you test RESTful APIs? Discuss different types of testing, such as unit testing, integration testing, and end-to-end testing. What tools and frameworks can be used for API testing?

# Performance Optimization
What strategies can be employed to optimize the performance of RESTful APIs? How do you identify performance bottlenecks, and what techniques can be used to address them?

# Documentation
Why is documentation important for RESTful APIs? What are some popular documentation formats and tools used for documenting APIs?

# Microservices Architecture
How does RESTful API development fit into a microservices architecture? What are the advantages and challenges of using RESTful APIs in a microservices-based system?

# Real-world Scenarios
Be prepared to discuss real-world scenarios or challenges you have encountered while developing or consuming RESTful APIs, and how you addressed them.

# Industry Trends
Stay updated on current trends and advancements in RESTful API development, such as GraphQL, serverless architecture, and API-first design principles.

# Tools and Technologies
- **Postman**: A popular tool for testing and documenting APIs.
- **Swagger/OpenAPI**: For documenting and testing APIs.
