using api_joyeria.Models;

namespace api_joyeria.Data.IService
{
    public interface IDetallePedidoService
    {
        Task<IEnumerable<DetallePedido>> GetByPedidoIdAsync(int pedidoId);
        Task<DetallePedido> CreateAsync(DetallePedido detalle);
    }
}