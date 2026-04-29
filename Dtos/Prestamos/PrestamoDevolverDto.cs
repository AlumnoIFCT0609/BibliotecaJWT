using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaWebApi_JWT.Dtos.Prestamos
{
    public class PrestamoDevolverDto
    {
        [Required] int UsuarioId { get; set; }
        [Required] int LibroId { get; set; }
        DateTime ? FechaDevolucionReal {  get; set; }= DateTime.Now;
    }
}
