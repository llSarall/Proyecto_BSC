using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace DataAccess.Repositories
{
    public interface IUsuarioRepository
    {
        Task<int> CrearAsync(string nombreUsuario, int idPerfil, string passwordHash);
        Task<Usuario?> ObtenerPorNombreAsync(string nombreUsuario);
    }
}
