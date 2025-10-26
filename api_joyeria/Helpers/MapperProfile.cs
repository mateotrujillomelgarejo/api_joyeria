using AutoMapper;
using api_joyeria.DTOs.Requests;
using api_joyeria.DTOs.Responses;
using api_joyeria.Models;

namespace api_joyeria.Helpers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // Producto
            CreateMap<Producto, ProductoResponse>();
            CreateMap<ProductoRequest, Producto>();

            // Cliente
            CreateMap<Cliente, ClienteResponse>();
            CreateMap<ClienteRequest, Cliente>();

            // Pedido y DetallePedido
            CreateMap<Pedido, PedidoResponse>()
                .ForMember(dest => dest.ClienteNombre, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nombre : string.Empty))
                .ForMember(dest => dest.ClienteEmail, opt => opt.MapFrom(src => src.Cliente.Email))
                .ForMember(dest => dest.FechaPedido, opt => opt.MapFrom(src => src.FechaPedido));

            CreateMap<DetallePedido, DetallePedidoResponse>()
                .ForMember(dest => dest.NombreProducto, opt => opt.MapFrom(src => src.Producto.Nombre));
        }
    }
}
