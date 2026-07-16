using CarStore.Domain.Entities;
using CarStore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Services.Specifications
{
    public class ActiveVerificationCodeSpecification : BaseSpecification<VerificationCode>
    {
        public ActiveVerificationCodeSpecification(string email, VerificationCodePurpose purpose)
            : base(vc => vc.Email == email && vc.Purpose == purpose && !vc.IsUsed)
        {
            AddOrderByDescending(vc => vc.CreatedOn);
        }
    }
}