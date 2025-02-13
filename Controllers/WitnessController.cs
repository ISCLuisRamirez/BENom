using BENom.Data;
using BENom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BENom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WitnessesController : ControllerBase
    {
    private readonly BENomDbContext _context;

        public WitnessesController(BENomDbContext context)
        {
            _context = context;
        }

        // Obtener todos los objetos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Witness>>> GetWitnesses()
        {
            return await _context.Witnesses.ToListAsync();
        }

        // Obtener un objeto por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Witness>> GetWitness(int id)
        {
            var witness = await _context.Witnesses.FindAsync(id);
            if (witness == null)
            {
                return NotFound();
            }
            return witness;
        }

        // Crear un nuevo objeto
        [HttpPost]
        public async Task<ActionResult<Witness>> PostWitness(Witness witness)
        {
            _context.Witnesses.Add(witness);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetWitness), new { id = witness.id }, witness);
        }

        // Actualizar un objeto
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWitness(int id, Witness witness)
        {
            if (id != witness.id)
            {
                return BadRequest("El ID del objeto no coincide.");
            }
            _context.Entry(witness).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(witness);
        }

        // Eliminar un objeto
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWitness(int id)
        {
            var witness = await _context.Witnesses.FindAsync(id);
            if (witness == null)
            {
                return NotFound();
            }
            _context.Witnesses.Remove(witness);
            await _context.SaveChangesAsync();
            return Content("Objeto eliminado");
        }
    }
}
