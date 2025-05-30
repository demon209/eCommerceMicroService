
namespace AuthenticationApi.Domain.Entities
{
    public class AppUser
    {
        // Các trường thông tin của Appuser
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? TelephoneNumber { get; set; }
        public string? Address {  get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public DateTime DatePegistered { get; set; } = DateTime.UtcNow;

    }
}
