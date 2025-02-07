public class RequestFiltroDto
{
    public int? IdRequesters { get; set; }
    public int? IdReason { get; set; }
    public int? IdLocation { get; set; }
    public int? IdSublocation { get; set; }
    public DateOnly? FechaDesde { get; set; }
    public DateOnly? FechaHasta { get; set; }
    public byte? Status { get; set; }
    public string? Folio { get; set; }

    public string? OrdenarPor { get; set; } = "folio"; // Campo para ordenar
    public bool OrdenDesc { get; set; } = false; // Indica si el orden es descendente

    public int Pagina { get; set; } = 1;
    public int TamanoPagina { get; set; } = 10;
}
