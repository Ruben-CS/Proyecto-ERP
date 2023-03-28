using System.Data;
using FluentValidation;
using Modelos.Models.Dtos;

namespace BlazorFrontend.Validators;

public class EmpresaValidators : AbstractValidator<EmpresaDto>
{
    public EmpresaValidators()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre de la empresa es requerido")
            .MaximumLength(50)
            .WithMessage("El nombre de la empresa no puede tener más de 50 caracteres");
        RuleFor(x => x.Nit).NotEmpty()
            .WithMessage("El NIT de la empresa es requerido");
        RuleFor(x => x.Sigla).NotEmpty()
            .WithMessage("La sigla de la empresa es requerida");
        RuleFor(x => x.Correo).EmailAddress()
            .WithMessage("El correo de la empresa no es válido");
        RuleFor(x => x.Niveles).NotEmpty();
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue =>
        async (model, propertyName) =>
        {
            var result = await ValidateAsync(
                ValidationContext<EmpresaDto>.CreateWithOptions(
                    (EmpresaDto)model, x => x.IncludeProperties(propertyName)));
            return result.IsValid
                ? Array.Empty<string>()
                : result.Errors.Select(e => e.ErrorMessage);
        };
}