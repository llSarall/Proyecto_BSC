using DataAccess.Repositories;
using Entities; 
using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.Exceptions;
using Entities.Dtos;

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

        public Task<int> RegistrarAsync(RegistrarProductoRequest request, int idUsuarioRegistro)
        {
            var clave = request.ClaveProducto.Trim().ToUpperInvariant();
            var nombre = request.NombreProducto.Trim();

            if (string.IsNullOrEmpty(clave))
                throw new ReglaNegocioException("La clave del producto es obligatoria.");

            if (string.IsNullOrEmpty(nombre))
                throw new ReglaNegocioException("El nombre del producto es obligatorio.");

            if (request.Existencia < 0)
                throw new ReglaNegocioException("La existencia no puede ser negativa.");

            var producto = new Producto
            {
                ClaveProducto = clave,
                NombreProducto = nombre,
                Existencia = request.Existencia,
                IdUsuarioRegistro = idUsuarioRegistro
            };

            return _productoRepository.RegistrarAsync(producto);
        }

        public Task<IEnumerable<ReporteExistencia>> ObtenerReporteExistenciasAsync()
        {
            return _productoRepository.ObtenerReporteExistenciasAsync();
        }

    }
}
