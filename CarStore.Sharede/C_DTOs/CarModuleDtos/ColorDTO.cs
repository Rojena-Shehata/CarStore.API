using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Shared.C_DTOs.CarModuleDtos
{
    public class ColorDTO:IDAndNameDTO
    {      
        public string HexCode { get; set; } = default!;
    }
}
