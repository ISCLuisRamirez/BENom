using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/email")]
[ApiController]
public class EmailController : ControllerBase
{
    private readonly EmailService _emailService;

    public EmailController(EmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
    {
        var success = await _emailService.SendEmailAsync(request.To, request.Subject, request.Body);
        if (success)
            return Ok(new { message = "Correo enviado correctamente" });
        
        return BadRequest(new { message = "Error al enviar correo" });
    }
}

public class EmailRequest
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}
