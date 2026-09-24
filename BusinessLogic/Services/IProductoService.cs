using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace BusinessLogic.Services
{
    public interface IProductoService
    {
        Task<IEnumerable<Producto>> ObtenerTodosAsync();
    }
}
