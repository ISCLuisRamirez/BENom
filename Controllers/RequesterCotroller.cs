using BENom.Data;
using BENom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BENom.Controllers
{
    [Route("[controller]")]
    [ApiController]
    
    public class RequestersController : ControllerBase
    {
    private readonly BENomDbContext _context;

        public RequestersController(BENomDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Comite")]
        public async Task<ActionResult<IEnumerable<Requester>>> GetRequesters([FromQuery] int? id_request)
        {
            var query = _context.Requesters.AsQueryable();
            if (id_request.HasValue)
            {
                query = query.Where(r => r.id_request == id_request.Value);
            }
            var requesters = await query.ToListAsync();
            if (!requesters.Any())
            {
                return NotFound();
            }
            return Ok(requesters);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Comite")]
        public async Task<ActionResult<Requester>> GetRequester(int id)
        {
            var requester = await _context.Requesters.FindAsync(id);
            if (requester == null)
            {
                return NotFound();
            }
            return requester;
        }

        [HttpPost]
        public async Task<ActionResult<Requester>> PostRequester(Requester requester)
        {
            _context.Requesters.Add(requester);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRequester), new { id = requester.id }, requester);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Comite")]
        public async Task<IActionResult> PutRequester(int id, Requester requester)
        {
            if (id != requester.id)
            {
                return BadRequest("El ID del objeto no coincide.");
            }
            _context.Entry(requester).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(requester);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Comite")]
        public async Task<IActionResult> DeleteRequester(int id)
        {
            var requester = await _context.Requesters.FindAsync(id);
            if (requester == null)
            {
                return NotFound();
            }
            _context.Requesters.Remove(requester);
            await _context.SaveChangesAsync();
            return Content("Objeto eliminado");
        }
    }
}
