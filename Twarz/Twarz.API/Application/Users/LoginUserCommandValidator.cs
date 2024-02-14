using FluentValidation;
using Twarz.API.Application.Company.Commands;

namespace Twarz.API.Application.Users
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(p => p.UserName)
                .NotNull();
            RuleFor(p => p.Password)
                .NotNull();
        }
    }
}
