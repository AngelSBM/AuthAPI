using Auth.LogicLayer.DTOs;
using FluentValidation;

namespace Auth.ClientLayer.Helpers.Validators
{
    public class LoginDTOValidator : AbstractValidator<UserLoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Correo inválido");
                
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Contraseña requerida");
        }
    }
}
