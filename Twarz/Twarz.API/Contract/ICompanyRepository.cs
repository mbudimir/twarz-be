using Twarz.API.Domains;

namespace Twarz.API.Contracts
{
    public interface ICompanyRepository : IAsyncRepository<Company>
    {
        Task<Company> GetRequestByUser(string? userName);
    }
}
