using FluentValidation;

namespace Twarz.API.Application.Sessions.Commands
{
    public class CreateSessionCommandValidator : AbstractValidator<CreateSessionCommand>
    {
        public CreateSessionCommandValidator()
        {
            RuleFor(p => p.Surname)
                .NotNull();
            RuleFor(p => p.GivenName)
                .NotNull();
            RuleFor(p => p.DocumentNumber)
                .NotNull();
        }
    }
}
