using BENom.Data;
using BENom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualBasic; // Esta linea para proteger con authenticate 

namespace BENom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /* [Authorize(Roles = "Admin")] */ // Solo usuarios con rol "Admin" pueden acceder a estos endpoints
    public class RequestsController : ControllerBase
    {
        private readonly BENomDbContext _context;

        public RequestsController(BENomDbContext context)
        {
            _context = context;
        }

        // Obtener todos los objetos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests([FromQuery] RequestFiltroDto filtro)
        {
            var query = _context.Requests.AsQueryable();

            if (filtro.IdReason.HasValue)
                query = query.Where(r => r.id_reason == filtro.IdReason.Value);

            if (filtro.IdLocation.HasValue)
                query = query.Where(r => r.id_location == filtro.IdLocation.Value);

            if (filtro.IdSublocation.HasValue)
                query = query.Where(r => r.id_sublocation == filtro.IdSublocation.Value);

            if (filtro.FechaDesde.HasValue)
                query = query.Where(r => r.date >= filtro.FechaDesde.Value);

            if (filtro.FechaHasta.HasValue)
                query = query.Where(r => r.date <= filtro.FechaHasta.Value);

            if (filtro.Status.HasValue)
                query = query.Where(r => r.status == filtro.Status.Value);

            if (!string.IsNullOrEmpty(filtro.Folio))
                query = query.Where(r => r.folio.Contains(filtro.Folio));

            var ordenarPor = filtro.OrdenarPor ?? "id";
            bool ordenDesc = filtro.OrdenDesc;

            query = ordenDesc
                ? query.OrderByDescending(e => EF.Property<object>(e, ordenarPor))
                : query.OrderBy(e => EF.Property<object>(e, ordenarPor));

            int totalRegistros = await query.CountAsync();
            var requests = await query
                .Skip((filtro.Pagina - 1) * filtro.TamanoPagina)
                .Take(filtro.TamanoPagina)
                .ToListAsync();

            return Ok(new
            {
                TotalRegistros = totalRegistros,
                PaginaActual = filtro.Pagina,
                TamanoPagina = filtro.TamanoPagina,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistros / filtro.TamanoPagina),
                Datos = requests
            });
        }

        // Obtener un objeto por ID
        [HttpGet("search")]
        public async Task<ActionResult<object>> GetRequestAsync([FromQuery] string folio, [FromQuery] string password, BENomDbContext db)
        {
            var request = await db.Requests.FirstOrDefaultAsync(r => r.folio == folio);

            if (request == null || !BCrypt.Net.BCrypt.Verify(password, request.password))
                return NotFound("No hay registro");

            var requestStatus = request.status;
            return Ok(new { requestStatus });
        }

        // Crear un nuevo objeto
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            string Folio = FolioGenerator();
            request.folio = Folio;
            string Password = PassGenerator(6);
            request.password = BCrypt.Net.BCrypt.HashPassword(Password);

            _context.Requests.Add(request);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Folio,
                Password
            });
        }

        // Actualizar un objeto
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.id)
            {
                return BadRequest("El ID del objeto no coincide.");
            }

            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(request);
        }

        // Eliminar un objeto
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return Content("Objeto eliminado");
        }

        static string FolioGenerator()
        {
            Random random = new Random();
            int folio = random.Next(100000, 1000000); // Asegura 6 dígitos
            return folio.ToString();
        }

        static string PassGenerator(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            char[] pass = new char[length];

            for (int i = 0; i < length; i++)
            {
                pass[i] = chars[random.Next(chars.Length)];
            }

            return new string(pass);
        }
    }
}
