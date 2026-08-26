using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Shared.C_DTOs.CarModuleDtos
{
    public class ReviewSummaryDto
    {
        public int ReviewsCount { get; set; }
        public double AverageRating { get; set; }
        public List<ReviewDto> Reviews { get; set; } = [];
    }
}
