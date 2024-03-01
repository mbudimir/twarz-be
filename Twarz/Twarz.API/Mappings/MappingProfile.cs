using AutoMapper;
using Twarz.API.Application.Company;
using Twarz.API.Application.Company.Commands;
using Twarz.API.Application.Requests.Queries;
using Twarz.API.Application.Sessions.Commands;
using Twarz.API.Application.Sessions.Queries;
using Twarz.API.Domains;

namespace Twarz.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Session, SessionMv>().ReverseMap();
            CreateMap<Session, SessionRequestMv>().ReverseMap();
            CreateMap<Company, CompanyMv>().ReverseMap();
            CreateMap<Request, RequestMv>().ReverseMap();
            CreateMap<Request, RequestCompanyMv>().ReverseMap();
            CreateMap<Session, CreateSessionCommand>().ReverseMap();
            CreateMap<Company, CreateCompanyCommand>().ReverseMap();
        }
    }
}
