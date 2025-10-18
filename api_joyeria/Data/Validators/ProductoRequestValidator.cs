using FluentValidation;
using api_joyeria.DTOs.Requests;

namespace api_joyeria.Data.Validators
{
    public class ProductoRequestValidator : AbstractValidator<ProductoRequest>
    {
        public ProductoRequestValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres");

            RuleFor(x => x.Precio)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo");

            RuleFor(x => x.ImagenUrl)
                .NotEmpty().WithMessage("La imagen del producto es importante");

            RuleFor(x => x.Descripcion)
                .MaximumLength(300).WithMessage("La descripción no puede superar los 300 caracteres");
        }
    }
}
