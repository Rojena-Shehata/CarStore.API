using CarStore.Domain.Entities.CarModule.Enums;

namespace CarStore.Shared.C_DTOs.CarModuleDtos.Params
{
    public class CarQueryParams
    {
        public int? SeatsNumber { get; set; }
        public string? Search { get; set; }//SearchByName
        public decimal? Price { get; set; }
        public TransmissionType? TransmissionType { get; set; }
        public CarSortingOptions? Sort { get; set; }

        public int? ColorId { get; set; }

        /// <summary>
        /// /Pagination
        /// </summary>
        private const int _minPageSize = 2;
        private const int _maxPageSize = 60;
        private int? _pageSize ;

        public int PageSize
        {
            get { return _pageSize??5; }
            set
            {
                _pageSize = value < _minPageSize ? _minPageSize :
                              value > _maxPageSize ? _maxPageSize :
                              value;
            }
        }

        private int _pageIndex = 1;
        public int PageNumber
        {
            get
            {
                return _pageIndex;
            }
            set
            {
                _pageIndex = value < 1 ? 1 : value;
            }
        }
    }
}
