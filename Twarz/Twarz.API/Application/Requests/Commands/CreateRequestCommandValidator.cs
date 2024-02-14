using FluentValidation;

namespace Twarz.API.Application.Requests.Commands
{
    public class CreateRequestCommandValidator : AbstractValidator<CreateRequestCommand>
    {
        public CreateRequestCommandValidator()
        {
            RuleFor(p => p.CompanyId)
                .NotNull();
            RuleFor(p => p.SessionId)
                .NotNull();
        }
    }
}
