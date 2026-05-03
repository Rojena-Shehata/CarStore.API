using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Entities.CarModule.Enums
{
    public enum Drivetrain
    {
        FWD=1,
        RWD,
        [Display(Name="4WD")]
        FourWD,
        AWD
    }
}
