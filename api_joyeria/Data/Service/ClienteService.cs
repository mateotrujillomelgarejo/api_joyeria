using AutoMapper;
using api_joyeria.Data.IRepository;
using api_joyeria.Data.IService;
using api_joyeria.DTOs.Requests;
using api_joyeria.DTOs.Responses;
using api_joyeria.Helpers;
using api_joyeria.Models;

namespace api_joyeria.Data.Service
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repo;
        private readonly IMapper _mapper;

        public ClienteService(IClienteRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClienteResponse>> GetAllAsync()
        {
            var clientes = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ClienteResponse>>(clientes);
        }

        public async Task<ClienteResponse?> GetByIdAsync(int id)
        {
            var cliente = await _repo.GetByIdAsync(id);
            return cliente == null ? null : _mapper.Map<ClienteResponse>(cliente);
        }

        public async Task<ClienteResponse> RegisterAsync(ClienteRequest request)
        {
            var existe = await _repo.GetByCorreoAsync(request.Email);
            if (existe != null) throw new Exception("El correo ya está registrado");

            var cliente = _mapper.Map<Cliente>(request);
            cliente.Password = PasswordHasher.HashPassword(request.Password);

            await _repo.AddAsync(cliente);
            await _repo.SaveAsync();

            return _mapper.Map<ClienteResponse>(cliente);
        }

        public async Task<ClienteResponse?> UpdateAsync(int id, ClienteRequest request)
        {
            var cliente = await _repo.GetByIdAsync(id);
            if (cliente == null) return null;

            cliente.Nombre = request.Nombre;
            cliente.Email = request.Email;

            _repo.Update(cliente);
            await _repo.SaveAsync();
            return _mapper.Map<ClienteResponse>(cliente);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cliente = await _repo.GetByIdAsync(id);
            if (cliente == null) return false;

            _repo.Delete(cliente);
            await _repo.SaveAsync();
            return true;
        }

        public async Task<ClienteResponse?> ValidateCredentialsAsync(string correo, string password)
        {
            var cliente = await _repo.GetByCorreoAsync(correo);
            if (cliente == null) return null;

            bool valido = PasswordHasher.VerifyPassword(password, cliente.Password);
            return valido ? _mapper.Map<ClienteResponse>(cliente) : null;
        }
    }
}
