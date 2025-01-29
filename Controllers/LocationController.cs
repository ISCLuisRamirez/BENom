using BENom.Data;
using BENom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization; // Esta linea para proteger con authenticate 

namespace BENom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Solo usuarios con rol "Admin" pueden acceder a estos endpoints
    public class LocationsController : ControllerBase
    {
    private readonly BENomDbContext _context;

        public LocationsController(BENomDbContext context)
        {
            _context = context;
        }

        // Obtener todos los objetos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
        {
            return await _context.Locations.ToListAsync();
        }

        // Obtener un objeto por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocation(int id)
        {
            var location = await _context.Locations.FindAsync(id);

            if (location == null)
            {
                return NotFound();
            }

            return location;
        }

        // Crear un nuevo objeto
        [HttpPost]
        public async Task<ActionResult<Location>> PostLocation(Location location)
        {
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetLocation), new { id = location.id }, location);
        }

        // Actualizar un objeto
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(int id, Location location)
        {
            if (id != location.id)
            {
                return BadRequest("El ID del objeto no coincide.");
            }

            _context.Entry(location).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(location);
        }

        // Eliminar un objeto
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var location = await _context.Locations.FindAsync(id);

            if (location == null)
            {
                return NotFound();
            }

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();

            return Content("Objeto eliminado");
        }
    }
}
