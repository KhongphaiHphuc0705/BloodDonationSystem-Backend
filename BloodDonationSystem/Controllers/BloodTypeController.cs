using Application.Service.BloodTypeServ;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonationSystem.Controllers
{
    [EnableCors("LocalPolicy")]
    [ApiController]
    public class BloodTypeController(IBloodTypeService _bloodTypeService) : ControllerBase
    {
        [HttpGet("api/bloodtypes")]
        public async Task<IActionResult> GetAllBloodTypes()
        {
            var bloodTypes = await _bloodTypeService.GetAllBloodTypesAsync();
            if (bloodTypes == null || !bloodTypes.Any())
            {
                return NotFound(new { Message = "No blood types found." });
            }
            return Ok(bloodTypes);
        }
    }
}
