using Microsoft.EntityFrameworkCore;
using Twarz.API.Contracts;
using Twarz.API.Domains;
using Twarz.API.Persistence;

namespace Twarz.API.Repositories
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(TwarzContext dbContext) : base(dbContext)
        {
        }

        public async Task<Company> GetRequestByUser(string? userName)
        {
            var user = await _dbContext.Users
                .Include(x => x.Company)
                .Where(o => o.UserName == userName)
                .FirstOrDefaultAsync();
            return user?.Company;
        }
    }
}
