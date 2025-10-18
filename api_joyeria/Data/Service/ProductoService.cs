using api_joyeria.Data.IRepository;
using api_joyeria.Data.IService;
using api_joyeria.DTOs.Requests;
using api_joyeria.DTOs.Responses;
using api_joyeria.Models;
using AutoMapper;

namespace api_joyeria.Data.Service
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repo;
        private readonly IMapper _mapper;

        public ProductoService(IProductoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductoResponse>> GetAllAsync()
        {
            var productos = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductoResponse>>(productos);
        }

        public async Task<ProductoResponse?> GetByIdAsync(int id)
        {
            var producto = await _repo.GetByIdAsync(id);
            return producto == null ? null : _mapper.Map<ProductoResponse>(producto);
        }

        public async Task<ProductoResponse> CreateAsync(ProductoRequest request)
        {
            var producto = _mapper.Map<Producto>(request);
            await _repo.AddAsync(producto);
            await _repo.SaveAsync();
            return _mapper.Map<ProductoResponse>(producto);
        }

        public async Task<ProductoResponse?> UpdateAsync(int id, ProductoRequest request)
        {
            var producto = await _repo.GetByIdAsync(id);
            if (producto == null) return null;

            _mapper.Map(request, producto);
            _repo.Update(producto);
            await _repo.SaveAsync();
            return _mapper.Map<ProductoResponse>(producto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var producto = await _repo.GetByIdAsync(id);
            if (producto == null) return false;

            _repo.Delete(producto);
            await _repo.SaveAsync();
            return true;
        }
    }
}
