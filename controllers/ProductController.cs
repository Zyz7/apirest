using ApiRest.dtos;
using ApiRest.models;
using ApiRest.repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiRest.controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        public readonly Context _context;

        public ProductController(Context context)
        {
            _context = context;
        } 

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //http://localhost:5054/api/product
        public async Task<IActionResult> Post([FromBody] ProductPost dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Amount = dto.Amount,
                Price = dto.Price
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return Created();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //http://localhost:5054/api/product
        public async Task<IActionResult> Get()
        {
            var products = await _context.Products
                .Select(p => new ProductGet
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Amount = p.Amount,
                    Price = p.Price,
                    Category = new Category
                    {
                        Id = p.Category!.Id,
                        Name = p.Category.Name
                    }
                })
                .ToListAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //http://localhost:5054/api/product/{id}
        public async Task<IActionResult> GetByID(int id)
        {
            var productDb = await _context.Products.FindAsync(id);
            if (productDb == null)
                return NotFound();

            var product = await _context.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductGet
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Amount = p.Amount,
                    Price = p.Price,
                    Category = new Category
                    {
                        Id = p.Category!.Id,
                        Name = p.Category.Name
                    }
                })
                .ToListAsync();

            return Ok(product);
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //http://localhost:5054/api/product/{id}
        public async Task<IActionResult> Put(int id, [FromBody] ProductPut dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound(new Response { Title = "NotFound", Status = 404, Message = "The product was not found"});

            var category = await _context.Categories.FindAsync(dto.CategoryId);
            if (category == null)
                return NotFound(new Response { Title = "NotFound", Status = 404, Message = "The category was not found"});

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Amount = dto.Amount;
            product.Price = dto.Price;
            product.CategoryId = dto.CategoryId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //http://localhost:5054/api/product/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var productDB = await _context.Products.FindAsync(id);
            if (productDB == null)
                return NotFound();

            _context.Products.Remove(productDB);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}