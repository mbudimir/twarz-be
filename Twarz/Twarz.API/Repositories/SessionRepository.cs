using Microsoft.EntityFrameworkCore;
using Twarz.API.Contracts;
using Twarz.API.Domains;
using Twarz.API.Persistence;

namespace Twarz.API.Repositories
{
    public class SessionRepository : RepositoryBase<Session>,ISessionRepository
    {
        public SessionRepository(TwarzContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Session>> GetSessionsByDocumentNumber(string documentNumber)
        {
            var sessionList = await _dbContext.Session
                .Where(o => o.DocumentNumber == documentNumber)
                .OrderByDescending(o => o.CreatedDate)
                .ToListAsync(); 
            return sessionList;
        }

        public async Task<Session> GetById(long id)
        {
            var session = await _dbContext.Session
                .Where(o => o.Id == id).FirstOrDefaultAsync();
            return session;
        }
    }
}
