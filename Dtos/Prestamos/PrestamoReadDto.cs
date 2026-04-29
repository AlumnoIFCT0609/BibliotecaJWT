using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaWebApi_JWT.Modelos;

namespace BibliotecaWebApi_JWT.Dtos.Prestamos
{
    public class PrestamoReadDto
    {
        public int Id { get; set; }
        public int LibroId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime ? FechaPrestamo {  get; set; }
        public DateTime ? FechaDevolucion {  get; set; }
        public DateTime ? FechaDevolucionReal { get; set; }
        public int DiasPrestamo { get; set; } = 0;
    }
}
