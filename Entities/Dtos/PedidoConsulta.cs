namespace Entities.Dtos;

public class PedidoConsulta
{
    public int IdPedido { get; set; }
    public DateTime FechaRegistroPedido { get; set; }
    public string NombreCliente { get; set; } = string.Empty;
    public string Vendedor { get; set; } = string.Empty;
    public string ClaveProducto { get; set; } = string.Empty;
    public string NombreProducto { get; set; } = string.Empty;
    public int Cantidad { get; set; }
}