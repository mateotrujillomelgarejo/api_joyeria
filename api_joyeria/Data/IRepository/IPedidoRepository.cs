using api_joyeria.Models;

namespace api_joyeria.Data.IRepository
{
    public interface IPedidoRepository : IBaseRepository<Pedido> 
    {
        Task<IEnumerable<Pedido>> GetAllWithRelationsAsync();
        Task<Pedido?> GetByIdWithRelationsAsync(int id);
    }
}
