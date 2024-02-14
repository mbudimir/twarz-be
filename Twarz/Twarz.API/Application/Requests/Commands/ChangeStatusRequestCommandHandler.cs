using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Twarz.API.Contracts;
using Twarz.API.Domains;
using Twarz.API.Domains.Enums;

namespace Twarz.API.Application.Requests.Commands
{
    public class ChangeStatusRequestCommandHandler : IRequestHandler<ChangeStatusRequestCommand, int>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly ILogger<ChangeStatusRequestCommand> _logger;

        public ChangeStatusRequestCommandHandler(IRequestRepository requestRepository, ILogger<ChangeStatusRequestCommand> logger)
        {
            _requestRepository = requestRepository;
            _logger = logger;
        }
        public async Task<int> Handle(ChangeStatusRequestCommand request, CancellationToken cancellationToken)
        {
            var requestEntity = await _requestRepository.GetByIdAsync(request.Id);

            if (requestEntity == null)
            {
                _logger.LogInformation("No se encontro el request");
                return 0;
            }

            requestEntity.Status = request.newState;

            await _requestRepository.UpdateAsync(requestEntity);
            _logger.LogInformation("Se cambio el estado del request.");
            return requestEntity.Id;
        }

    }
}
