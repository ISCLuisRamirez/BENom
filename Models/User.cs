using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BENom.Models;

[Table("users")]
public class User
{
    public int id { get; set; }
    public int id_role { get; set; }
    public int id_department {get; set;}
    [StringLength(8, ErrorMessage = "El numero de empleado no puede exceder los 8 caracteres.")]
    public required string employee_number { get; set; }
    public required string email { get; set; }
    public required string password { get; set; }
}
