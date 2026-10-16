using CarStore.Shared.C_DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Services
{

    public class DriverInfoDtoValidator : AbstractValidator<DriverInfoDto>
    {
        public DriverInfoDtoValidator()
        {
            {
                RuleFor(x => x.FirstName)
                    .NotEmpty()
                    .WithMessage("First name is required.")
                    .MaximumLength(50)
                    .WithMessage("First name cannot exceed 50 characters.")
                   ;

                RuleFor(x => x.LastName)
                    .NotEmpty()
                    .WithMessage("Last name is required.")
                    .MaximumLength(50)
                    .WithMessage("Last name cannot exceed 50 characters.")
                    ;

                RuleFor(x => x.NationalId)
                    .NotEmpty()
                    .WithMessage("National ID is required.")
                    .Length(14)
                    .When(x => !string.IsNullOrWhiteSpace(x.NationalId))
                    .WithMessage("National ID must contain exactly 14 digits.")
                    ;

                RuleFor(x => x.Email)
                    .NotEmpty()
                    .WithMessage("Email address is required.")
                    .EmailAddress()
                    .When(x => !string.IsNullOrWhiteSpace(x.Email))
                    .WithMessage("Please enter a valid email address.")
                    .MaximumLength(254)
                    .WithMessage("Email address cannot exceed 254 characters.");

                RuleFor(x => x.PhoneNumber)
                    .NotEmpty()
                    .WithMessage("Phone number is required.")
                    ;

                RuleFor(x => x.StreetAddress)
                    .NotEmpty()
                    .WithMessage("Street address is required.")
                    .MaximumLength(200)
                    .WithMessage("Street address cannot exceed 200 characters.");

                RuleFor(x => x.City)
                    .NotEmpty()
                    .WithMessage("City is required.")
                    .MaximumLength(100)
                    .WithMessage("City cannot exceed 100 characters.");

                RuleFor(x => x.ZipCode)
                    .NotEmpty()
                    .WithMessage("ZIP or postal code is required.")
                    .MaximumLength(20)
                    .WithMessage("ZIP or postal code cannot exceed 20 characters.");

                RuleFor(x => x.DriverIdNumber)
                    .NotEmpty()
                    .WithMessage("Driver's license number is required.")
                    .MaximumLength(50)
                    .WithMessage("Driver's license number cannot exceed 50 characters.");
            }
        }
}
