using api_joyeria.Models;
using Microsoft.EntityFrameworkCore;

namespace api_joyeria.Data
{
    public class JoyeriaDbContext : DbContext
    {
        public JoyeriaDbContext(DbContextOptions<JoyeriaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallesPedido { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Evitar pluralización automática (opcional)
            modelBuilder.Entity<Cliente>().ToTable("Clientes");
            modelBuilder.Entity<Producto>().ToTable("Productos");
            modelBuilder.Entity<Pedido>().ToTable("Pedidos");
            modelBuilder.Entity<DetallePedido>().ToTable("DetallesPedido");
        }
    }
}
