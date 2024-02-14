namespace Twarz.API.Domains
{
    public class Company : EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
    }
}
