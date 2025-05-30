using System.ComponentModel.DataAnnotations;

namespace AuthenticationApi.Application.DTOs
{
    // DTO trả về thông tin khách hàng nhưng không có trường Password
    public record GetUserDTO
    (
       int Id,
       [Required] string Name,
       [Required] string TelephoneNumber,
       [Required] string Address,
       [Required, EmailAddress] string Email,
       [Required] string Role
     );
}
