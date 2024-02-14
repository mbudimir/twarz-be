using Microsoft.AspNetCore.Identity;

namespace Twarz.API.Domains
{
    public class UserCompany : IdentityUser
    {
        public Company Company { get; set; }
    }
}
