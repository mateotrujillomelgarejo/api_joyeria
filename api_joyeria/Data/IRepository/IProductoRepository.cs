using api_joyeria.Models;

namespace api_joyeria.Data.IRepository
{
    public interface IProductoRepository : IBaseRepository<Producto>
    {
        Task UpdateAsync(Producto producto);
    }
}
