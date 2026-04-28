using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Shared.C_DTOs
{
    public class ColorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string HexCode { get; set; } = default!;
    }
}
