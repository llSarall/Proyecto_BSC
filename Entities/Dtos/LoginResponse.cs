using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiracion { get; set; }
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Perfil { get; set; } = string.Empty;
    }
}
