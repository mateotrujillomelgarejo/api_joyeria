using api_joyeria.Data.IService;
using api_joyeria.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _service.GetByIdAsync(id);
            if (cliente == null)
                return NotFound("Cliente no encontrado");

            return Ok(cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClienteRequest request)
        {
            var actualizado = await _service.UpdateAsync(id, request);
            if (actualizado == null)
                return NotFound("Cliente no encontrado");

            return Ok(actualizado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok)
                return NotFound("Cliente no encontrado");

            return Ok("Cliente eliminado correctamente");
        }
    }
}
