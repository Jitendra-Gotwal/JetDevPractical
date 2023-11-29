using JetDevsPrcatical.Data.Request;
using JetDevsPrcatical.Data.Response;
using JetDevsPrcatical.Data.Response.Account;
using JetDevsPrcatical.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JetDevsPrcatical.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {

        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;


        public AccountController(ILogger<AccountController> logger, IUserService userService, ITokenService tokenService)
        {
            _logger = logger;
            _userService = userService;
            _tokenService = tokenService;
        }


        [AllowAnonymous]
        [HttpPost("Registration")]
        public async Task<IActionResult> UserRegister(UserRegisterRequest userRegisterModel)
        {
            _logger.LogInformation("User registration process started. "+ userRegisterModel);
            Response<string> response = new();
            if (userRegisterModel is null)
            {
                response.Message = "Invalid client request.";
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                _logger.LogError("Invalid client request.");
                return BadRequest(response);
            }

            var result = await _userService.UserRegister(userRegisterModel);
            response.Data = Convert.ToString(result);
            response.Message = "Record Inserted Successfully..";
            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            _logger.LogInformation("Record Inserted Successfully...");
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            _logger.LogInformation("Login process started. "+  loginRequest);
            Response<LoginResponse> response = new Response<LoginResponse>();

            if (loginRequest is null)
            {
                response.Message = "Invalid client request.";
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }
            var user = await _userService.AuthenticateAsync(loginRequest);

            if (user is null)
            {
                response.Message = "Invalid Email or Password.";
                response.Success = false;
                response.StatusCode = HttpStatusCode.Unauthorized;
                return Unauthorized(response);
            }

            var result = await _tokenService.GenerateAccessToken(user);

            response.Data = result;
            response.Message = "Access Token Generated.";
            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            _logger.LogInformation("Access Token Generated.");
            return Ok(response);

        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            _logger.LogInformation("Forgot Password started. " + forgotPasswordRequest.Email);
            Response<ForgotPasswordResponse> response = new Response<ForgotPasswordResponse>();

            var forgotPasswordResponse = await _userService.ForgotPasswordAsync(forgotPasswordRequest.Email);

            response.Data = forgotPasswordResponse;
            response.Message = "Password reset link sent successfully.";
            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            _logger.LogInformation("Password reset link sent successfully.");
            return Ok(response);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            _logger.LogInformation("Reset Password started. " + resetPasswordRequest);
            Response<string> response = new Response<string>();

            LoginRequest loginRequest = new()
            {
                Email = resetPasswordRequest.Email,
                Password = resetPasswordRequest.OldPassword
            };
            var user = await _userService.AuthenticateAsync(loginRequest);

            if (user is null)
            {
                response.Message = "Invalid Email or old password.";
                response.Success = false;
                response.StatusCode = HttpStatusCode.Unauthorized;
                return NotFound(response);
            }

            await _userService.ResetPassword(resetPasswordRequest);

            response.Data = "Password reset successful";
            response.Message = "Password reset successful.";
            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            _logger.LogInformation("Password reset successful.");
            return Ok(response);

        }

    }
}