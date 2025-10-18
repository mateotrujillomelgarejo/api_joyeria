using api_joyeria.Data.IRepository;
using api_joyeria.Models;
using Microsoft.EntityFrameworkCore;

namespace api_joyeria.Data.Repository
{
    public class PedidoRepository : BaseRepository<Pedido>, IPedidoRepository
    {
        public PedidoRepository(JoyeriaDbContext context) : base(context) { }

        public async Task<IEnumerable<Pedido>> GetAllWithRelationsAsync()
        {
            return await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .ToListAsync();
        }

        public async Task<Pedido?> GetByIdWithRelationsAsync(int id)
        {
            return await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }

}
