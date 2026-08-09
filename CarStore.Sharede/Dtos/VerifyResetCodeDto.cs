using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Shared.Dtos
{
    public sealed record VerifyResetCodeDto
         (
             string Email,
             string Code
         );
}
