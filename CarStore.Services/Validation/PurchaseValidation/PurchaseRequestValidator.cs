using System;
using CarStore.Shared.C_DTOs.PurchaseModuleDtos.CommandsDTOs;
using FluentValidation;

namespace CarStore.Services.Validation.PurchaseValidation
{
    public class PurchaseRequestValidator : AbstractValidator<PurchaseRequestDTO>
    {
        public PurchaseRequestValidator()
        {
            RuleFor(x => x.CarId)
                .GreaterThan(0)
                .WithMessage("CarId must be greater than 0.");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name is required.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name is required.");

            RuleFor(x => x.NationalId)
                .NotEmpty()
                .WithMessage("National ID is required.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("A valid email address is required.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone number is required.");

            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("City is required.");

            RuleFor(x => x.ZipCode)
                .NotEmpty()
                .WithMessage("Zip code is required.");

            RuleFor(x => x.PreferredContactTime)
                .NotEmpty()
                .WithMessage("Preferred contact time is required.");

            RuleFor(x => x.PurchaseType)
                .IsInEnum()
                .WithMessage("Invalid purchase type.");

            When(x => x.PurchaseType ==
                Domain.Entities.OrderModule.PurchaseModule.Enums.PurchaseRequestType.Finance, () =>
                {
                    RuleFor(x => x.DownPayment)
                        .NotNull()
                        .GreaterThan(0)
                        .WithMessage("Down payment must be greater than 0 for finance requests.");

                    RuleFor(x => x.LoanTerm)
                        .NotNull()
                        .IsInEnum()
                        .WithMessage("Loan term is required and must be valid for finance requests.");
                });
        }
    }
}