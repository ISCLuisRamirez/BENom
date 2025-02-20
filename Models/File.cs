using System.ComponentModel.DataAnnotations;

public class File
{
    [Key]
    public int id { get; set; }

    [Required]
    public string file_name { get; set; } = string.Empty;

    [Required]
    public byte[] file_data { get; set; } = Array.Empty<byte>();

    [Required]
    public string content_type { get; set; } = string.Empty;

    public DateTime upload_date { get; set; } = DateTime.UtcNow;
     public int id_request { get; set; }
}
