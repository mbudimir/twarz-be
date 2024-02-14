using System.Text.Json.Serialization;

namespace Twarz.API.Domains
{
    public class Session: EntityBase
    {
        public long Id { get; set; }
        public string DocumentNumber { get; set; }
        public string Surname { get; set; }
        public string GivenName { get; set; }
        public string Copy { get; set; }
        public string IdentityCardNumber { get; set; }
        public string PersonalNumber { get; set; }
        public string Nationality { get; set; }
        public string Sex { get; set; }
        public string IssuingStateCode { get; set; }
        public string IssuingState { get; set; }
        public byte[] Portrait { get; set; }
        public byte[] DocumentImage { get; set; }
        public string DocumentCategory { get; set; }
        public string DocumentType { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string TokenDevice { get; set; }

    }
}
