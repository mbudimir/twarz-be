using MediatR;

namespace Twarz.API.Application.Company.Commands
{
    public class CreateCompanyCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? ImageUrl { get; set; }
        
    }
}
