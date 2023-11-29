
using JetDevsPrcatical.Common;
using JetDevsPrcatical.Data.Abstract;
using JetDevsPrcatical.Data.Entity;
using JetDevsPrcatical.Data.Request;
using JetDevsPrcatical.Data.Response;
using JetDevsPrcatical.Service.Abstract;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Mail;

namespace JetDevsPrcatical.Service.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<bool> UserRegister(UserRegisterRequest userRegisterModel)
        {
            userRegisterModel.Password = PasswordHelper.GetSHA256Password(userRegisterModel.Password);
            return await _userRepository.UserRegister(userRegisterModel);
        }

        public async Task<Users> AuthenticateAsync(LoginRequest loginRequest)
        {
            loginRequest.Password = PasswordHelper.GetSHA256Password(loginRequest.Password);
            return await _userRepository.AuthenticateAsync(loginRequest);
        }


        public async Task<bool> ValidateToken(string userId, string token)
        {

            return await _userRepository.ValidateToken(userId, token);
        }
        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(string email)
        {
            ForgotPasswordResponse response = new ForgotPasswordResponse();
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user != null)
            {
                // Generate a new password 
                string newPassword = PasswordHelper.GenerateRandomPassword();

                // Update user's password
                string encryptedPassword = PasswordHelper.GetSHA256Password(newPassword);
                await _userRepository.UpdatePasswordAsync(user, encryptedPassword);

                // ResetToken Null
                await _userRepository.ResetToken(user);

                // Send an email to the user with the new password or a password reset link           
                await SendPasswordResetEmail(user.Email, newPassword);
                response.NewPassword = newPassword;
            }
            return response;
        }

        private async Task SendPasswordResetEmail(string email, string newPassword)
        {
            var fromAddress = new MailAddress("your_email@example.com", "Your Name");
            var toAddress = new MailAddress(email);

            const string fromPassword = "your_email_password"; // Replace with your email password
            const string subject = "Password Reset";
            string body = $"Your new password is: {newPassword}";

            var smtp = new SmtpClient
            {
                Host = "smtp.example.com", // Update with your SMTP server address
                Port = 587, // Update with your SMTP server port
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            try
            {
                await smtp.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                // Handle email sending errors
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        public async Task<bool> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {

            var user = await _userRepository.GetUserByEmailAsync(resetPasswordRequest.Email);           
            resetPasswordRequest.NewPassword = PasswordHelper.GetSHA256Password(resetPasswordRequest.NewPassword);

            // Update the user in the repository
            await _userRepository.UpdatePasswordAsync(user, resetPasswordRequest.NewPassword);

            // ResetToken Null
            await _userRepository.ResetToken(user);

            return true;

        }

        public async Task<bool> IsEmailExist(string email)
        {
            return await _userRepository.IsEmailExist(email);
        }
    }
}
