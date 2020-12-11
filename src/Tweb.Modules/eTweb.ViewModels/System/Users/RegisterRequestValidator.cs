using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using eTweb.Utilities.ValidatorExtensions;

namespace eTweb.ViewModels.System.Users
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name is required.")
                .MaximumLength(200)
                .WithMessage("First name cannot over 200 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name is required.")
                .MaximumLength(200)
                .WithMessage("Last name cannot over than 200 characters.");

            RuleFor(x => x.Dob)
                .NotEmpty()
                .WithMessage("Date of birth is required.")
                .GreaterThan(DateTime.Now.AddYears(-130))
                .WithMessage("Date of birth cannot over than 130 years.");

            RuleFor(x => x.Email)
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Email format not match.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone number is required.");

            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("User name is required.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password is at least 8 characters.");

            RuleFor(x => x)
                 .Custom((request, context) =>
                 {
                     if (request.ConfirmPassword != request.Password)
                         context.AddFailure("Confirm password is not match");
                 });
        }
    }
}