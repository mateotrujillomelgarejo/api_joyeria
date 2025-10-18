using api_joyeria.DTOs.Requests;
using api_joyeria.DTOs.Responses;

namespace api_joyeria.Data.IService
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoResponse>> GetAllAsync();
        Task<ProductoResponse?> GetByIdAsync(int id);
        Task<ProductoResponse> CreateAsync(ProductoRequest request);
        Task<ProductoResponse?> UpdateAsync(int id, ProductoRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
