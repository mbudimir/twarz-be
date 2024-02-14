using MediatR;

namespace Twarz.API.Application.Sessions.Queries
{
    public class GetSessionbyIdQuery : IRequest<SessionMv>
    {
        public long Id { get; set; }

        public GetSessionbyIdQuery(long id)
        {
            Id = id;
        }
    }
}
