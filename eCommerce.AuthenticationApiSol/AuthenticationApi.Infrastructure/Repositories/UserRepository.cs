using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Infrastructure.Data;
using eCommerceSharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationApi.Infrastructure.Repositories
{
    internal class UserRepository(AuthenticationDbContext context, IConfiguration config) : IUser
    {

        // phương thức lấy user từ email (nội bộ) trả về AppUser
        private async Task<AppUser> GetUserByEmail(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(u=>u.Email == email);
            return user!;
        }        
        //Phương thức tạo token JWT để bảo mật các api phân quyền (role)
        private string GenerateToken(AppUser user)
        {
            var key = Encoding.UTF8.GetBytes(config.GetSection("Authentication:Key").Value!);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            // Gán thông tin người dùng vào token
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Name!),
                new(ClaimTypes.Email, user.Email!)
            };
            // nếu có role thì gán
            if (!string.IsNullOrEmpty(user.Role))
                claims.Add(new(ClaimTypes.Role, user.Role));
            // tạo token
            var token = new JwtSecurityToken(
                issuer: config["Authentication:Issuer"],
                audience: config["Authentication:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30), // Thêm thời gian hết hạn
                signingCredentials: credentials
            );
            // trả về token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<GetUserDTO> GetUser(int userId)
        {
            var user = await context.Users.FindAsync(userId);
            return user != null ? new GetUserDTO(
                user.Id,
                user.Name!,
                user.TelephoneNumber!,
                user.Address!,
                user.Email!,
                user.Role!) : null!;
        }

        // Login sử dụng bycript để lưu thông tin password cũng như decode để đăng nhập
        public async Task<Response> Login(LoginDTO loginDTO)
        {
            var getUser = await GetUserByEmail(loginDTO.Email);
            if (getUser == null)
                return new Response(false, "Invalid credentials");
            bool verifyPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, getUser.Password);
            if (!verifyPassword)
                return new Response(false, "Invalid credentials");
            // Tạo token với thông tin đăng nhập
            string token = GenerateToken(getUser); 
            return new Response(true, token); // trả về Message là token, dùng token này để thực hiện các api bảo mật bằng post man
        }



        public async Task<Response> Register(AppUserDTO appUserDTO)
        {
            var getUser = await GetUserByEmail(appUserDTO.Email);
            if (getUser != null)
                return new Response(false, "You cannot use this email for registration");
            var result = context.Users.Add(new AppUser()
            {
                Name = appUserDTO.Name,
                Email = appUserDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(appUserDTO.Password),
                TelephoneNumber = appUserDTO.TelephoneNumber,
                Address = appUserDTO.Address,
                Role = appUserDTO.Role
            });

            await context.SaveChangesAsync();
            return result.Entity.Id > 0 ? new Response(true, "User registered successfully") :
                new Response(false, "Invalid data provided");
        }
    }
}
