namespace BENom.Models;

public class Request
{
    public int id { get; set; }
    public int id_requesters { get; set; }
    public int id_reason { get; set; }
    public int id_location { get; set; }
    public int id_sublocation { get; set; }
    public DateOnly date { get; set; }
    public string file { get; set; }
    public string folio { get; set; }
    public string password { get; set; }
    public byte status { get; set; }
}