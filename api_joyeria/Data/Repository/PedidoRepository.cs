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

        public async Task UpdateAsync(Pedido pedido)
        {
            var existingPedido = await _context.Pedidos
                .Include(p => p.Detalles)
                .FirstOrDefaultAsync(p => p.Id == pedido.Id);

            if (existingPedido == null)
                throw new KeyNotFoundException($"No se encontró el pedido con ID {pedido.Id}");

            _context.Entry(existingPedido).CurrentValues.SetValues(pedido);

            var detallesToRemove = existingPedido.Detalles
                .Where(d => !pedido.Detalles.Any(nd => nd.Id == d.Id))
                .ToList();

            foreach (var detalle in detallesToRemove)
                _context.Remove(detalle);

            foreach (var detalle in pedido.Detalles)
            {
                var existingDetalle = existingPedido.Detalles
                    .FirstOrDefault(d => d.Id == detalle.Id);

                if (existingDetalle != null)
                {
                    _context.Entry(existingDetalle).CurrentValues.SetValues(detalle);
                }
                else
                {
                    existingPedido.Detalles.Add(detalle);
                }
            }

            await _context.SaveChangesAsync();
        }
    }

}
