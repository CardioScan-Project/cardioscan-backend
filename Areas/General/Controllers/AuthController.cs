using ChatBot.API.Areas.General.Data;
using ChatBot.API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CardioScanAPI.Areas.General.Models.Auth;

namespace CardioScanAPI.Areas.General.Controllers
{
    [Route("api/general")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            var servicio = new DAuth();

            var response = await servicio.IniciarSesion(userLogin.Email, userLogin.Password);

            if (response.Codigo == ConstantHelpers.Response.Respuesta.CORRECTO)
            {
                var token = GenerateJwtToken(response.Username, response.DoctorId);
                return Ok(new { token });
            }

            return BadRequest(response.Mensaje);
        }

        private string GenerateJwtToken(string username, int doctorId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddHours(12)).ToUnixTimeSeconds().ToString()),
                new Claim("username", username),
                new Claim("doctorId", doctorId.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
