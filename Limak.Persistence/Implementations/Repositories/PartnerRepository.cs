using Limak.Application.Abstractions.Repositories;
using Limak.Domain.Entities;
using Limak.Persistence.Contexts;
using Limak.Persistence.Implementations.Repositories.Generic;

namespace Limak.Persistence.Implementations.Repositories;

public class PartnerRepository(LimakDbContext context) : Repository<Partner>(context), IPartnerRepository
{
}
