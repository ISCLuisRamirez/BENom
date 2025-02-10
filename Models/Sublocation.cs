using System.ComponentModel.DataAnnotations;

namespace BENom.Models;

public class Sublocation
{
    public int id { get; set; }
    public int id_location { get; set; }
    [StringLength(100, ErrorMessage = "El campo nombre no puede exceder los 100 caracteres.")]
    public string sublocation_name { get; set; }
}