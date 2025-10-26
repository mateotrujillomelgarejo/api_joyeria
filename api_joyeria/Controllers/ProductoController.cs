using Microsoft.AspNetCore.Mvc;
using api_joyeria.Data.IService;
using api_joyeria.DTOs.Requests;

namespace api_joyeria.Controllers
{
    [ApiController]
    [Route("api/productos")]
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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null) return NotFound(new { Success = false, Message = "Producto no encontrado" });
            return Ok(new { Success = true, Data = data });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductoRequest request)
        {
            var data = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = data.Id }, new { Success = true, Data = data });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductoRequest request)
        {
            var data = await _service.UpdateAsync(id, request);
            if (data == null) return NotFound(new { Success = false, Message = "Producto no encontrado" });
            return Ok(new { Success = true, Data = data });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound(new { Success = false, Message = "Producto no encontrado" });
            return Ok(new { Success = true, Message = "Producto eliminado correctamente" });
        }
    }
}
