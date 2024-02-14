using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Twarz.API.Contracts;
using Twarz.API.Domains;
using Twarz.API.Domains.Enums;

namespace Twarz.API.Application.Requests.Commands
{
    public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, int>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateRequestCommandHandler> _logger;
        private readonly UserManager<User> _userManager;

        public CreateRequestCommandHandler(IRequestRepository requestRepository, ICompanyRepository companyRepository,
            ISessionRepository sessionRepository,IMapper mapper, ILogger<CreateRequestCommandHandler> logger, UserManager<User> userManager)
        {
            _requestRepository = requestRepository;
            _companyRepository = companyRepository;
            _sessionRepository = sessionRepository;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }
        public async Task<int> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
        {
            var companyEntity = await _companyRepository.GetByIdAsync(request.CompanyId);
            var sessionEntity = await _sessionRepository.GetById(request.SessionId);

            var newRequest = new Request
            {
                Company = companyEntity,
                Session = sessionEntity,
                RequestDate = DateTime.Now,
                Status = RequestStatusEnum.Pending
            };

            var requestSaved = await _requestRepository.AddAsync(newRequest);

            // Enviar el push Notification

            _logger.LogInformation($"Request {requestSaved.Id} is successfully created.");

            return requestSaved.Id;

        }

    }
}
