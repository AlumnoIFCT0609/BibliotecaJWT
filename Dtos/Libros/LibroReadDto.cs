using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaWebApi_JWT.Modelos;

namespace BibliotecaWebApi_JWT.Dtos.Libros
{
    public class LibroReadDto
    {
        public int Id { get; set; } = 0;
        public string Titulo { get; set; } = String.Empty;
        public string Autor {  get; set; } = String.Empty;
        public string Categoria {  get; set; }=String.Empty;
        public int Anio {  get; set; }=0;
        public bool Disponible { get; set; }=true;

      
    }
}
