using Application.DTO.BloodRegistration;
using Application.Service.BloodRegistrationServ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTO;
using Application.DTO.BloodRegistrationDTO;

namespace BloodDonationSystem.Controllers
{
    [ApiController]
    public class BloodRegistrationsController(IBloodRegistrationService _service) : ControllerBase
    {
        [Authorize(Roles = "Member")]
        [HttpPost]
        [Route("api/events/{id}/blood-registrations")]
        public async Task<IActionResult> RegisterDonation(int id, [FromBody] BloodRegistrationRequest request)
        {
            var bloodRegistration = await _service.RegisterDonation(id, request);
            if (bloodRegistration == null)
                return BadRequest(new ApiResponse<BloodRegistrationRequest>()
                {
                    IsSuccess = false,
                    Message = "Event not found or registration unsuccessfully"
                });

            return Ok(new ApiResponse<BloodRegistrationRequest>()
            {
                IsSuccess = true,
                Message = "Register donation successfully"
            });
        }

        [Authorize(Roles = "Staff")]
        [HttpPut("api/blood-registrations/{id}/reject")]
        public async Task<IActionResult> RejectRegistration(int id)
        {
            var bloodRegistration = await _service.RejectRegistration(id);
            if (bloodRegistration == null)
                return BadRequest(new ApiResponse<RejectBloodRegistration>()
                {
                    IsSuccess = false,
                    Message = "Blood registration not found or reject unsuccessfully"
                });

            return Ok(new ApiResponse<RejectBloodRegistration>()
            {
                IsSuccess = true,
                Message = "Reject blood registration successfully"
            });
        }

        [Authorize(Roles = "Member")]
        [HttpPut("api/blood-registrations/{id}/cancel-own")]
        public async Task<IActionResult> CancelOwnRegistration(int id)
        {
            var bloodRegistration = await _service.CancelOwnRegistration(id);
            if (bloodRegistration == null)
                return BadRequest(new ApiResponse<CancelOwnRegistration>
                {
                    IsSuccess = false,
                    Message = "Blood registration not found or cancel unsuccessfully"
                });

            return Ok(new ApiResponse<CancelOwnRegistration>()
            {
                IsSuccess = true,
                Message = "Cancel registration successfully"
            });
        }
    }
}