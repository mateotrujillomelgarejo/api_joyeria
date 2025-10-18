using api_joyeria.Models;
using api_joyeria.Data.IRepository;

namespace api_joyeria.Data.Repository
{
    public class DetallePedidoRepository : BaseRepository<DetallePedido>, IDetallePedidoRepository
    {
        public DetallePedidoRepository(JoyeriaDbContext context) : base(context) { }
    }
}
