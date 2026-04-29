using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaWebApi_JWT.Modelos;

namespace BibliotecaWebApi_JWT.Dtos.Libros
{

    public class LibroUpdateDto
    {
        [MaxLength(200)] public string? Titulo { get; set; } 
        [MaxLength(150)] public string? Autor { get; set; } 
        [MaxLength(100)] public string? Categoria { get; set; } 
        [Range(1400, 2100)] public int? Anio { get; set; }
        public bool? Disponible { get; set; } 
    }
}
