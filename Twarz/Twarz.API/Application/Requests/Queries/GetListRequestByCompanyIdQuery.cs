using MediatR;

namespace Twarz.API.Application.Requests.Queries
{
    public class GetListRequestByCompanyIdQuery : IRequest<List<RequestCompanyMv>>
    {
        public int CompanyId { get; set; }

        public GetListRequestByCompanyIdQuery(int companyId)
        {
            CompanyId = companyId;
        }
    }
}
