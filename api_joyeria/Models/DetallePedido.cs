using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_joyeria.Models
{
    [Table("DetallesPedido")]
    public class DetallePedido
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Pedido")]
        public int PedidoId { get; set; }

        [ForeignKey("Producto")]
        public int ProductoId { get; set; }

        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioUnitario { get; set; }

        public Pedido? Pedido { get; set; }
        public Producto? Producto { get; set; }
    }
}
