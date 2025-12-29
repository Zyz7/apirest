using ApiRest.dtos;
using ApiRest.models;
using ApiRest.repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiRest.controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        public readonly Context _context;

        public CategoryController(Context context)
        {
            _context = context;
        } 

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //http://localhost:5054/api/category
        public async Task<IActionResult> Post([FromBody] CategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return Created();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //http://localhost:5054/api/category
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Categories.ToListAsync());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //http://localhost:5054/api/category/{id}
        public async Task<IActionResult> GetByID(int id)
        {
            var categories = await _context.Categories.FindAsync(id);
            if (categories == null)
                return NotFound();

            return Ok(categories);
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //http://localhost:5054/api/category/{id}
        public async Task<IActionResult> Put(int id, [FromBody] CategoryDto dto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            category.Name = dto.Name;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //http://localhost:5054/api/category/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}