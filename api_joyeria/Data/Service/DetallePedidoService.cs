using api_joyeria.Data.IRepository;
using api_joyeria.Data.IService;
using api_joyeria.Models;

namespace api_joyeria.Data.Service
{
    public class DetallePedidoService : IDetallePedidoService
    {
        private readonly IDetallePedidoRepository _detalleRepo;

        public DetallePedidoService(IDetallePedidoRepository detalleRepo)
        {
            _detalleRepo = detalleRepo;
        }

        public async Task<IEnumerable<DetallePedido>> GetByPedidoIdAsync(int pedidoId)
        {
            var all = await _detalleRepo.GetAllAsync();
            return all.Where(d => d.PedidoId == pedidoId);
        }

        public async Task<DetallePedido> CreateAsync(DetallePedido detalle)
        {
            await _detalleRepo.AddAsync(detalle);
            await _detalleRepo.SaveAsync();
            return detalle;
        }
    }
}
