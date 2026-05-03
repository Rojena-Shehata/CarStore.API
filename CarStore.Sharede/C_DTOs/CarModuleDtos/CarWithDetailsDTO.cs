namespace CarStore.Shared.C_DTOs.CarModuleDtos
{
    public class CarWithDetailsDTO:IDAndNameDTO
    {      
        public decimal? Price { get; set; }
        public string PriceLabel { get; set; } = default!;
        public string? Description { get; set; } = default!;
        public int? Year { get; set; }
        public int SeatsNumber { get; set; }
        public int? MaxSpeed { get; set; }

        public string TransmissionType { get; set; } = default!;
        public string FuelType { get; set; } = default!;//
        public string Drivetrain { get; set; } = default!;//
        public EngineDTO Engine { get; set; } = default!;

        //Relationships
        //->> 1 Color 
        public ColorDTO Color { get; set; } = default!;

        //->> 1 Brand
        public BrandDTO Brand { get; set; }


        //->> 1 Category 
        public CategoryDTO Category { get; set; }
        public List<FeatureDTO> Features { get; set; }


    }
}
