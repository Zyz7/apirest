using ApiRest.dtos;
using ApiRest.models;
using ApiRest.repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiRest.controllers
{
    [ApiController]
    [Route("api/address")]
    public class AddressController : ControllerBase
    {
        public readonly Context _context;

        public AddressController(Context context)
        {
            _context = context;
        } 

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //http://localhost:5054/api/address
        public async Task<IActionResult> Post([FromBody] AddressDto dto)
        {
            var address = new Address
            {
                Street = dto.Street,
                Num = dto.Num,
                Colony = dto.Colony,
                PostalCode = dto.PostalCode,
                State = dto.State,
                Country = dto.Country     
            };

            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();

            return Created();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //http://localhost:5054/api/address
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Addresses.ToListAsync());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //http://localhost:5054/api/address/{id}
        public async Task<IActionResult> GetByID(int id)
        {
            var addresses = await _context.Addresses.FindAsync(id);
            if (addresses == null)
                return NotFound();

            return Ok(addresses);
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //http://localhost:5054/api/address/{id}
        public async Task<IActionResult> Put(int id, [FromBody] AddressDto dto)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
                return NotFound();

            address.Street = dto.Street;
            address.Num = dto.Num;
            address.Colony = dto.Colony;
            address.PostalCode = dto.PostalCode;
            address.State = dto.State;
            address.Country = dto.Country;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //http://localhost:5054/api/address/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
                return NotFound();

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}