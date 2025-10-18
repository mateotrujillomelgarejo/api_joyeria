using Microsoft.AspNetCore.Mvc;
using api_joyeria.Data.IService;
using api_joyeria.DTOs.Requests;
using api_joyeria.DTOs.Shared;

namespace api_joyeria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _service;

        public ClienteController(IClienteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _service.GetAllAsync();
            return Ok(new ApiResponse<object> { Success = true, Data = clientes });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _service.GetByIdAsync(id);
            if (cliente == null)
                return NotFound(new ApiResponse<string> { Success = false, Message = "Cliente no encontrado" });

            return Ok(new ApiResponse<object> { Success = true, Data = cliente });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClienteRequest request)
        {
            var actualizado = await _service.UpdateAsync(id, request);
            if (actualizado == null)
                return NotFound(new ApiResponse<string> { Success = false, Message = "Cliente no encontrado" });

            return Ok(new ApiResponse<object> { Success = true, Data = actualizado });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok)
                return NotFound(new ApiResponse<string> { Success = false, Message = "Cliente no encontrado" });

            return Ok(new ApiResponse<string> { Success = true, Message = "Cliente eliminado correctamente" });
        }
    }
}
