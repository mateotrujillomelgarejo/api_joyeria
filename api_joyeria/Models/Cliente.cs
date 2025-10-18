using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_joyeria.Models
{
    [Table("Clientes")]
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string Password { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Direccion { get; set; }

        public bool EsAdmin { get; set; } = false;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Relaciones
        public ICollection<Pedido>? Pedidos { get; set; }
    }
}
