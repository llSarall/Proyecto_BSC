using Entities;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Services
{
    public interface IProductoService
    {
        Task<IEnumerable<Producto>> ObtenerTodosAsync();
        Task<int> RegistrarAsync(RegistrarProductoRequest request, int idUsuarioRegistro);
        Task<IEnumerable<ReporteExistencia>> ObtenerReporteExistenciasAsync();
    }
}
