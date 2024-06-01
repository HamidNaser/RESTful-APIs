using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    // Other properties...

    public Product(int id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }
}

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private static readonly List<Product> _products = new List<Product>
    {
        new Product(1, "Product A", 10.99m),
        new Product(2, "Product B", 20.49m),
        new Product(3, "Product C", 15.99m),
        new Product(4, "Product D", 5.99m),
        new Product(5, "Product E", 8.99m),
        // Add more sample products...
    };

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetProducts(int page = 1, int pageSize = 5, string filter = null)
    {
        // Apply filtering if specified
        var filteredProducts = string.IsNullOrWhiteSpace(filter) ? _products : _products.Where(p => p.Name.Contains(filter, StringComparison.OrdinalIgnoreCase));

        // Calculate total pages
        var totalPages = (int)Math.Ceiling((double)filteredProducts.Count() / pageSize);

        // Validate page number
        if (page < 1 || page > totalPages)
        {
            return BadRequest("Invalid page number.");
        }

        // Paginate the results
        var pagedProducts = filteredProducts.Skip((page - 1) * pageSize).Take(pageSize);

        // Add pagination metadata to response headers
        Response.Headers.Add("X-Total-Count", filteredProducts.Count().ToString());
        Response.Headers.Add("X-Page", page.ToString());
        Response.Headers.Add("X-Page-Size", pageSize.ToString());
        Response.Headers.Add("X-Total-Pages", totalPages.ToString());

        return Ok(pagedProducts);
    }
}
