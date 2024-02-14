using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Twarz.API.Domains;

namespace Twarz.API.Persistence
{
    public class TwarzContext: IdentityDbContext<User>
    {
        public TwarzContext(DbContextOptions<TwarzContext> options):base(options) { }

        public DbSet<Session> Session { get; set; }
        public DbSet<Request> Request { get; set; }
        public DbSet<Company> Company { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = "Twarz";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = "Twarz";
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
