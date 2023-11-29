
using AutoMapper;
using FluentValidation;
using JetDevsPrcatical.Data;
using JetDevsPrcatical.Data.Abstract;
using JetDevsPrcatical.Data.Concrete;
using JetDevsPrcatical.Data.Request;
using JetDevsPrcatical.Service.Abstract;
using JetDevsPrcatical.Service.Concrete;
using JetDevsPrcatical.Validator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;





namespace JetDevsPrcatical.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region DB
            services.AddDbContext<JetDevsPrcaticalContext>(options =>
              options.UseSqlServer(
                  configuration.GetConnectionString("JetDevsPrcatical")), ServiceLifetime.Transient);
            #endregion

            #region JWT
            
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = configuration["JWTSettings:Issuer"],
                ValidAudience = configuration["JWTSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"])),
            };
            services.AddSingleton(tokenValidationParameters);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenValidationParameters;
                });
            #endregion

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "JetDevs.Api",
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },new List<string>()
                    },
        });
            });
            #endregion
        }

        public static void AddRepositoryServicesWithAutoMapper(this IServiceCollection services)
        {
            #region Repository

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ITokenService, TokenService>();          
            services.AddScoped<IEmailSender, EmailSender>();

            #endregion

            #region Validator            
            services.AddTransient<IValidator<UserRegisterRequest>, UserRegistrationValidator>();
            services.AddTransient<IValidator<LoginRequest>, LoginRequestValidator>();
            services.AddTransient<IValidator<ForgotPasswordRequest>, ForgotPasswordRequestValidator>();
            services.AddTransient<IValidator<ResetPasswordRequest>, ResetPasswordRequestValidator>();
            
            #endregion

            #region AutoMapper

            services.AddSingleton(new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            }).CreateMapper());
            #endregion           
        }
    }
}
