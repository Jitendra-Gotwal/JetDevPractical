using FluentValidation;
using JetDevsPrcatical.Data.Request;

namespace JetDevsPrcatical.Validator
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
       
        public LoginRequestValidator()
        {             

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
                        
            RuleFor(user => user.Password)
         .NotEmpty().WithMessage("Password is required.")
         .MaximumLength(50).WithMessage("Password must not exceed 50 characters.");
        }

       
    }
}
