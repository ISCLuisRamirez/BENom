using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BENom.Models;

[Table("roles")]
public class Role
{
    public int id { get; set; }
    [StringLength(50, ErrorMessage = "El campo nombre no puede exceder los 50 caracteres.")]
    public required string role_name { get; set; }
}