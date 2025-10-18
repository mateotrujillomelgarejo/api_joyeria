using Microsoft.AspNetCore.Mvc;
using api_joyeria.Data.IService;
using api_joyeria.DTOs.Requests;
using api_joyeria.DTOs.Shared;

namespace api_joyeria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _service;

        public PedidoController(IPedidoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pedidos = await _service.GetAllAsync();
            return Ok(new ApiResponse<object> { Success = true, Data = pedidos });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pedido = await _service.GetByIdAsync(id);
            if (pedido == null)
                return NotFound(new ApiResponse<string> { Success = false, Message = "Pedido no encontrado" });

            return Ok(new ApiResponse<object> { Success = true, Data = pedido });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PedidoRequest request)
        {
            try
            {
                var nuevo = await _service.CreateAsync(request);
                return Ok(new ApiResponse<object> { Success = true, Data = nuevo });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string> { Success = false, Message = ex.Message });
            }
        }
    }
}
