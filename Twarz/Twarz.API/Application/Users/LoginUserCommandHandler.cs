using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Twarz.API.Application.Company.Commands;
using Twarz.API.Contracts;
using Twarz.API.Domains;

namespace Twarz.API.Application.Users
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, JwtSecurityToken?>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<LoginUserCommandHandler> _logger;
        private readonly IConfiguration _configuration;
        private readonly ICompanyRepository _companyRepository;

        public LoginUserCommandHandler(UserManager<User> userManager, ILogger<LoginUserCommandHandler> logger
            , IConfiguration configuration, ICompanyRepository companyRepository)
        {
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
            _companyRepository = companyRepository;
        }

        public async Task<JwtSecurityToken?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {
                var company = await _companyRepository.GetRequestByUser(user.UserName);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("CompanyId", company.Id.ToString()),
                    new Claim("CompanyName", company.Name)
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddYears(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
                return token;
            }

            return null;
        }
    }
}
