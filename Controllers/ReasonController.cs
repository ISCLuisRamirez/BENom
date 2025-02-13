using BENom.Data;
using BENom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BENom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Protege todos los endpoints para usuarios con rol "Admin"
    public class ReasonsController : ControllerBase
    {
        private readonly BENomDbContext _context;

        public ReasonsController(BENomDbContext context)
        {
            _context = context;
        }

        // Obtener todos los roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reason>>> GetReasons()
        {
            return await _context.Reasons.ToListAsync();
        }

        // Obtener un rol por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Reason>> GetReason(int id)
        {
            var reason = await _context.Reasons.FindAsync(id);
            if (reason == null)
            {
                return NotFound();
            }
            return reason;
        }

        // Crear un nuevo rol
        [HttpPost]
        public async Task<ActionResult<Reason>> PostReason(Reason reason)
        {
            _context.Reasons.Add(reason);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReason), new { id = reason.id }, reason);
        }

        // Actualizar un rol
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReason(int id, Reason reason)
        {
            if (id != reason.id)
            {
                return BadRequest("El ID del objeto no coincide.");
            }
            _context.Entry(reason).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(reason);
        }

        // Eliminar un rol
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReason(int id)
        {
            var reason = await _context.Reasons.FindAsync(id);
            if (reason == null)
            {
                return NotFound();
            }
            _context.Reasons.Remove(reason);
            await _context.SaveChangesAsync();
            return Content("Objeto eliminado");
        }
    }
}
