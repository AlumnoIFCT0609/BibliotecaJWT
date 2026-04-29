using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaWebApi_JWT.Dtos.Usuarios
{
    public class UsuarioReadDto
    {
        public int Id { get; set; } = 0;
        public string Nombre { get; set; }= string.Empty;
        public string DNI {  get; set; }=string.Empty;
        public DateTime FechaAlta {  get; set; }
        public int DiasPrestamo { get; set; }    
        
    }
}
