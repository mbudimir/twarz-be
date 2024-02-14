using Twarz.API.Domains;

namespace Twarz.API.Contracts
{
    public interface ISessionRepository : IAsyncRepository<Session>
    {
        Task<IEnumerable<Session>> GetSessionsByDocumentNumber(string documentNumber);
        Task<Session> GetById(long id);
    }
}
