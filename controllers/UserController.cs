using ApiRest.dtos;
using ApiRest.models;
using ApiRest.repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiRest.controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        public readonly Context _context;

        public UserController(Context context)
        {
            _context = context;
        } 
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //http://localhost:5054/api/user
        public async Task<IActionResult> Post([FromBody] UserPost dto)
        {
            var user = new User
            {
                Name = dto.Name,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = dto.Password,
                Active = 'Y'
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Created();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //http://localhost:5054/api/user
        public async Task<IActionResult> Get()
        {
            var users = await _context.Users
                .Where(u => u.Active == 'Y').Include(u => u.Address)
                .Select(u => new UserGet
                {
                    Id = u.Id,
                    Name = u.Name,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Address = new Address
                    {
                        Id = u.Address!.Id,
                        Street = u.Address.Street,
                        Num = u.Address.Num,
                        Colony = u.Address.Colony,
                        PostalCode = u.Address.PostalCode,
                        State = u.Address.State,
                        Country = u.Address.Country
                    }
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //http://localhost:5054/api/user/{id}
        public async Task<IActionResult> GetByID(int id)
        {
            var userDb = await _context.Users.FindAsync(id);
            if (userDb == null)
                return NotFound();

            var user = await _context.Users
                .Where(u => u.Active == 'Y' && u.Id == id)
                .Include(u => u.Address)
                .Select(u => new UserGet
                {
                    Id = u.Id,
                    Name = u.Name,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Address = new Address
                    {
                        Id = u.Address!.Id,
                        Street = u.Address.Street,
                        Num = u.Address.Num,
                        Colony = u.Address.Colony,
                        PostalCode = u.Address.PostalCode,
                        State = u.Address.State,
                        Country = u.Address.Country
                    }
                })
                .FirstOrDefaultAsync();

            return Ok(user);
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //http://localhost:5054/api/user/{id}
        public async Task<IActionResult> Put(int id, [FromBody] UserPut dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new Response { Title = "NotFound", Status = 404, Message = "The user was not found"});

            var address = await _context.Addresses.FindAsync(dto.AddressId);
            if (address == null)
                return NotFound(new Response { Title = "NotFound", Status = 404, Message = "The address was not found"});

            user.Name = dto.Name;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.AddressId = dto.AddressId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("pass/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //http://localhost:5054/api/user/pass/{id}
        public async Task<IActionResult> Pass(int id, [FromBody] Password dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            if (user.Password == dto.Current)
                user.Password = dto.Confirm;
            else 
                return Unauthorized();

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //http://localhost:5054/api/user/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            user.Active = 'N';
            //_context.users.Remove(userDB);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}