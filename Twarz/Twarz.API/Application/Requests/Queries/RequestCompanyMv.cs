using Twarz.API.Domains.Enums;
using Twarz.API.Application.Sessions.Queries;
using Twarz.API.Application.Company;

namespace Twarz.API.Application.Requests.Queries
{
    public class RequestMv
    {
        public int Id { get; set; }
        public CompanyMv Company { get; set; }
        public DateTime RequestDate { get; set; }
        // public SessionMv Session { get; set; }
        public DateTime? SessionDate { get; set; }
        public RequestStatusEnum Status { get; set; } = RequestStatusEnum.Pending;
    }
}
