using Limak.Domain.Entities.Common;
using Limak.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Limak.Persistence.Interceptors;

public class BaseAuditableInterceptor:SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateAuditColummns(eventData);
        return base.SavingChanges(eventData, result);
    }


    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateAuditColummns(eventData);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateAuditColummns(DbContextEventData eventData)
    {
        if (eventData.Context is LimakDbContext limakDbContext)
        {
            var entries = limakDbContext.ChangeTracker.Entries<BaseAuditableEntity>().ToList();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        entry.Entity.DeletedAt = DateTime.UtcNow.AddHours(4);
                        entry.Entity.DeletedBy = "Elchin";
                        entry.Entity.IsDeleted = true;
                        entry.State = EntityState.Modified;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow.AddHours(4);
                        entry.Entity.UpdatedBy = "Elchin";
                        break;
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow.AddHours(4);
                        entry.Entity.CreatedBy = "Elchin";
                        break;
                    default:
                        break;
                }
            }
        }
    }

}
