namespace Entities.Dtos;

public class ReporteExistencia
{
    public int IdProducto { get; set; }
    public string ClaveProducto { get; set; } = string.Empty;
    public string NombreProducto { get; set; } = string.Empty;
    public int Existencia { get; set; }
    public string EstatusExistencia { get; set; } = string.Empty;
}