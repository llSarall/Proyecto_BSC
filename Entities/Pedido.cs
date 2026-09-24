using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public int IdUsuarioRegistro { get; set; }
        public DateTime FechaRegistroPedido { get; set; }

        public List<DetallePedido> Detalles { get; set; } = new();
    }
}
