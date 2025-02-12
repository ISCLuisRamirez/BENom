using System.ComponentModel.DataAnnotations;

namespace BENom.Models;

public class Request
{
    public int id { get; set; }
    public int id_reason { get; set; }
    public int id_location { get; set; }
    public int? id_user { get; set; }
    public int? id_via { get; set; }
    public int? id_sublocation { get; set; }
    [StringLength(100, ErrorMessage = "El nombre de sububicación no puede exceder los 100 caracteres.")]
    public string? name_sublocation { get; set; }
    public DateOnly? date { get; set; }
    public string? period { get; set; }
    public string? folio { get; set; } = string.Empty;
    public string? password { get; set; } = string.Empty;
    public string? description { get; set; } = string.Empty;
    public int status { get; set; }
}