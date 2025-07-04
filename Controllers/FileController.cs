using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.IO;
using BENom.Data;

[Route("files")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly BENomDbContext _context;

    public FilesController(BENomDbContext context)
    {
        _context = context;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No se proporcionó un archivo válido.");

        string idRequestString = Request.Form["id_request"];
        if (!int.TryParse(idRequestString, out int id_request))
        {
            return BadRequest("El valor de id_request no es un entero válido.");
        }

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);

        var fileModel = new File
        {
            file_name = file.FileName,
            content_type = file.ContentType,
            file_data = memoryStream.ToArray(),
            upload_date = DateTime.UtcNow,
            id_request = id_request
        };

        _context.Files.Add(fileModel);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Archivo subido correctamente", id = fileModel.id });
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Comite")]
    public async Task<IActionResult> GetFile(int id)
    {
        var file = await _context.Files.FindAsync(id);
        if (file == null)
            return NotFound("Archivo no encontrado.");

        return File(file.file_data, file.content_type, file.file_name);
    }

    [HttpGet("get/{id_request}")]
    [Authorize(Roles = "Admin,Comite")]
    public async Task<IActionResult> GetFilesByRequest(int id_request)
    {
        var files = await _context.Files
                                  .Where(f => f.id_request == id_request)
                                  .ToListAsync();

        if (files == null || !files.Any())
        {
            return NotFound("No se encontraron archivos para el id_request proporcionado.");
        }

        var result = files.Select(file => new
        {
            file.id,
            file.file_name,
            file.content_type,
            file.upload_date
        });

        return Ok(result);
    }
}
