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
           
        }
    }
}
