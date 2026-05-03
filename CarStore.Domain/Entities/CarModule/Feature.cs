using CarStore.Domain.Entities.CarModule.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Entities.CarModule
{
    public class Feature:BaseEntity<int>
    {
        public string Name { get; set; } = default!;
        public FeatureType Type { get; set; }
    }
}
