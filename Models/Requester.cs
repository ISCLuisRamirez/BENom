using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BENom.Models;

[Table("requesters")]
public class Requester
{
    public int id { get; set; }
    public int id_request { get; set; }
    [StringLength(150, ErrorMessage = "El campo nombre no puede exceder los 150 caracteres.")]
    public required string name { get; set; }
    [StringLength(150, ErrorMessage = "El campo puesto no puede exceder los 150 caracteres.")]
    public string? position { get; set; }
    [StringLength(8, ErrorMessage = "El número de empleado no puede exceder los 8 caracteres.")]
    public string? employee_number { get; set; }
    [StringLength(10, ErrorMessage = "El teléfono no puede exceder los 10 caracteres.")]
    public string? phone { get; set; }
    [StringLength(100, ErrorMessage = "El campo correo no puede exceder los 150 caracteres.")]
    public string? email { get; set; }
}