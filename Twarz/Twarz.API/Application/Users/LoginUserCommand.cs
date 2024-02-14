using MediatR;
using System.IdentityModel.Tokens.Jwt;

namespace Twarz.API.Application.Users
{
    public class LoginUserCommand : IRequest<JwtSecurityToken?>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
