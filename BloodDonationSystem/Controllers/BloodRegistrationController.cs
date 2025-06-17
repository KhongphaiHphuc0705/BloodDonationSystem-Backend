using Application.DTO.BloodRegistration;
using Application.DTO.BloodRegistrationDTO;
using Application.Service.BloodRegistrationServ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloodRegistrationController : ControllerBase
    {
        private readonly IBloodRegistrationService _service;

        public BloodRegistrationController(IBloodRegistrationService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Member")]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterDonation([FromBody] BloodRegistrationRequest request)
        {
            var bloodRegistration = await _service.RegisterDonation(request);
            if (bloodRegistration == null)
                return BadRequest(new
                {
                    Message = "Event not found or registration unsuccessfully"
                });

            return Ok(new
            {
                Message = "Register donation successfully"
            });
        }

        [Authorize(Roles = "Staff")]
        [HttpPut("reject/{id}")]
        public async Task<IActionResult> RejectRegistration(int id)
        {
            var bloodRegistration = await _service.RejectRegistration(id);
            if (bloodRegistration == null)
                return NotFound(new
                {
                    Message = "Blood registration not found or reject unsuccessfully"
                });

            return Ok(new
            {
                Message = "Reject blood registration successfully",
            });
        }

        [Authorize(Roles = "Member")]
        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelOwnRegistration(int id)
        {
            var bloodRegistration = await _service.CancelOwnRegistration(id);
            if (bloodRegistration == null)
                return NotFound(new
                {
                    Message = "Blood registration not found or cancel unsuccessfully"
                });

            return Ok(new
            {
                Message = "Cancel registration successfully"
            });
        }

        [Authorize(Roles = "Member")]
        [HttpPost("register-volunteer")]
        public async Task<IActionResult> RegisterVolunteerDonation([FromBody] RegisterVolunteerDonation request)
        {
            var volunteerRegistration = await _service.RegisterVolunteerDonation(request);

            return Ok(new
            {
                Message = "Register volunteer donation successfully"
            });
        }
    }
}
