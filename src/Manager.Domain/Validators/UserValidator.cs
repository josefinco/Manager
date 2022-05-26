using FluentValidation;
using Manager.Domain.Entities;

namespace Manager.Domain.Validators
{
    public class UserValidator : AbstractValidator<User>
    {

        public UserValidator()
        {
            RuleFor(x => x)
                .NotEmpty()
                .WithMessage("A entidade não pode ser vazia.")

                .NotNull()
                .WithMessage("A entidade não pode ser nula.");

            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage("O nome não pode ser nulo.")

                .NotEmpty()
                .WithMessage("O nome não pode ser vazio.")

                .MinimumLength(3)
                .WithMessage("O nome deve ter no mínimo 3 caracteres.")

                .MaximumLength(80)
                .WithMessage("O nome deve ter no máximo 80 caracteres.");

            RuleFor(x => x.Email)
                .NotNull()
                .WithMessage("O email não pode ser nulo.")

                .NotEmpty()
                .WithMessage("O email não pode ser vazio.")

                .MaximumLength(280)
                .WithMessage("O email deve ter no máximo 280 caracteres.")

                .MinimumLength(10)
                .WithMessage("O email deve ter no mínimo 10 caracteres.")

                .Matches(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$")
                .WithMessage("O email deve ser um email válido.");

            RuleFor(x => x.Password)
                .NotNull()
                .WithMessage("A senha não pode ser nula.")

                .NotEmpty()
                .WithMessage("A senha não pode ser vazia.")

                .MinimumLength(6)
                .WithMessage("A senha deve ter no mínimo 6 caracteres.")

                .MaximumLength(30)
                .WithMessage("A senha deve ter no máximo 30 caracteres.");

        }
    } 
}
