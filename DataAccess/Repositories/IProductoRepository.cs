using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace DataAccess.Repositories
{
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> ObtenerTodosAsync();
    }
}
