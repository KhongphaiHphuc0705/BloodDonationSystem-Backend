﻿using Application.DTO;
using Application.DTO.BloodRegistration;
using Application.DTO.BloodRegistrationDTO;
using Application.Service.BloodHistoryServ;
using Application.Service.BloodRegistrationServ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonationSystem.Controllers
{
    [EnableCors("LocalPolicy")]
    [ApiController]
    public class BloodRegistrationsController(IBloodRegistrationService _service, IBloodHistoryService _historyService) : ControllerBase
    {
        [Authorize(Roles = "Member")]
        [HttpPost("api/events/{eventId}/blood-registrations")]
        public async Task<IActionResult> RegisterDonation(int eventId, [FromBody] BloodRegistrationRequest request)
        {
            var apiResponse = await _service.RegisterDonation(eventId, request);

            if (apiResponse != null && apiResponse.IsSuccess == false)
                return BadRequest(apiResponse);

            return Ok(apiResponse);
        }

        [Authorize(Roles = "Staff")]
        [HttpPut("api/blood-registrations/{bloodRegisId}/reject")]
        public async Task<IActionResult> RejectBloodRegistration(int bloodRegisId)
        {
            var bloodRegistration = await _service.RejectBloodRegistration(bloodRegisId);
            if (bloodRegistration == null)
                return BadRequest(new ApiResponse<BloodRegistrationRequest>()
                {
                    IsSuccess = false,
                    Message = "Reject blood registration unsuccessfully"
                });

            return Ok(new ApiResponse<BloodRegistrationRequest>()
            {
                IsSuccess = true,
                Message = "Reject blood registration successfully"
            });
        }

        [Authorize(Roles = "Member")]
        [HttpPut("api/blood-registrations/{bloodRegisId}/cancel-own")]
        public async Task<IActionResult> CancelOwnRegistration(int bloodRegisId)
        {
            var apiResponse = await _service.CancelOwnRegistration(bloodRegisId);
            if (apiResponse?.IsSuccess == false)
                return BadRequest(apiResponse);

            return Ok(apiResponse);
        }

        [Authorize(Roles = "Staff")]
        [HttpGet("api/events/{eventId}/blood-registrations")]
        public async Task<IActionResult> GetBloodRegistrationsByPaged(int eventId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var bloodRegisResponse = await _service.GetBloodRegistrationsByPaged(eventId, pageNumber, pageSize);

            if (bloodRegisResponse == null)
                return NotFound(new ApiResponse<PaginatedResultBloodRegis>
                {
                    IsSuccess = false,
                    Message = "Not found event"
                });

            return Ok(new ApiResponse<PaginatedResultBloodRegis>()
            {
                IsSuccess = true,
                Message = "Get blood registrations successfully",
                Data = bloodRegisResponse
            });
        }

        [Authorize]
        [HttpGet("api/event-registration-history")]
        public async Task<IActionResult> GetEventRegistrationHistory()
        {
            var bloodHistory = await _historyService.GetBloodRegistraionHistoryAsync();

            if (bloodHistory == null || !bloodHistory.Any())
            {
                return NotFound(new
                {
                    IsSuccess = false,
                    Message = "Cannot found any registration"
                });
            }
            return Ok(new
            {
                IsSuccess = true,
                Message = "History retrieve successfully",
                Data = bloodHistory
            });
        }
        [Authorize]
        [HttpGet("api/donation-history")]
        public async Task<IActionResult> GetDonationHistory()
        {
            var bloodHistory = await _historyService.GetDonationHistoryAsync();

            if (bloodHistory == null || !bloodHistory.Any())
            {
                return NotFound(new
                {
                    IsSuccess = false,
                    Message = "Cannot found any record"
                });
            }
            return Ok(new
            {
                IsSuccess = true,
                Message = "History retrieve successfully",
                Data = bloodHistory
            });
        }

        [Authorize(Roles = "Staff")]
        [HttpGet("api/blood-registrations/search")]
        public async Task<IActionResult> SearchBloodRegistrationsByPhoneOrName([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyword = null, [FromQuery] int? eventId = null)
        {
            var bloodRegistrations = await _service.SearchBloodRegistrationsByPhoneOrName(pageNumber, pageSize, keyword, eventId);
            if (bloodRegistrations == null || !bloodRegistrations.Items.Any())
            {
                return NotFound(new
                {
                    IsSuccess = false,
                    Message = "No registrations found for the given keyword."
                });
            }
            return Ok(new
            {
                IsSuccess = true,
                Message = "Search completed successfully.",
                Data = bloodRegistrations
            });
        }
    }
}