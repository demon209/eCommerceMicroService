namespace AuthenticationApi.Application.DTOs
{
    // Thông tin login
    public record LoginDTO
    (
        string Email,
        string Password
    );
}
