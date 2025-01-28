namespace BENom.Models;

public class Witness
{
    public int id { get; set; }
    public int id_request { get; set; }
    public string name { get; set; }
    public string position { get; set; } = string.Empty;
    public string employee_number { get; set; } = string.Empty;
    public string phone { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
}