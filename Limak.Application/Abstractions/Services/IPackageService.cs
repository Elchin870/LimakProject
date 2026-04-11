using Limak.Application.Dtos.PackageDtos;
using Limak.Application.Dtos.ResultDtos;

namespace Limak.Application.Abstractions.Services;

public interface IPackageService
{
    Task<ResultDto> DeclarePackage(UserPackageDeclareDto dto);
    Task<ResultDto<List<UserPackageListDto>>> GetUserPackages(Guid customerId);
    Task<ResultDto<List<AdminPackageListDto>>> GetAdminPackages();
    Task<ResultDto> UpdatePackageStatus(AdminPackageUpdateStatusDto dto);
    Task<ResultDto<List<UserPackageListDto>>> GetWaitingForDeclarePackages(Guid customerId);
    Task<ResultDto<List<AdminPackageListDto>>> GetDeclaredPackagesForAdmin();
    Task<ResultDto> PayForPackage(PackagePaymentDto dto);
    Task<ResultDto> CreatePackageAdmin(string adminUserId, AdminCreatePackageDto dto);
}
