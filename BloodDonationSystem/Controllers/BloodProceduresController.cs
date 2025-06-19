﻿using Application.DTO;
using Application.DTO.BloodProcedureDTO;
using Application.Service.BloodProcedureServ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonationSystem.Controllers
{
    [EnableCors("LocalPolicy")]
    [ApiController]
    public class BloodProceduresController(IBloodProcedureService _service) : ControllerBase
    {
        [Authorize(Roles = "Staff")]
        [HttpPost("api/blood-registrations/{id}/blood-procedures/collect")]
        public async Task<IActionResult> RecordBloodCollection(int id, [FromBody] BloodCollectionRequest request)
        {
            var bloodCollection = await _service.RecordBloodCollectionAsync(id, request);

            if (bloodCollection == null)
            {
                return BadRequest(new ApiResponse<BloodCollectionRequest>
                {
                    IsSuccess = false,
                    Message = "Failed to record blood collection."
                });
            }

            return Ok(new ApiResponse<BloodCollectionRequest>
            {
                IsSuccess = true,
                Message = "Blood collection recorded successfully.",
            });
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("api/blood-registrations/{id}/blood-procedures/qualify")]
        public async Task<IActionResult> RecordBloodQualification(int id, [FromBody] RecordBloodQualification request)
        {
            var bloodProcedure = await _service.UpdateBloodQualificationAsync(id, request);

            if (bloodProcedure == null)
                return BadRequest(new ApiResponse<RecordBloodQualification>()
                {
                    IsSuccess = false,
                    Message = "Failed to record blood qualification."
                });

            return Ok(new ApiResponse<RecordBloodQualification>()
            {
                IsSuccess = true,
                Message = "Blood qualification recorded successfully."
            });
        }
    }
}
