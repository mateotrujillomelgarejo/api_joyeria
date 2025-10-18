using Microsoft.AspNetCore.Mvc;
using api_joyeria.Data.IService;
using api_joyeria.Models;

namespace api_joyeria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallePedidoController : ControllerBase
    {
        private readonly IDetallePedidoService _repo;
        public DetallePedidoController(IDetallePedidoService repo) => _repo = repo;

        [HttpGet("pedido/{pedidoId:int}")]
        public async Task<IActionResult> GetByPedido(int pedidoId)
        {
            var list = await _repo.GetByPedidoIdAsync(pedidoId);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DetallePedido detalle)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var creado = await _repo.CreateAsync(detalle);
            return CreatedAtAction(nameof(GetByPedido), new { pedidoId = creado.PedidoId }, creado);
        }
    }
}
