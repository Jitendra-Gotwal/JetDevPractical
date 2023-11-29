using JetDevsPrcatical.Data.Response;
using JetDevsPrcatical.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace JetDevsPrcatical.Middileware
{
    public class JwtTokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenValidationParameters _tokenValidationParameters;


        public JwtTokenValidationMiddleware(RequestDelegate next, TokenValidationParameters tokenValidationParameters)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _tokenValidationParameters = tokenValidationParameters ?? throw new ArgumentNullException(nameof(tokenValidationParameters));

        }

        public async Task Invoke(HttpContext context, IUserService _userService)
        {
            var endpoint = context.GetEndpoint();

            // Check if the endpoint allows anonymous access
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                await _next(context);
                return;
            }

            if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var token = authHeader.ToString().Replace("Bearer ", string.Empty);
                token = token.ToString().Replace("bearer ", string.Empty);

                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out _);

                    var userId = principal?.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid").Value;

                    bool isValidToken = await _userService.ValidateToken(userId, token);
                    if (isValidToken)
                    {
                        await _next(context);
                        return;
                    }
                }
                catch (SecurityTokenException)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    var results = JsonConvert.SerializeObject(new Response<string>("You are not Authorized", 401));
                    await context.Response.WriteAsync(results);
                    return;
                }
            }

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new Response<string>("You are not Authorized", 401));
            await context.Response.WriteAsync(result);
        }
    }
    public static class JwtTokenValidationMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtTokenValidation(this IApplicationBuilder builder, TokenValidationParameters tokenValidationParameters)
        {
            return builder.UseMiddleware<JwtTokenValidationMiddleware>(tokenValidationParameters);
        }
    }
}
