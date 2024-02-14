using Twarz.API.Domains.Enums;

namespace Twarz.API.Domains
{
    public class Request : EntityBase
    {
        public int Id { get; set; }
        public Company Company { get; set; }
        public DateTime RequestDate { get; set; }
        public Session Session { get; set; }
        public DateTime? SessionDate { get; set; }
        public RequestStatusEnum Status { get; set; } = RequestStatusEnum.Pending;
    }
}
