namespace BENom.Models;

public class User
{
    public int id { get; set; }
    public int id_cat_role { get; set; }
    public string employee_number { get; set; }
    public string email { get; set; }
    public string password { get; set; } // Contraseña encriptada
}
