using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_joyeria.Models
{
    [Table("Pedidos")]
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }

        public DateTime FechaPedido { get; set; } = DateTime.Now;

        [MaxLength(50)]
        public string Estado { get; set; } = "Pendiente";

        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        [MaxLength(200)]
        public string? DireccionEnvio { get; set; }

        [MaxLength(500)]
        public string? Observaciones { get; set; }

        public Cliente? Cliente { get; set; }
        public ICollection<DetallePedido>? Detalles { get; set; }
    }
}
