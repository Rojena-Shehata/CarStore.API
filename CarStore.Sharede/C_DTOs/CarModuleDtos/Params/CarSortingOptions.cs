using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CarStore.Shared.C_DTOs.CarModuleDtos.Params
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CarSortingOptions
    {
        HighSalesBrands,
        A_Z,
        Z_A
    }
}
