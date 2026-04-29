using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaWebApi_JWT.Dtos.Auth
{
    public class LoginRequestDto
    {
        [Required] public string Username { get; set; }= string.Empty;

        [Required] public string Password { get; set;}= string.Empty;
    }
}
