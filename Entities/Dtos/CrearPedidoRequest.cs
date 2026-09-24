using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos;

public class CrearPedidoRequest
{
    [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
    [StringLength(255)]
    public string NombreCliente { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un producto.")]
    public int IdProducto { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
    public int Cantidad { get; set; }
}