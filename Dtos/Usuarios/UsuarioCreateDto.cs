using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaWebApi_JWT.Dtos.Usuarios
{
    public class UsuarioCreateDto
    {
        [Required,MaxLength(150)] public string Nombre {  get; set; }=string.Empty;
        [Required, MaxLength(9)] public string DNI { get; set; } = string.Empty;
    }
}
