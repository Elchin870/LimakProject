using Limak.Application.Abstractions.Repositories;
using Limak.Domain.Entities;
using Limak.Persistence.Contexts;
using Limak.Persistence.Implementations.Repositories.Generic;

namespace Limak.Persistence.Implementations.Repositories;

public class ShipmentTypeRepository(LimakDbContext context) : Repository<ShipmentType>(context), IShipmentTypeRepository
{
}
