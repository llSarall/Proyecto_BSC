using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public int IdPerfil { get; set; }
        public string Passwordhash { get; set; } = string.Empty;
        public bool Estatus { get; set; }


    }
}
