using Limak.Domain.Entities.Common;

namespace Limak.Domain.Entities;

public class CountryAddress : BaseAuditableEntity
{
    public string City { get; set; }
    public string State { get; set; }
    public Country Country { get; set; }
    public Guid CountryId { get; set; }
    public string? TC { get; set; }
    public string Address { get; set; }
}
