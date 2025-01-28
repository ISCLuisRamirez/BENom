using BENom.Data;
using BENom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
/* using Microsoft.AspNetCore.Authorization; */ // Descomenta esta linea para proteger con authenticate 

namespace BENom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /* [Authorize(Roles = "Admin")] */ // Protege todos los endpoints para usuarios con rol "Admin"
    public class SublocationsController : ControllerBase
    {
    private readonly BENomDbContext _context;

        public SublocationsController(BENomDbContext context)
        {
            _context = context;
        }

        // Obtener todos los objetos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sublocation>>> GetSublocation()
        {
            return await _context.Sublocations.ToListAsync();
        }

        // Obtener un objeto por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Sublocation>> GetSublocation(int id)
        {
            var sublocation = await _context.Sublocations.FindAsync(id);

            if (sublocation == null)
            {
                return NotFound();
            }

            return sublocation;
        }

        // Crear un nuevo objeto
        [HttpPost]
        public async Task<ActionResult<Sublocation>> PostSublocation(Sublocation sublocation)
        {
            _context.Sublocations.Add(sublocation);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetSublocation), new { id = sublocation.id }, sublocation);
        }

        // Actualizar un objeto
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSublocation(int id, Sublocation sublocation)
        {
            if (id != sublocation.id)
            {
                return BadRequest("El ID del objeto no coincide.");
            }

            _context.Entry(sublocation).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(sublocation);
        }

        // Eliminar un objeto
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSublocation(int id)
        {
            var sublocation = await _context.Sublocations.FindAsync(id);

            if (sublocation == null)
            {
                return NotFound();
            }

            _context.Sublocations.Remove(sublocation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
