using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.PackageDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Limak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminPackagesController : ControllerBase
    {
        private readonly IPackageService _packageService;

        public AdminPackagesController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpGet("get-all-packages")]
        public async Task<IActionResult> GetAdminPackages()
        {
            var result = await _packageService.GetAdminPackages();
            return Ok(result);
        }

        [HttpPut("update-status")]
        public async Task<IActionResult> UpdatePackageStatus(AdminPackageUpdateStatusDto dto)
        {
            var result = await _packageService.UpdatePackageStatus(dto);
            return Ok(result);
        }

        [HttpGet("get-declared-packages")]
        public async Task<IActionResult> GetDeclaredPackages()
        {
            var result = await _packageService.GetDeclaredPackagesForAdmin();
            return Ok(result);
        }

        [HttpPost("create-package/{adminUserId}")]
        public async Task<IActionResult> CreatePackageAdmin(string adminUserId, AdminCreatePackageDto dto)
        {
            var result = await _packageService.CreatePackageAdmin(adminUserId, dto);
            return Ok(result);
        }
    }
}
