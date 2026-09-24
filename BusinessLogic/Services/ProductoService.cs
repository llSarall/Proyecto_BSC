using DataAccess.Repositories;
using Entities; 
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;

        public ProductoService(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public Task<IEnumerable<Producto>> ObtenerTodosAsync()
        {
            return _productoRepository.ObtenerTodosAsync();
        }
    }
}
