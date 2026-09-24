using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string ClaveProducto { get; set; } = string.Empty;
        public string NombreProducto { get; set; } = string.Empty;
        public int Existencia { get; set; }
        public int IdUsuarioRegistro { get; set; }
        public DateTime FechaRegistroProducto { get; set; }
    }
}
