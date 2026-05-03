using CarStore.Domain.Entities.CarModule;
using CarStore.Domain.Entities.CarModule.Enums;
using CarStore.Shared.C_DTOs.CarModuleDtos.Params;
using System.Linq.Expressions;

namespace E_Commerce.Services.Specifications.ProductSpecifications
{
    internal static class CarSpecificationHelper
    {

        internal static Expression<Func<Car, bool>> CreateCriteria(CarQueryParams parameters)
        {
            return car => (!parameters.SeatsNumber.HasValue || car.SeatsNumber == parameters.SeatsNumber.Value)
                        && (!parameters.TransmissionType.HasValue || car.TransmissionType == parameters.TransmissionType.Value)
                        && (!parameters.ColorId.HasValue || car.ColorId == parameters.ColorId.Value)

                        //price=> check daily rent price and sale price
                        && (!parameters.Price.HasValue ||
                            (car.CarMode == CarMode.Rent &&
                                car.DailyRentPrice.HasValue && car.DailyRentPrice.Value <= parameters.Price.Value)
                            || (car.CarMode == CarMode.Sale &&
                                car.SalePrice.HasValue && car.SalePrice.Value <= parameters.Price.Value))

                        && (string.IsNullOrWhiteSpace(parameters.Search) || car.Name.ToLower().Contains(parameters.Search.ToLower()));

        }
    }
}
