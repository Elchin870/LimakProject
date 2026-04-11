using Limak.Application.Abstractions.Repositories.Generic;
using Limak.Domain.Entities;

namespace Limak.Application.Abstractions.Repositories;

public interface ITarifRepository : IRepository<Tariff>
{
    Task<Tariff?> GetApplicableTariffAsync(Guid countryId, Guid deliveryTypeId, Guid shipmentTypeId, decimal weight);
}
