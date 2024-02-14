using System.Data;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Identity;
using Twarz.API.Contracts;
using Twarz.API.Domains;

namespace Twarz.API.Application.Company.Commands
{
    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, int>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCompanyCommandHandler> _logger;
        private readonly UserManager<User> _userManager;

        public CreateCompanyCommandHandler(ICompanyRepository companyRepository, 
            IMapper mapper, ILogger<CreateCompanyCommandHandler> logger, UserManager<User> userManager)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }
        public async Task<int> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var companyEntity = _mapper.Map<Domains.Company>(request);
            var newCompany = await _companyRepository.AddAsync(companyEntity);
            _logger.LogInformation($"Company {newCompany.Id} is successfully created.");

            var existUser = await _userManager.FindByNameAsync(request.Username);
            if (existUser != null)
            {
                // Por el momento una compañia por login.
                throw new DuplicateNameException("The company already register.");
            }

            var user = new User 
            { 
                UserName = request.Username,
                Company = newCompany
            };

            var password = request.Password;
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                _logger.LogInformation($"User {user.Id} is successfully created.");
            }
            else
            {
                throw new CannotUnloadAppDomainException("Cannot create User.");
            }

            return newCompany.Id;

        }

    }
}
