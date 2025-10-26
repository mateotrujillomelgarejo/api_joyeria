using api_joyeria.DTOs.Requests;
using api_joyeria.DTOs.Responses;

namespace api_joyeria.Data.IService
{
    public interface IPedidoService
    {
        Task<IEnumerable<PedidoResponse>> GetAllAsync();
        Task<PedidoResponse?> GetByIdAsync(int id);
        Task<PedidoResponse> CreateAsync(PedidoRequest request);
        Task<bool> CambiarEstadoAsync(int id, string estado);
    }
}
