using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaWebApi_JWT.Modelos;

namespace BibliotecaWebApi_JWT.Dtos.Libros
{
    public class LibroCreateDto
    {
        [Required, MaxLength(200)] public string Titulo { get; set; } = string.Empty;
        [Required, MaxLength(150)] public string Autor { get; set; } = string.Empty;
        [Required, MaxLength(100)] public string Categoria { get; set; } = string.Empty;
        [Range (1400, 2100)]   public int Anio { get; set;} = DateTime.Now.Year;
    }
}
