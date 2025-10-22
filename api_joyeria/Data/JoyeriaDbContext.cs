using api_joyeria.Models;
using Microsoft.EntityFrameworkCore;
using System;

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

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.ClienteId);

            modelBuilder.Entity<DetallePedido>()
                .HasOne(dp => dp.Pedido)
                .WithMany(p => p.Detalles)
                .HasForeignKey(dp => dp.PedidoId);

            modelBuilder.Entity<DetallePedido>()
                .HasOne(dp => dp.Producto)
                .WithMany(p => p.DetallesPedido)
                .HasForeignKey(dp => dp.ProductoId);

            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Total)
                .HasPrecision(10, 2);

            modelBuilder.Entity<DetallePedido>()
                .Property(dp => dp.PrecioUnitario)
                .HasPrecision(10, 2);

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Cliente administrador
            modelBuilder.Entity<Cliente>().HasData(
                new Cliente
                {
                    Id = 1,
                    Nombre = "Administrador",
                    Email = "admin@joyeria.com",
                    Password = "admin123",
                    Direccion = "Oficina Principal",
                    EsAdmin = true,
                    FechaRegistro = new DateTime(2025, 1, 1)
                }
            );

            // Productos iniciales
            modelBuilder.Entity<Producto>().HasData(
                new Producto
                {
                    Id = 1,
                    Nombre = "Anillo de Oro 18k",
                    Descripcion = "Elegante anillo de oro de 18 quilates con acabado brillante",
                    Precio = 850.00m,
                    Stock = 15,
                    ImagenUrl = "https://cdn-media.glamira.com/media/product/newgeneration/view/1/sku/GWD210000/alloycolour/yellow/width/w4/profile/prA/surface/polished.jpg",
                    Disponible = true,
                    FechaCreacion = new DateTime(2025, 1, 1)
                },
                new Producto
                {
                    Id = 2,
                    Nombre = "Collar de Plata",
                    Descripcion = "Hermoso collar de plata 925 con colgante de corazón",
                    Precio = 320.00m,
                    Stock = 25,
                    ImagenUrl = "https://baliq.com/wp-content/uploads/2023/12/DIS-2982-y-CAS-0864.jpg",
                    Disponible = true,
                    FechaCreacion = new DateTime(2025, 1, 1)
                },
                new Producto
                {
                    Id = 3,
                    Nombre = "Aretes de Diamante",
                    Descripcion = "Exclusivos aretes con diamantes naturales",
                    Precio = 1200.00m,
                    Stock = 8,
                    ImagenUrl = "https://cdn-media.glamira.com/media/product/newgeneration/view/1/sku/G100735/diamond/lab-grown-diamond_AAA/alloycolour/white.jpg",
                    Disponible = true,
                    FechaCreacion = new DateTime(2025, 1, 1)
                }
            );
        }
    }
}
