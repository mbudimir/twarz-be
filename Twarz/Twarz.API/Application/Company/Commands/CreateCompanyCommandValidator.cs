using FluentValidation;

namespace Twarz.API.Application.Company.Commands
{
    public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
    {
        public CreateCompanyCommandValidator()
        {
            RuleFor(p => p.Username)
                .NotNull();
            RuleFor(p => p.Password)
                .NotNull();
            RuleFor(p => p.Name)
                .NotNull();
        }
    }
}
