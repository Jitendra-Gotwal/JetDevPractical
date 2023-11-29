using JetDevsPrcatical.Data.Entity;
using JetDevsPrcatical.Data.Request;

namespace JetDevsPrcatical.Data.Abstract
{
    public interface IUserRepository
    {
        Task<bool> UserRegister(UserRegisterRequest userRegisterModel);

        Task<Users> AuthenticateAsync(LoginRequest loginRequest);

        Task<Users> GetUserByEmailAsync(string email);
        Task UpdatePasswordAsync(Users user, string newPassword);

        Task UpdateTokenAsync(Users user, string token);
        Task ResetToken(Users user);
        Task<bool> ValidateToken(string userId, string token);
        Task<bool> IsEmailExist(string email);
    }
}
