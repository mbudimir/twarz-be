using MediatR;

namespace Twarz.API.Application.Requests.Queries
{
    public class GetListRequestQuery : IRequest<List<RequestMv>>
    {
        public string DocumentNumber { get; set; }

        public GetListRequestQuery(string documentNumber)
        {
            DocumentNumber = documentNumber;
        }
    }
}
