using api_joyeria.DTOs.Requests;
using api_joyeria.DTOs.Responses;

namespace api_joyeria.Data.IService
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteResponse>> GetAllAsync();
        Task<ClienteResponse?> GetByIdAsync(int id);
        Task<ClienteResponse> RegisterAsync(ClienteRequest request);
        Task<ClienteResponse?> UpdateAsync(int id, ClienteRequest request);
        Task<bool> DeleteAsync(int id);
        Task<ClienteResponse?> ValidateCredentialsAsync(string correo, string password);
    }
}
