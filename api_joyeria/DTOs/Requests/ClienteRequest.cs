namespace api_joyeria.DTOs.Requests
{
    public class ClienteRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Direccion { get; set; }
    }
}