using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Shared.C_DTOs.PurchaseModuleDtos.CommandsDTOs
{
    public class PurchaseResponseDTO
    {
        public int PurchaseId { get; set; }

        public string ReferenceNumber { get; set; } = default!;
    }
}
