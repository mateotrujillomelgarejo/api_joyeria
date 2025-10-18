using api_joyeria.Data.IRepository;
using api_joyeria.Models;
using Microsoft.EntityFrameworkCore;

namespace api_joyeria.Data.Repository
{
    public class ProductoRepository : BaseRepository<Producto>, IProductoRepository
    {
        private new readonly JoyeriaDbContext _context;

        public ProductoRepository(JoyeriaDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateAsync(Producto producto)
        {
            var existing = await _context.Productos.FirstOrDefaultAsync(p => p.Id == producto.Id);

            if (existing == null)
                throw new Exception($"Producto con ID {producto.Id} no encontrado.");

            existing.Nombre = producto.Nombre;
            existing.Descripcion = producto.Descripcion;
            existing.Precio = producto.Precio;
            existing.Stock = producto.Stock;
            existing.ImagenUrl = producto.ImagenUrl;

            _context.Productos.Update(existing);
            await _context.SaveChangesAsync();
        }
    }
}
