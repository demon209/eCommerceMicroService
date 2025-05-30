using AuthenticationApi.Application.DTOs;
using eCommerceSharedLibrary.Responses;

namespace AuthenticationApi.Application.Interfaces
{
    // Các interFace của Autheintication
    public interface IUser
    {
        Task<Response> Register(AppUserDTO appUserDTO);
        Task<Response> Login(LoginDTO loginDTO);
        Task<GetUserDTO> GetUser(int userId);

    }
}
