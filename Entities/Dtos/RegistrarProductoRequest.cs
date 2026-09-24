using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos;

public class RegistrarProductoRequest
{
    [Required(ErrorMessage = "La clave es obligatoria.")]
    [StringLength(50)]
    public string ClaveProducto { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(50)]
    public string NombreProducto { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "La existencia no puede ser negativa.")]
    public int Existencia { get; set; }
}