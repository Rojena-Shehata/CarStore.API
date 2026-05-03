using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Entities.CarModule
{
    public class CarFeature
    {
        public int CarId { get; set; }
        public Feature Feature { get; set; }
        public Car Car { get; set; }
        public int FeatureId { get; set; }
    }
}
