using Limak.Domain.Enums;

namespace Limak.Application.Dtos.PackageDtos;

public class AdminPackageUpdateStatusDto
{
    public Guid PackageId { get; set; }
    public PackageStatus Status { get; set; }
    public string? AdminNote { get; set; }
    public decimal Weight { get; set; }
    public decimal? Width { get; set; }
    public decimal? Length { get; set; }
    public decimal? Height { get; set; }
}
