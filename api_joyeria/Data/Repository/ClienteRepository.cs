using api_joyeria.Models;
using api_joyeria.Data.IRepository;
using Microsoft.EntityFrameworkCore;

namespace api_joyeria.Data.Repository
{
    public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(JoyeriaDbContext context) : base(context) { }

        public async Task<Cliente?> GetByCorreoAsync(string correo)
            => await _dbSet.FirstOrDefaultAsync(c => c.Email == correo);
    }
}
