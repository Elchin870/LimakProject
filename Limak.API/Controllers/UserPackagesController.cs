using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.PackageDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Limak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPackagesController : ControllerBase
    {
        private readonly IPackageService _packageService;

        public UserPackagesController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpPost("declare-package")]
        public async Task<IActionResult> DeclarePackage(UserPackageDeclareDto dto)
        {
            var result = await _packageService.DeclarePackage(dto);
            return Ok(result);
        }

        [HttpGet("get-user-packages/{customerId}")]
        public async Task<IActionResult> GetUserPackages(Guid customerId)
        {
            var result = await _packageService.GetUserPackages(customerId);
            return Ok(result);
        }

        [HttpGet("get-waiting-for-declare-packages/{customerId}")]
        public async Task<IActionResult> GetWaitingForDeclarePackages(Guid customerId)
        {
            var result = await _packageService.GetWaitingForDeclarePackages(customerId);
            return Ok(result);
        }

        [HttpPost("pay-package")]
        public async Task<IActionResult> PayForPackage(PackagePaymentDto dto)
        {
            var result = await _packageService.PayForPackage(dto);
            return Ok(result);
        }
    }
}
