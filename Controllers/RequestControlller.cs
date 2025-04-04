using BENom.Data;
using BENom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BENom.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly BENomDbContext _context;

        public RequestsController(BENomDbContext context)
        {
            _context = context;
        }

        // Obtener todos los objetos
        [HttpGet]
        [Authorize(Roles = "Admin,Comite")]
        public async Task<ActionResult> GetRequests([FromQuery] RequestFiltroDto filtro)
        {
            var idDepartmentClaim = User.FindFirst("id_department");

            if (idDepartmentClaim == null)
            {
                return BadRequest("El usuario no tiene un ID de departamento asignado.");
            }

            // Convertir el valor del claim a entero
            if (!int.TryParse(idDepartmentClaim.Value, out int idDepartment))
            {
                return BadRequest($"El ID del departamento no es válido: {idDepartmentClaim.Value}");
            }

            // Obtener los ID de requests que ya están en Witnesses
            var existingWitnessesDeparmentsIds = await _context.Witnesses
                .Where(w => w.id_department == idDepartment)
                .Select(w => w.id_request)
                .ToListAsync();

            var existingSubjectsDeparmentsIds = await _context.Subjects
                .Where(s => s.id_department == idDepartment)
                .Select(s => s.id_request)
                .ToListAsync();

            var query = _context.Requests.AsQueryable();

            // Excluir las requests que ya están en Witnesses
            query = query.Where(r => !existingWitnessesDeparmentsIds.Contains(r.id));
            query = query.Where(r => !existingSubjectsDeparmentsIds.Contains(r.id));

            // Aplicar filtros
            if (filtro.Pagina <= 0) filtro.Pagina = 1; // Default to page 1
            if (filtro.TamanoPagina <= 0) filtro.TamanoPagina = 10; // Default page size to 10
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

            // Ordenamiento
            var ordenarPor = filtro.OrdenarPor ?? "id";
            bool ordenDesc = filtro.OrdenDesc;
            query = ordenDesc
                ? query.OrderByDescending(e => EF.Property<object>(e, ordenarPor))
                : query.OrderBy(e => EF.Property<object>(e, ordenarPor));

            // Paginación
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

        // Obtener un objeto por ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Comite")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.Requests
                .Where(r => r.id == id)
                .FirstOrDefaultAsync();

            if (request == null)
            {
                return NotFound();
            }

            var userUpdated = await _context.Users
                .Where(u => u.id == request.id_user_updated)
                .FirstOrDefaultAsync();

            var userClosed = await _context.Users
                .Where(u => u.id == request.id_user_closed)
                .FirstOrDefaultAsync();

            var result = new
            {
                request.id,
                request.status,
                request.folio,
                request.password,
                request.created_date,
                request.id_reason,
                request.id_location,
                request.id_sublocation,
                request.date,
                request.updated_date,
                request.closed_date,
                request.id_user_updated,
                request.id_user_closed,
                employee_number_user_updated = userUpdated?.employee_number,
                employee_number_user_closed = userClosed?.employee_number
            };

            return Ok(result);
        }

        // Crear un nuevo objeto
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            string Folio = FolioGenerator();
            request.folio = Folio;
            string Password = PassGenerator(6);
            request.password = BCrypt.Net.BCrypt.HashPassword(Password);
            request.created_date = DateOnly.FromDateTime(DateTime.Now); ;

            _context.Requests.Add(request);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(PostRequest), new { id = request.id }, new
            {
                id = request.id,
                Folio,
                Password
            });
        }

        // Actualizar un objeto
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Comite")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (request == null || id != request.id)
            {
                return BadRequest("El ID del objeto no coincide o la solicitud es inválida.");
            }
            var existingRequest = await _context.Requests.FindAsync(id);
            if (existingRequest == null)
            {
                return NotFound("No se encontró la solicitud con el ID especificado.");
            }
            // Actualizar solo los campos necesarios en lugar de marcar todo como modificado
            existingRequest.status = request.status;
            try
            {
                await _context.SaveChangesAsync();
                return Ok(existingRequest);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Error al actualizar la solicitud: " + ex.Message);
            }
        }


        // Eliminar un objeto
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Comite")]
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

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin,Comite")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] int newStatus)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null || (newStatus != 2 && newStatus != 3 && newStatus != 4))
            {
                return BadRequest("El ID de la solicitud no es válido o el nuevo estado no es válido.");
            }
            {
                return NotFound("No se encontró la solicitud con el ID especificado.");
            }
            request.status = newStatus;
            var userId = User.FindFirst("id")?.Value;
            if (newStatus == 2)
            {
                if (int.TryParse(userId, out int parsedUserId))
                {
                    request.id_user_updated = parsedUserId;
                }
                request.updated_date = DateOnly.FromDateTime(DateTime.UtcNow);
            }
            else
            {
                if (int.TryParse(userId, out int parsedUserId))
                {
                    request.id_user_closed = parsedUserId;
                }
                request.closed_date = DateOnly.FromDateTime(DateTime.UtcNow);
            }
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Estado actualizado correctamente", request });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Error al actualizar el estado: " + ex.Message);
            }
        }

        [HttpGet("counter")]
        [Authorize(Roles = "Admin,Comite")]
        public async Task<ActionResult<object>> GetRequestCountByStatus()
        {
            var counts = await _context.Requests
                .GroupBy(r => r.status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
            int totalRequests = await _context.Requests.CountAsync();
            var statusMap = new Dictionary<int, string>
            {
                { 1, "Registrado" },
                { 2, "En Proceso" },
                { 3, "Finalizado" },
                { 4, "Rechazado" }
            };
            var result = new
            {
                total = totalRequests,
                count = counts.Select(c => c.Count).ToList(),
                status = counts.Select(c => statusMap.ContainsKey(c.Status) ? statusMap[c.Status] : "Desconocido").ToList()
            };
            return Ok(result);
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