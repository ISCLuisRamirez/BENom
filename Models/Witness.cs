using System.ComponentModel.DataAnnotations;

namespace BENom.Models;

public class Witness
{
    public int id { get; set; }
    public int id_request { get; set; }
    [StringLength(100, ErrorMessage = "El campo nombre no puede exceder los 100 caracteres.")]
    public string name { get; set; }
    public string? position { get; set; }
    [StringLength(6, ErrorMessage = "El número de empleado no puede exceder los 6 caracteres.")]
    public string? employee_number { get; set; }
    [StringLength(10, ErrorMessage = "El teléfono no puede exceder los 10 caracteres.")]
    public string? phone { get; set; }
    public string? email { get; set; }
}