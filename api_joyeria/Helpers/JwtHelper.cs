using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using api_joyeria.Models;

namespace api_joyeria.Helpers
{
    public class JwtHelper
    {
        private readonly IConfiguration _config;
        public JwtHelper(IConfiguration config) => _config = config;

        public string GenerateToken(Cliente cliente)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, cliente.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, cliente.Email),
                new Claim("nombre", cliente.Nombre),
                new Claim("esAdmin", cliente.EsAdmin.ToString())//nuevo apartado para ver si es admin
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
