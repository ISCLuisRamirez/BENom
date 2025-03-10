using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BENom.Models;

[Table("reasons")]
public class Reason
{
    public int id { get; set; }
    [StringLength(100, ErrorMessage = "El campo nombre no puede exceder los 100 caracteres.")]
    public string reason_name { get; set; }
}