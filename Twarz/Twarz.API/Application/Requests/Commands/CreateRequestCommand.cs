using MediatR;

namespace Twarz.API.Application.Requests.Commands
{
    public class CreateRequestCommand : IRequest<int>
    {
        public int CompanyId { get; set; }
        public long SessionId { get; set; }

    }
}
