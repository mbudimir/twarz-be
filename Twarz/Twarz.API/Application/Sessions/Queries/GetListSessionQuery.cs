using MediatR;

namespace Twarz.API.Application.Sessions.Queries
{
    public class GetListSessionQuery:IRequest<List<SessionMv>>
    {
        public string DocumentNumber { get; set; }

        public GetListSessionQuery(string documentNumber)
        {
            DocumentNumber = documentNumber;
        }
    }
}
