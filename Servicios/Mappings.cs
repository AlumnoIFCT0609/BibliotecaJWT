using BibliotecaWebApi_JWT.Dtos.Libros;
using BibliotecaWebApi_JWT.Dtos.Prestamos;
using BibliotecaWebApi_JWT.Dtos.Usuarios;
using BibliotecaWebApi_JWT.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaWebApi_JWT.Servicios
{
    public static class Mappings
    {
        public static LibroReadDto ToReadDto(this Libro libro)
        {
            return new LibroReadDto
            {
                Id = libro.Id,
                Titulo = libro.Titulo,
                Autor = libro.Autor,
                Categoria = libro.Categoria,
                Anio = libro.Anio,
                Disponible = libro.Disponible
            };
        }

        
        public static UsuarioReadDto ToReadDto(this Usuario usuario) {
            return new UsuarioReadDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                DNI = usuario.DNI,
                FechaAlta = usuario.FechaAlta
            };
        }
        public static PrestamoReadDto ToReadDto(this Prestamo prestamo) {
            return new PrestamoReadDto
            {
                Id = prestamo.Id,
                LibroId = prestamo.LibroId,
                UsuarioId = prestamo.UsuarioId,
                FechaDevolucion= prestamo.FechaDevolucion,
                FechaDevolucionReal= prestamo.FechaDevolucionReal,
                FechaPrestamo= prestamo.FechaPrestamo
            };
            
        }


    }
}
