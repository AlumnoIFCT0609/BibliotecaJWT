using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaWebApi_JWT.Dtos.Usuarios
{
    public class UsuarioUpdateDto
    {

        [MaxLength(150)] public string? Nombre { get; set; }
       //No actualizamos DNI al ser campo de especial seguridad
       //[MaxLength(9)] public string Dni { get; set; }
    }
}
