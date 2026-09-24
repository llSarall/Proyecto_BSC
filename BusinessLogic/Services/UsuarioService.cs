using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.Exceptions;
using DataAccess.Repositories;
using Entities;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogic.Services
{
    public class UsuarioService : IUsuarioService
    {
        private const int LongitudMinimaPassword = 8;

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly PasswordHasher<Usuario> _passwordHasher = new();

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<int> CrearUsuarioAsync(string nombreUsuario, string password, int idPerfil)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                throw new ReglaNegocioException("El nombre de usuario es obligatorio.");

            ValidarPassword(password);

            var existente = await _usuarioRepository.ObtenerPorNombreAsync(nombreUsuario);
            if (existente is not null)
                throw new ReglaNegocioException("El nombre de usuario ya está registrado.");

            var usuario = new Usuario { NombreUsuario = nombreUsuario, IdPerfil = idPerfil };
            var passwordHash = _passwordHasher.HashPassword(usuario, password);

            return await _usuarioRepository.CrearAsync(nombreUsuario, idPerfil, passwordHash);
        }

        public async Task<Usuario?> ValidarCredencialesAsync(string nombreUsuario, string password)
        {
            var usuario = await _usuarioRepository.ObtenerPorNombreAsync(nombreUsuario);

            if (usuario is null || !usuario.Estatus)
                return null;

            try
            {
                var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, password);
                return resultado == PasswordVerificationResult.Failed ? null : usuario;
            }
            catch (FormatException)
            {
                // El hash guardado no tiene un formato válido (ej. usuarios de prueba con hash temporal)
                return null;
            }
        }

        private static void ValidarPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < LongitudMinimaPassword)
                throw new ReglaNegocioException($"La contraseña debe tener al menos {LongitudMinimaPassword} caracteres.");

            if (!password.Any(char.IsLetter))
                throw new ReglaNegocioException("La contraseña debe contener al menos una letra.");

            if (!password.Any(char.IsDigit))
                throw new ReglaNegocioException("La contraseña debe contener al menos un número.");
        }
    }
}
