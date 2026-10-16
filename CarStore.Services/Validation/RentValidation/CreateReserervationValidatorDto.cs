using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarStore.Shared;

namespace CarStore.Services
{
    public class CreateReserervationValidatorDto : AbstractValidator<CreateReservationDto>
    {
        public CreateReserervationValidatorDto()
        {
            RuleFor(x => x.CarId)
              .GreaterThan(0)
              .WithMessage("Car ID must be valid.");

            RuleFor(x => x.PickupOfficeId)
                .GreaterThan(0)
                .WithMessage("Pickup office must be selected.");

            RuleFor(x => x.ReturnOfficeId)
                .GreaterThan(0)
                .WithMessage("Return office must be selected.");

            RuleFor(x => x.PickupDate)
                .NotEmpty()
                .WithMessage("Pickup date is required.")
                .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
                .WithMessage("Pickup date cannot be in the past.");

            RuleFor(x => x.ReturnDate)
                .NotEmpty()
                .WithMessage("Return date is required.")
                .GreaterThan(x => x.PickupDate)
                .WithMessage("Return date must be after pickup date.");

            RuleFor(x => x.DriverInfo)
                .NotNull()
                .SetValidator(new DriverInfoDtoValidator());
        }
    }
}
