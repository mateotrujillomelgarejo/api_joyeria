using FluentValidation;
using api_joyeria.DTOs.Requests;

namespace api_joyeria.Data.Validators
{
    public class PedidoRequestValidator : AbstractValidator<PedidoRequest>
    {
        public PedidoRequestValidator()
        {
            RuleFor(x => x.ClienteId)
                .GreaterThan(0).WithMessage("Debe especificar un cliente válido");

            RuleFor(x => x.ProductosIds)
                .NotEmpty().WithMessage("Debe agregar al menos un producto al pedido");

            RuleFor(x => x.Cantidades)
                .NotEmpty().WithMessage("Debe indicar las cantidades")
                .Must((req, cantidades) => cantidades.Count == req.ProductosIds.Count)
                .WithMessage("Cada producto debe tener una cantidad asociada");

            RuleForEach(x => x.Cantidades)
                .GreaterThan(0).WithMessage("Las cantidades deben ser mayores que 0");
        }
    }
}
