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
    public class RolesController : ControllerBase
    {
        private readonly BENomDbContext _context;

        public RolesController(BENomDbContext context)
        {
            _context = context;
        }

        // Obtener todos los roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        // Obtener un rol por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        // Crear un nuevo rol
        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetRole), new { id = role.id }, role);
        }

        // Actualizar un rol
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, Role role)
        {
            if (id != role.id)
            {
                return BadRequest("El ID del producto no coincide.");
            }

            _context.Entry(role).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(role);
        }

        // Eliminar un rol
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
