using AutoMapper;
using api_joyeria.Data.IRepository;
using api_joyeria.Data.IService;
using api_joyeria.DTOs.Requests;
using api_joyeria.DTOs.Responses;
using api_joyeria.Models;

namespace api_joyeria.Data.Service
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepo;
        private readonly IProductoRepository _productoRepo;
        private readonly IClienteRepository _clienteRepo;
        private readonly IMapper _mapper;

        public PedidoService(IPedidoRepository pedidoRepo, IProductoRepository productoRepo, IClienteRepository clienteRepo, IMapper mapper)
        {
            _pedidoRepo = pedidoRepo;
            _productoRepo = productoRepo;
            _clienteRepo = clienteRepo;
            _mapper = mapper;
        }

        public async Task<PedidoResponse> CreateAsync(PedidoRequest request)
        {
            if (request.ProductosIds.Count != request.Cantidades.Count)
                throw new Exception("El número de productos no coincide con el número de cantidades.");

            var cliente = await _clienteRepo.GetByIdAsync(request.ClienteId);
            if (cliente == null)
                throw new Exception($"Cliente con ID {request.ClienteId} no encontrado.");

            decimal total = 0;
            var detalles = new List<DetallePedido>();

            for (int i = 0; i < request.ProductosIds.Count; i++)
            {
                var producto = await _productoRepo.GetByIdAsync(request.ProductosIds[i]);
                if (producto == null)
                    throw new Exception($"Producto con ID {request.ProductosIds[i]} no encontrado.");

                if (producto.Stock < request.Cantidades[i])
                    throw new Exception($"Stock insuficiente para el producto {producto.Nombre}.");

                decimal precioUnitario = producto.Precio;
                decimal subtotal = precioUnitario * request.Cantidades[i];
                total += subtotal;

                detalles.Add(new DetallePedido
                {
                    ProductoId = producto.Id,
                    Cantidad = request.Cantidades[i],
                    PrecioUnitario = precioUnitario
                });

                producto.Stock -= request.Cantidades[i];
                await _productoRepo.UpdateAsync(producto);
            }

            var pedido = new Pedido
            {
                ClienteId = request.ClienteId,
                Cliente = cliente,
                FechaPedido = DateTime.Now,
                Total = total,
                Detalles = detalles
            };

            await _pedidoRepo.AddAsync(pedido);
            await _pedidoRepo.SaveAsync();

            var response = _mapper.Map<PedidoResponse>(pedido);
            return response;
        }

        public async Task<IEnumerable<PedidoResponse>> GetAllAsync()
        {
            var pedidos = await _pedidoRepo.GetAllWithRelationsAsync();
            return _mapper.Map<IEnumerable<PedidoResponse>>(pedidos);
        }

        public async Task<PedidoResponse?> GetByIdAsync(int id)
        {
            var pedido = await _pedidoRepo.GetByIdWithRelationsAsync(id);
            return pedido == null ? null : _mapper.Map<PedidoResponse>(pedido);
        }

    }
}
