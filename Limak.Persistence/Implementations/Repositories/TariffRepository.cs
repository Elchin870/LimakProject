using Limak.Application.Abstractions.Repositories;
using Limak.Domain.Entities;
using Limak.Persistence.Contexts;
using Limak.Persistence.Implementations.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

public class TariffRepository(LimakDbContext context) : Repository<Tariff>(context), ITarifRepository
{
    public async Task<Tariff?> GetApplicableTariffAsync(Guid countryId, Guid deliveryTypeId, Guid shipmentTypeId, decimal weight)
    {
        return await context.Tariffs
            .Include(x => x.Country)
            .Include(x => x.DeliveryType)
            .Include(x => x.ShipmentType)
            .Where(x =>
                x.CountryId == countryId &&
                x.DeliveryTypeId == deliveryTypeId &&
                x.ShipmentTypeId == shipmentTypeId &&
                weight >= x.MinWeight &&
                weight < x.MaxWeight)
            .FirstOrDefaultAsync();
    }

}
