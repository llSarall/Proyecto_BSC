using Entities;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Entities.Dtos;

namespace DataAccess.Repositories
{
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> ObtenerTodosAsync();
        Task<int> RegistrarAsync(Producto producto);
        Task<IEnumerable<ReporteExistencia>> ObtenerReporteExistenciasAsync();
    }
}
