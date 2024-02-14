using AutoMapper;
using MediatR;
using Twarz.API.Contracts;
using Twarz.API.Domains;

namespace Twarz.API.Application.Sessions.Queries
{
    public class GetListSessionQueryHandler: IRequestHandler<GetListSessionQuery, List<SessionMv>>
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IMapper _mapper;

        public GetListSessionQueryHandler(ISessionRepository sessionRepository, IMapper mapper)
        {
            _sessionRepository = sessionRepository ?? throw new ArgumentNullException(nameof(sessionRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<SessionMv>> Handle(GetListSessionQuery request, CancellationToken cancellationToken)
        {
            var sessionList = await _sessionRepository.GetSessionsByDocumentNumber(request.DocumentNumber);
            return _mapper.Map<List<SessionMv>>(sessionList);
        }
    }
}
