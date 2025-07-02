using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BENom.Models;

[Table("sublocations")]
public class Sublocation
{
    public int id { get; set; }
    public int id_location { get; set; }
    [StringLength(100, ErrorMessage = "El campo nombre no puede exceder los 100 caracteres.")]
    public required string sublocation_name { get; set; }
}