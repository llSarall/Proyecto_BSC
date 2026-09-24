using Dapper;
using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataAccess.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public UsuarioRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> CrearAsync(string nombreUsuario, int idPerfil, string passwordHash)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.ExecuteScalarAsync<int>(
                "dbo.sp_crear_usuario",
                new { usuario = nombreUsuario, id_perfil = idPerfil, password_hash = passwordHash },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Usuario?> ObtenerPorNombreAsync(string nombreUsuario)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.QuerySingleOrDefaultAsync<Usuario>(
                "dbo.sp_obtener_usuario_por_nombre",
                new { usuario = nombreUsuario },
                commandType: CommandType.StoredProcedure);
        }
    }
}
