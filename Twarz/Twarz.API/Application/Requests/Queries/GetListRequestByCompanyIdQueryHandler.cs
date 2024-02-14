using AutoMapper;
using MediatR;
using Twarz.API.Contracts;

namespace Twarz.API.Application.Requests.Queries
{
    public class GetListRequestByCompanyIdQueryHandler : IRequestHandler<GetListRequestByCompanyIdQuery, List<RequestCompanyMv>>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IMapper _mapper;

        public GetListRequestByCompanyIdQueryHandler(IRequestRepository requestRepository, IMapper mapper)
        {
            _requestRepository = requestRepository ?? throw new ArgumentNullException(nameof(requestRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<RequestCompanyMv>> Handle(GetListRequestByCompanyIdQuery request, CancellationToken cancellationToken)
        {
            var requestList = await _requestRepository.GetRequestsByCompanyId(request.CompanyId);
            return _mapper.Map<List<RequestCompanyMv>>(requestList);
        }
    }
}
