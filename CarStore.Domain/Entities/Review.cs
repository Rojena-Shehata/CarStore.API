using CarStore.Domain.Entities.CarModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Entities
{
    public class Review
    {
        public int CarId { get; set; }
        public string UserId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; } //1:5

    }
}
