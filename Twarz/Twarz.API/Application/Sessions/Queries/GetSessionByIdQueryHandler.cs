using AutoMapper;
using MediatR;
using Twarz.API.Contracts;
using Twarz.API.Domains;

namespace Twarz.API.Application.Sessions.Queries
{
    public class GetSessionbyIdQueryHandler : IRequestHandler<GetSessionbyIdQuery, SessionMv>
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IMapper _mapper;

        public GetSessionbyIdQueryHandler(ISessionRepository sessionRepository, IMapper mapper)
        {
            _sessionRepository = sessionRepository ?? throw new ArgumentNullException(nameof(sessionRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<SessionMv> Handle(GetSessionbyIdQuery request, CancellationToken cancellationToken)
        {
            var session = await _sessionRepository.GetById(request.Id);
            return _mapper.Map<SessionMv>(session);
        }
    }
}
