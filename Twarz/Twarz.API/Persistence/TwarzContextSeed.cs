using Twarz.API.Domains;

namespace Twarz.API.Persistence
{
    public class TwarzContextSeed
    {
        public static async Task SeedAsync(TwarzContext twarzContext, ILogger<TwarzContextSeed> logger)
        {
            if (!twarzContext.Session.Any())
            {
                // twarzContext.Session.AddRange(GetPreconfiguredTwarz());
                await twarzContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(TwarzContext).Name);
            }
        }

    }
}
