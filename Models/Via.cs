using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BENom.Models;

[Table("vias")]
public class Via
{
    public int id { get; set; }
    public required string name { get; set; }

}