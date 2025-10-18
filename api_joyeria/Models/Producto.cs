using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_joyeria.Models
{
    [Table("Productos")]
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        [Required]
        public int Stock { get; set; }

        [MaxLength(255)]
        public string? ImagenUrl { get; set; }

        public bool Disponible { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public ICollection<DetallePedido>? DetallesPedido { get; set; }
    }
}
