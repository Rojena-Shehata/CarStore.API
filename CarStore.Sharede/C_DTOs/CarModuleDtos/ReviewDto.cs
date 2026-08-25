namespace CarStore.Shared.C_DTOs.CarModuleDtos
{
    public class ReviewDto
    {
        public string ReviewerName { get; set; } = default!;
        public string? ReviewerImage { get; set; }
        public int Rating { get; set; }
        public string Date { get; set; } = default!;
        public string Comment { get; set; } = default!;
    }
}