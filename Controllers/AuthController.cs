using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaWebApi_JWT.Dtos.Auth;
using BibliotecaWebApi_JWT.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using static System.Net.Mime.MediaTypeNames;

namespace BibliotecaWebApi_JWT.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    [Produces("application/json", "application/xml")]
    public class AuthController: ControllerBase
    {

        private readonly TokenService _tokenService;
        public AuthController(TokenService tokenService) => _tokenService = tokenService;

        [HttpPost("login")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(typeof(LoginResponseDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status401Unauthorized)]
        public ActionResult<LoginResponseDto> Login([FromBody] LoginRequestDto dto)
        {
            if (!_tokenService.ValidateUser(dto.Username, dto.Password, out var role)) 
            {
                return Unauthorized(new ProblemDetails
                {
                    Title = "No autorizado",
                    Detail = "Usuario o contraseña incorrectos.",
                    Status = 401
                });
            }
            var (token, expiresIn) = _tokenService.CreateToken(dto.Username, role);
            return Ok(new LoginResponseDto
            {
                AccessToken = token,
                ExpiresInSeconds = expiresIn
            });
        }
    }
}
