using FluentValidation;

namespace Twarz.API.Application.Requests.Commands
{
    public class ChangeStatusRequestCommandValidator : AbstractValidator<ChangeStatusRequestCommand>
    {
        public ChangeStatusRequestCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotNull();
            RuleFor(p => p.newState)
                .NotNull();
        }
    }
}
