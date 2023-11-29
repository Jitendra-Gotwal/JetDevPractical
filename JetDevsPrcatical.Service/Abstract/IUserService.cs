using JetDevsPrcatical.Data.Entity;
using JetDevsPrcatical.Data.Request;
using JetDevsPrcatical.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetDevsPrcatical.Service.Abstract
{
    public interface IUserService
    {       
        Task<bool> UserRegister(UserRegisterRequest userRegisterModel);
        Task<Users> AuthenticateAsync(LoginRequest loginRequest);

        Task<ForgotPasswordResponse> ForgotPasswordAsync(string email);
        Task<bool> ValidateToken(string userId,string token);
        Task<bool> ResetPassword(ResetPasswordRequest resetPasswordRequest);
        Task<bool> IsEmailExist(string email);
    }
}
