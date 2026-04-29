using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaWebApi_JWT.Dtos.Prestamos
{
    public class PrestamoCreateDto
    {
        [Required] public int UsuarioId { get; set; } = 0;


        [Required] public int LibroId { get; set; } = 0;

        public int DiasPrestamo { get; set; } = 0;
    }
}
