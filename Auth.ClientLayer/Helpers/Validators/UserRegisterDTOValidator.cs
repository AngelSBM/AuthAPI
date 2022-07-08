using Auth.ClientLayer.Helpers.Utilities;
using Auth.LogicLayer.DTOs;
using FluentValidation;

namespace Auth.ClientLayer.Helpers.Validators
{
    public class UserRegisterDTOValidator : AbstractValidator<UserRegisterDTO>
    {
        public UserRegisterDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Nombre requerido.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Apellido requerido.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Correo inválido."); ;

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Contraseña requerida.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefono requerido.");

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .Must(AttributeValidator.isValidDate)
                .WithMessage("Fecha inválida.");
                
        }
    }
}
