using Limak.Domain.Enums;

namespace Limak.Application.Helpers;

public static class PackageStatusTransitions
{
    public static readonly Dictionary<PackageStatus, List<PackageStatus>> AllowedTransitions =
        new()
        {
            { PackageStatus.WaitingForDeclaration, new() { PackageStatus.Declared } },

            { PackageStatus.Declared, new() { PackageStatus.NeedCorrection, PackageStatus.ReadyForWarehouse } },

            { PackageStatus.NeedCorrection, new() { PackageStatus.Declared } },

            { PackageStatus.ReadyForWarehouse, new() { PackageStatus.InWarehouse } },

            { PackageStatus.InWarehouse, new() { PackageStatus.InTransit } },

            { PackageStatus.InTransit, new() { PackageStatus.ArrivedToCountry } },

            { PackageStatus.ArrivedToCountry, new() { PackageStatus.ReadyForDelivery } },

            { PackageStatus.ReadyForDelivery, new() { PackageStatus.Delivered } }
        };
}
