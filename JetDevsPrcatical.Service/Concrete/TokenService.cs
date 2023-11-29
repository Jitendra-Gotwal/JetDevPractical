using JetDevsPrcatical.Data;
using JetDevsPrcatical.Data.Abstract;
using JetDevsPrcatical.Data.Entity;
using JetDevsPrcatical.Data.Response.Account;
using JetDevsPrcatical.Service.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JetDevsPrcatical.Service.Concrete
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;        
        private readonly IUserRepository _userRepository;

        public TokenService(IConfiguration _configuration,IUserRepository userRepository)
        {          
            configuration = _configuration;           
            _userRepository = userRepository;
        }
        public async Task<LoginResponse> GenerateAccessToken(Users users)
        {
            LoginResponse authenticationResponse = new LoginResponse();

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, users.FirstName),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.Email, users.Email),
                    new Claim(ClaimTypes.Sid,users.UserId.ToString())
                };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: configuration["JWTSettings:Issuer"],
                audience: configuration["JWTSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            authenticationResponse.JWToken = tokenString;          
            authenticationResponse.UserName = users.FirstName;

            await _userRepository.UpdateTokenAsync(users, tokenString);           

            return authenticationResponse;
        }       
    }
}
