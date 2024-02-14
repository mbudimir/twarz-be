using Twarz.API.Domains;

namespace Twarz.API.Contracts
{
    public interface IRequestRepository : IAsyncRepository<Request>
    {
        Task<IEnumerable<Request>> GetRequestsByDocumentNumber(string documentNumber);
        Task<IEnumerable<Request>> GetRequestsByCompanyId(int companyId);
    }
}
