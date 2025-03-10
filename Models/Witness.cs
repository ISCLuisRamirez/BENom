using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BENom.Models;

[Table("witnesses")]
public class Witness
{
    public int id { get; set; }
    public int id_request { get; set; }
    public int? id_department {get; set;}
    [StringLength(100, ErrorMessage = "El campo nombre no puede exceder los 100 caracteres.")]
    public string name { get; set; }
    public string? position { get; set; }
    [StringLength(6, ErrorMessage = "El número de empleado no puede exceder los 6 caracteres.")]
    public string? employee_number { get; set; }
}