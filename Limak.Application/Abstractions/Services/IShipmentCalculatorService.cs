using Limak.Application.Dtos.CalcullatorDtos;
using Limak.Application.Dtos.ResultDtos;

namespace Limak.Application.Abstractions.Services;

public interface IShipmentCalculatorService
{
    Task<ResultDto<decimal>> CalculatePriceAsync(ShipmentCalculateDto dto);
}
