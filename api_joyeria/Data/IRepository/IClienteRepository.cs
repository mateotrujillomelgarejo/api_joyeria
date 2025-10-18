using api_joyeria.Models;

namespace api_joyeria.Data.IRepository
{
    public interface IClienteRepository : IBaseRepository<Cliente>
    {
        Task<Cliente?> GetByCorreoAsync(string correo);
    }
}
