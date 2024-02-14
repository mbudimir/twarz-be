using AutoMapper;
using MediatR;
using Twarz.API.Contracts;

namespace Twarz.API.Application.Requests.Queries
{
    public class GetListRequestQueryHandler: IRequestHandler<GetListRequestQuery, List<RequestMv>>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IMapper _mapper;

        public GetListRequestQueryHandler(IRequestRepository requestRepository, IMapper mapper)
        {
            _requestRepository = requestRepository ?? throw new ArgumentNullException(nameof(requestRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<RequestMv>> Handle(GetListRequestQuery request, CancellationToken cancellationToken)
        {
            var requestList = await _requestRepository.GetRequestsByDocumentNumber(request.DocumentNumber);
            return _mapper.Map<List<RequestMv>>(requestList);
        }
    }
}
