namespace api_joyeria.DTOs.Requests
{
    public class ProductoRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string ImagenUrl { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }
}
