using Application.DTO;
using Application.DTO.BloodRegistrationDTO;
using Application.Service.VolunteerServ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonationSystem.Controllers
{
    [EnableCors("LocalPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteersController(IVolunteerService _service) : ControllerBase
    {
        [Authorize(Roles = "Member")]
        [HttpPost]
        public async Task<IActionResult> RegisterVolunteerDonation([FromBody] RegisterVolunteerDonation request)
        {
            var volunteerRegistration = await _service.RegisterVolunteerDonation(request);

            return Ok(new ApiResponse<RegisterVolunteerDonation>()
            {
                IsSuccess = true,
                Message = "Register volunteer donation successfully"
            });
        }
    }
}
