using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Connections;
using Twarz.API.Contracts;
using Twarz.API.Domains;

namespace Twarz.API.Application.Sessions.Commands
{
    public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, string>
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateSessionCommandHandler> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public CreateSessionCommandHandler(ISessionRepository sessionRepository, 
            IMapper mapper, ILogger<CreateSessionCommandHandler> logger, UserManager<User> userManager,
            IConfiguration configuration)
        {
            _sessionRepository = sessionRepository;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<string> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
        {
            
            var sessionEntity = _mapper.Map<Session>(request);
            var newSession = await _sessionRepository.AddAsync(sessionEntity);
            _logger.LogInformation($"Session {newSession.Id} is successfully created.");

            var existUser = await _userManager.FindByNameAsync(GetUserNameBySession(newSession));
            if (existUser != null)
            {
                // Por el momento luego asociaremos el dispositivo.
                throw new AddressInUseException("The username already register.");
            }

            var user = new User { UserName = GetUserNameBySession(newSession), Session = newSession, Code = request.GivenName};

            var password = "31Twarz*";
            var result = await _userManager.CreateAsync((User)user, password);
            if (result.Succeeded)
            {
                _logger.LogInformation($"User {user.Id} is successfully created.");

                return GenerateToken(user);
            }
            _logger.LogInformation($"Session doenst created.");

            return "Invalid Token.";

        }

        private string GetUserNameBySession(Session session)
        {
            return session.Id + session.DocumentNumber;
        }

        private string GenerateToken(User user)
        {
            var key = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Session.DocumentNumber),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Surname, user.Session.Surname),
                new Claim(ClaimTypes.Country, user.Session.Nationality),
                new Claim(ClaimTypes.DateOfBirth, user.Session.DateOfBirth.ToString()),
                new Claim(ClaimTypes.Gender, user.Session.Sex),
                new Claim(ClaimTypes.SerialNumber, user.Session.Id.ToString()),
                new Claim(ClaimTypes.GivenName, user.Session.GivenName),
            };
            var token = new JwtSecurityToken(issuer,
                audience,
                claims,
                expires: DateTime.Now.AddYears(1),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
