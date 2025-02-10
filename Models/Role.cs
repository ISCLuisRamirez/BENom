using System.ComponentModel.DataAnnotations;

namespace BENom.Models;

public class Role
{
    public int id { get; set; }
    [StringLength(100, ErrorMessage = "El campo nombre no puede exceder los 100 caracteres.")]
    public string role_name { get; set; }
}