using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BENom.Models;

[Table("requests")]
public class Request
{
    public int id { get; set; }
    public int id_reason { get; set; }
    public int id_location { get; set; }
    public int? id_sublocation { get; set; }
    public int? id_user { get; set; }
    public int? id_user_updated { get; set; }
    public int? id_user_closed { get; set; }
    public int? id_via { get; set; }
    [StringLength(100, ErrorMessage = "El nombre de sububicación no puede exceder los 100 caracteres.")]
    public string? name_sublocation { get; set; }
    public string? via_detail { get; set; }
    public string? period { get; set; }
    public string? folio { get; set; } = string.Empty;
    public string? password { get; set; } = string.Empty;
    public string? description { get; set; } = string.Empty;
    public string? reported { get; set; }
    public int status { get; set; }
    public DateOnly? date { get; set; }
    public DateOnly created_date { get; set; }
    public DateOnly? updated_date { get; set; }
    public DateOnly? closed_date { get; set; }
}