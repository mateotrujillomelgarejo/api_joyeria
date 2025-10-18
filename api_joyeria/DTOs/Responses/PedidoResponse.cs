namespace api_joyeria.DTOs.Responses
{
    public class PedidoResponse
    {
        public int Id { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
    }
}
