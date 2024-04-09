using Microsoft.EntityFrameworkCore;
using Twarz.API.Contracts;
using Twarz.API.Domains;
using Twarz.API.Persistence;

namespace Twarz.API.Repositories
{
    public class RequestRepository : RepositoryBase<Request>, IRequestRepository
    {
        public RequestRepository(TwarzContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Request>> GetRequestsByDocumentNumber(string documentNumber)
        {
            var requestList = await _dbContext.Request
                //.Include(x => x.Session)
                .Include(x=> x.Company)
                .Where(o => o.Session.DocumentNumber == documentNumber || o.Session.PersonalNumber == documentNumber)
                .OrderByDescending(o => o.CreatedDate)
                .ToListAsync();
            return requestList;
        }

        public async Task<IEnumerable<Request>> GetRequestsByCompanyId(int companyId)
        {
            var requestList = await _dbContext.Request
                .Include(x => x.Session)
                .Include(x => x.Company)
                .Where(o => o.Company.Id == companyId)
                .OrderByDescending(x => x.RequestDate)
                .ToListAsync();
            return requestList;
        }
    }
}
