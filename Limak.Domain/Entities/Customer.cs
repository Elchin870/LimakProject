using Limak.Domain.Entities.Common;

namespace Limak.Domain.Entities;

public class Customer : BaseAuditableEntity
{
    public AppUser AppUser { get; set; }
    public string AppUserId { get; set; }
    public Office Office { get; set; }
    public Guid OfficeId { get; set; }
    public string CustomerCode { get; set; }
    public decimal Balance { get; set; } = 0m;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<BalanceTransaction> BalanceTransactions { get; set; } = new List<BalanceTransaction>();
    public ICollection<Package> Packages { get; set; } = new List<Package>();
}
