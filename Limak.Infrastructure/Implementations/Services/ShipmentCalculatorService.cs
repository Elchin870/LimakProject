using Limak.Application.Abstractions.Repositories;
using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.CalcullatorDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Exceptions;
using Limak.Domain.Entities;

namespace Limak.Infrastructure.Implementations.Services;

public class ShipmentCalculatorService : IShipmentCalculatorService
{
    private readonly ITarifRepository _tariffRepository;
    private const decimal VolumetricThresholdCm = 100m;
    private const decimal VolumetricDivisor = 6000m;

    public ShipmentCalculatorService(ITarifRepository tariffRepository)
    {
        _tariffRepository = tariffRepository;
    }

    public async Task<ResultDto<decimal>> CalculatePriceAsync(ShipmentCalculateDto dto)
    {
        var chargeableWeight = CalculateChargeableWeight(dto);

        var tariff = await _tariffRepository.GetApplicableTariffAsync(dto.CountryId, dto.DeliveryTypeId, dto.ShipmentTypeId, chargeableWeight);

        if (tariff is null)
            throw new NotFoundException("No applicable tariff found for given parameters.");

        return new(CalculateTariffPrice(tariff, chargeableWeight));
    }

    private decimal CalculateChargeableWeight(ShipmentCalculateDto dto)
    {
        bool volumetricRequired =
            (dto.Length.HasValue && dto.Length.Value > VolumetricThresholdCm) ||
            (dto.Width.HasValue && dto.Width.Value > VolumetricThresholdCm) ||
            (dto.Height.HasValue && dto.Height.Value > VolumetricThresholdCm);

        decimal volumetricWeight = 0m;

        if (dto.Length.HasValue && dto.Width.HasValue && dto.Height.HasValue)
        {
            volumetricWeight =
                (dto.Length.Value * dto.Width.Value * dto.Height.Value) / VolumetricDivisor;
        }

        if (volumetricRequired)
        {
            return RoundUpTo2Decimals(volumetricWeight);
        }

        var finalWeight = Math.Max(dto.Weight, volumetricWeight);

        return RoundUpTo2Decimals(finalWeight);
    }

    private decimal CalculateTariffPrice(Tariff tariff, decimal weight)
    {
        if (!tariff.ExtraPricePerKg.HasValue)
            return tariff.BasePrice;

        if (weight <= tariff.BaseWeightLimit)
            return tariff.BasePrice;

        var extraWeight = weight - tariff.BaseWeightLimit;

        extraWeight = Math.Ceiling(extraWeight);

        return tariff.BasePrice + (extraWeight * tariff.ExtraPricePerKg.Value);
    }

    private static decimal RoundUpTo2Decimals(decimal value)
    {
        return Math.Ceiling(value * 100m) / 100m;
    }
}
