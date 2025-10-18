namespace api_joyeria.DTOs.Requests
{
    public class PedidoRequest
    {
        public int ClienteId { get; set; }
        public List<int> ProductosIds { get; set; } = new();
        public List<int> Cantidades { get; set; } = new();
    }
}
