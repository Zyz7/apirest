using ApiRest.dtos;
using ApiRest.models;
using ApiRest.repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiRest.controllers
{
    [ApiController]
    [Route("api/supplier")]
    public class SupplierController : ControllerBase
    {
        public readonly Context _context;

        public SupplierController(Context context)
        {
            _context = context;
        } 

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //http://localhost:5054/api/supplier
        public async Task<IActionResult> Post([FromBody] SupplierPost dto)
        {
            var supplier = new Supplier
            {
                Name = dto.Name,
                Email = dto.Email,
                Telephone = dto.Telephone
            };

            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();

            return Created();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //http://localhost:5054/api/supplier
        public async Task<IActionResult> Get()
        {
            var suppliers = await _context.Suppliers
                .Select(s => new SupplierGet
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    Telephone = s.Telephone,
                    Address = new Address
                    {
                        Id = s.Address!.Id,
                        Street = s.Address.Street,
                        Num = s.Address.Num,
                        Colony = s.Address.Colony,
                        PostalCode = s.Address.PostalCode,
                        State = s.Address.State,
                        Country = s.Address.Country
                    }
                })
                .ToListAsync();

            return Ok(suppliers);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //http://localhost:5054/api/supplier/{id}
        public async Task<IActionResult> GetByID(int id)
        {
            var supplierDb = await _context.Suppliers.FindAsync(id);
            if (supplierDb == null)
                return NotFound();

            var supplier = await _context.Suppliers
                .Where(s => s.Id == id)
                .Select(s => new SupplierGet
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    Telephone = s.Telephone,
                    Address = new Address
                    {
                        Id = s.Address!.Id,
                        Street = s.Address.Street,
                        Num = s.Address.Num,
                        Colony = s.Address.Colony,
                        PostalCode = s.Address.PostalCode,
                        State = s.Address.State,
                        Country = s.Address.Country
                    }
                })
                .ToListAsync();

            return Ok(supplier);
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //http://localhost:5054/api/supplier/{id}
        public async Task<IActionResult> Put(int id, [FromBody] SupplierPut dto)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
                return NotFound(new Response { Title = "NotFound", Status = 404, Message = "The supplier was not found"});

            var address = await _context.Addresses.FindAsync(dto.AddressId);
            if (address == null)
                return NotFound(new Response { Title = "NotFound", Status = 404, Message = "The address was not found"});

            supplier.Name = dto.Name;
            supplier.Email = dto.Email;
            supplier.Telephone = dto.Telephone;
            supplier.AddressId = dto.AddressId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //http://localhost:5054/api/supplier/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
                return NotFound();

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}