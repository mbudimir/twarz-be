using MediatR;
using Twarz.API.Domains.Enums;

namespace Twarz.API.Application.Requests.Commands
{
    public class ChangeStatusRequestCommand : IRequest<int>
    {
        public int Id { get; set; }
        public RequestStatusEnum newState { get; set; }

    }
}
