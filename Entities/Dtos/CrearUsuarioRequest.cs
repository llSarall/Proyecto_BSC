using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace Entities.Dtos
{
    public class CrearUsuarioRequest
    {
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [StringLength(50)]
        public string Usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Password { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Selecciona un perfil.")]
        public int IdPerfil { get; set; }
    }
}

