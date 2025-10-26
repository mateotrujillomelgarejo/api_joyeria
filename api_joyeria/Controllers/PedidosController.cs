using Microsoft.AspNetCore.Mvc;
using api_joyeria.Data.IService;
using api_joyeria.DTOs.Requests;
using api_joyeria.DTOs.Shared;
using api_joyeria.Data.Repository;

namespace api_joyeria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _service;


        public PedidosController(IPedidoService service)
        {
            _service = service;

        }

        //aca lo cambie mira
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pedidos = await _service.GetAllAsync();
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pedido = await _service.GetByIdAsync(id);
            if (pedido == null)
                return NotFound("Pedido no encontrado");

            return Ok(pedido);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PedidoRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var nuevo = await _service.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = nuevo.Id }, nuevo);
        }

        [HttpPatch("{id}/estado")]
        async Task<IActionResult> CambiarEstado(int id, [FromBody] EstadoRequest req)
        {
            if (string.IsNullOrWhiteSpace(req?.Estado))
                return BadRequest("Estado inválido.");

            // asumo que el servicio expone CambiarEstadoAsync
            var ok = await _service.CambiarEstadoAsync(id, req.Estado);
            if (!ok) return NotFound($"Pedido {id} no encontrado o no se pudo actualizar.");
            return NoContent();
        }
    }
}