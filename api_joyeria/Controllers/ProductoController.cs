using Microsoft.AspNetCore.Mvc;
using api_joyeria.Data.IService;
using api_joyeria.DTOs.Requests;
using api_joyeria.DTOs.Shared;

namespace api_joyeria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _service;

        public ProductoController(IProductoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null) return NotFound("Producto no encontrado");
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductoRequest request)
        {
            var data = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = data.Id }, data );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductoRequest request)
        {
            var data = await _service.UpdateAsync(id, request);
            if (data == null) return NotFound("Producto no encontrado");
            return Ok(data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound("Producto no encontrado");
            return Ok("Producto eliminado correctamente");
        }
    }
}
