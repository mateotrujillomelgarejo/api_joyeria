namespace api_joyeria.DTOs.Responses
{
    public class PedidoResponse
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; } = "";
        public string ClienteEmail { get; set; } = "";
        public string Estado { get; set; } = "Pendiente";
        public string? DireccionEnvio { get; set; }
        public string? Observaciones { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaPedido { get; set; }
        public List<DetallePedidoResponse> Detalles { get; set; } = new();
    }
}
