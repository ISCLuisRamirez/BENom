using BENom.Data;
using BENom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BENom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RequestersController : ControllerBase
    {
    private readonly BENomDbContext _context;

        public RequestersController(BENomDbContext context)
        {
            _context = context;
        }

        // Obtener todos los objetos
        [HttpGet]
        [Authorize(Roles = "Admin,Comite")]
        public async Task<ActionResult<IEnumerable<Requester>>> GetRequesters()
        {
            return await _context.Requesters.ToListAsync();
        }

        // Obtener un objeto por ID
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

        // Crear un nuevo objeto
        [HttpPost]
        public async Task<ActionResult<Requester>> PostRequester(Requester requester)
        {
            _context.Requesters.Add(requester);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRequester), new { id = requester.id }, requester);
        }

        // Actualizar un objeto
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

        // Eliminar un objeto
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
