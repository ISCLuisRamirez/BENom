using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BENom.Models;

[Table("users")]
public class User
{
    public int id { get; set; }
    public int id_role { get; set; }
    public int id_department {get; set;}
    [StringLength(6, ErrorMessage = "El numero de empleado no puede exceder los 6 caracteres.")]
    public string employee_number { get; set; }
    public string email { get; set; }
    public string password { get; set; } // Contraseña encriptada
}
