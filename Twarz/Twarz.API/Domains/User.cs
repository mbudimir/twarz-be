using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Twarz.API.Domains
{
    public class User : IdentityUser
    {
        public string? Code { get; set; }
        public virtual Session? Session { get; set; }
        public virtual Company? Company { get; set; }
    }
}
