using BibliotecaWebApi_JWT.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;




namespace BibliotecaWebApi_JWT.Servicios
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }
        public (string token, int expiresInSeconds) CreateToken(string username, string role)
        {
            var jwt = _config.GetSection("Jwt");
            string issuer = jwt["Issuer"]!;
            string audience = jwt["Audience"]!;
            string key = jwt["Key"]!;
            int mins = int.TryParse(jwt["ExpiresMinutes"], out var m) ? m : 120;
            var claims = new List<Claim>{
            new(ClaimTypes.Name,username),
            new(ClaimTypes.Role,role),
            new("scope","api")
            };
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(mins),
                signingCredentials: creds
            );
            return (new JwtSecurityTokenHandler().WriteToken(token), mins * 60);
        }
        public bool ValidateUser(string username, string password, out string role)
        {
            role = "";
            foreach (var u in _config.GetSection("Auth:Users").GetChildren())
            {
                var un = u["username"];
                var pw = u["password"];
                var rl = u["role"] ?? "User";
                if (string.Equals(un, username, StringComparison.OrdinalIgnoreCase) && pw == password)
                {
                    role = rl;
                    return true;
                }
                

            }
            return false;
        }
    }
}
