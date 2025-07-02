using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BENom.Models;

[Table("vias")]
public class Via
{
    public int id { get; set; }
    [StringLength(100, ErrorMessage = "El campo nombre no puede exceder los 100 caracteres.")]
    public required string name { get; set; }

}