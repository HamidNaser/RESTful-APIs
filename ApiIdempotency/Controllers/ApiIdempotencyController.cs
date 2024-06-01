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
    private readonly Dictionary<int, Product> _products = new Dictionary<int, Product>();

    public ProductsController()
    {
        // Initialize _products with sample data
        _products = new Dictionary<int, Product>
        {
            { 1, new Product(1, "Product A", 10.99m) },
            { 2, new Product(2, "Product B", 20.49m) },
            { 3, new Product(3, "Product C", 15.99m) },
        };
    }
    [HttpGet("{id}")]
    public ActionResult<Product> GetProduct(int id)
    {
        if (_products.ContainsKey(id))
        {
            return Ok(_products[id]);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost]
    public ActionResult<Product> CreateOrUpdateProduct(Product product)
    {
        if (_products.ContainsKey(product.Id))
        {
            _products[product.Id] = product;
            return Ok(product);
        }
        else
        {
            _products.Add(product.Id, product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteProduct(int id)
    {
        if (_products.ContainsKey(id))
        {
            _products.Remove(id);
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }
}
