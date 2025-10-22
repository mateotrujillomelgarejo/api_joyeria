using api_joyeria.Data.IService;
using api_joyeria.DTOs.Requests;
using api_joyeria.DTOs.Shared;
using api_joyeria.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace api_joyeria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly JwtHelper _jwtHelper;

        public AuthController(IClienteService clienteService, JwtHelper jwtHelper)
        {
            _clienteService = clienteService;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var cliente = await _clienteService.ValidateCredentialsAsync(request.Email, request.Password);
            if (cliente == null)
                return Unauthorized("Credenciales inválidas");

            var token = _jwtHelper.GenerateToken(new Models.Cliente
            {
                Id = cliente.Id,
                Email = cliente.Email,
                Nombre = cliente.Nombre
            });

            return Ok(new{token, cliente});
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ClienteRequest request)
        {
            try
            {
                var nuevo = await _clienteService.RegisterAsync(request);
                return Ok(nuevo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
