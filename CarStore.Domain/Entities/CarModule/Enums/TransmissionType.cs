using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CarStore.Domain.Entities.CarModule.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransmissionType
    {
        Manual=1,
        Automatic=2
    }
}
