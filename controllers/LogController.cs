using ApiRest.models;
using ApiRest.repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiRest.controllers
{
    [ApiController]
    [Route("api/log")]
    public class LogController : ControllerBase
    {
        public readonly Context _context;

        public LogController(Context context)
        {
            _context = context;
        } 

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //http://localhost:5054/api/log
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Logs.ToListAsync());
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //http://localhost:5054/api/log/{id}
        public async Task<IActionResult> GetByID(int id)
        {
            var log = await _context.Logs.FindAsync(id);
            if (log == null)
                return NotFound();

            return Ok(log);
        }

    }
}