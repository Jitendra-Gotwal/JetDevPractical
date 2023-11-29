using FluentValidation;
using JetDevsPrcatical.Data.Request;

namespace JetDevsPrcatical.Validator
{
    public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
    {
       
        public ForgotPasswordRequestValidator()
        {             

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
                                  
        }

       
    }
}
