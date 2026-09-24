using System;
using System.Collections.Generic;
using System.Text;
using Entities; 

namespace BusinessLogic.Services
{
    public interface IUsuarioService
    {
        Task<int> CrearUsuarioAsync(string nombreUsuario, string password, int idPerfil);
        Task<Usuario?> ValidarCredencialesAsync(string nombreUsuario, string password);
    }
}
