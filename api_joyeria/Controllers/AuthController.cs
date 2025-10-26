using api_joyeria.Data.IService;
using api_joyeria.DTOs.Requests;
using api_joyeria.DTOs.Responses; // si lo tienes, o crea ApiResponse / LoginResponse en API
using api_joyeria.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace api_joyeria.Controllers
{
    [ApiController]
    [Route("api/clientes/")]
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
            var clienteResp = await _clienteService.ValidateCredentialsAsync(request.Email, request.Password);
            if (clienteResp == null)
                return Unauthorized(new { message = "Credenciales inválidas" });

            var clienteParaToken = new api_joyeria.Models.Cliente
            {
                Id = clienteResp.Id,
                Email = clienteResp.Email,
                Nombre = clienteResp.Nombre,
                EsAdmin = clienteResp.EsAdmin
            };

            var token = _jwtHelper.GenerateToken(clienteParaToken);

            var loginResponse = new
            {
                Token = token,
                Cliente = clienteResp
            };

            var wrapper = new
            {
                Success = true,
                Message = "Login exitoso",
                Data = loginResponse
            };

            return Ok(wrapper);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ClienteRequest request)
        {
            try
            {
                var nuevo = await _clienteService.RegisterAsync(request);
                return Ok(new { Success = true, Data = nuevo });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }
    }
}
