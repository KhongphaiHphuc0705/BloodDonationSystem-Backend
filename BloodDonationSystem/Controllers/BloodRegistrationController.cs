using Application.DTO.BloodRegistration;
using Application.DTO.BloodRegistrationDTO;
using Application.Service.BloodRegistrationServ;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            return Ok(new
            {
                Message = "Register donation successfully"
            });
        }

        [Authorize(Roles = "Staff")]
        [HttpPatch("evaluate/{id}")]
        public async Task<IActionResult> EvaluateRegistration(int id, [FromBody] EvaluateBloodRegistration evaluation)
        {
            var bloodRegistration = await _service.EvaluateRegistration(id, evaluation);
            if (bloodRegistration == null)
                return NotFound(new
                {
                    Message = "Blood registration not found"
                });

            return Ok(new
            {
                Message = "Evaluate registration successfully",
            });
        }

        [Authorize(Roles = "Member")]
        [HttpPatch("cancel/{id}")]
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
