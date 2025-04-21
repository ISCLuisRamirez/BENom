using BENom.Data;
using BENom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BENom.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly BENomDbContext _context;

        public SubjectsController(BENomDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Comite")]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjects([FromQuery] int? id_request)
        {
            var query = _context.Subjects.AsQueryable();
            if (id_request.HasValue)
            {
                query = query.Where(s => s.id_request == id_request.Value);
            }
            var subjects = await query.ToListAsync();
            if (!subjects.Any())
            {
                return NotFound();
            }
            return Ok(subjects);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Comite")]
        public async Task<ActionResult<Subject>> GetSubject(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return subject;
        }

        [HttpPost]
        public async Task<ActionResult<Subject>> PostSubject(Subject subject)
        {
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSubject), new { id = subject.id }, subject);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Comite")]
        public async Task<IActionResult> PutSubject(int id, Subject subject)
        {
            if (id != subject.id)
            {
                return BadRequest("El ID del objeto no coincide.");
            }
            _context.Entry(subject).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(subject);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Comite")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return Content("Objeto eliminado");
        }
    }
}
