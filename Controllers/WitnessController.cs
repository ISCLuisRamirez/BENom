using BENom.Data;
using BENom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BENom.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WitnessesController : ControllerBase
    {
        private readonly BENomDbContext _context;

        public WitnessesController(BENomDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Comite")]
        public async Task<ActionResult<IEnumerable<Witness>>> GetWitnesses([FromQuery] int? id_request)
        {
            var query = _context.Witnesses.AsQueryable();
            if (id_request.HasValue)
            {
                query = query.Where(w => w.id_request == id_request.Value);
            }
            var witnesses = await query.ToListAsync();
            if (!witnesses.Any())
            {
                return NotFound();
            }
            return Ok(witnesses);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Comite")]
        public async Task<ActionResult<Witness>> GetWitness(int id)
        {
            var witness = await _context.Witnesses.FindAsync(id);
            if (witness == null)
            {
                return NotFound();
            }
            return witness;
        }

        [HttpPost]
        public async Task<ActionResult<Witness>> PostWitness(Witness witness)
        {
            _context.Witnesses.Add(witness);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetWitness), new { id = witness.id }, witness);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Comite")]
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Comite")]
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
