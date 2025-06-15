using Application.DTO.BloodProcedureDTO;
using Application.Service.BloodProcedureServ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace BloodDonationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloodProcedureController : ControllerBase
    {
        private readonly IBloodProcedureService _service;

        public BloodProcedureController(IBloodProcedureService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("record-blood-collection")]
        public async Task<IActionResult> RecordBloodCollection([FromBody] BloodCollectionRequest request)
        {
            var bloodCollection = await _service.RecordBloodCollectionAsync(request);

            if (bloodCollection == null)
            {
                return BadRequest(new
                {
                    Message = "Failed to record blood collection."
                });
            }

            return Ok(new
            {
                Message = "Blood collection recorded successfully."
            });
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("record-blood-qualification/{regisId}")]
        public async Task<IActionResult> RecordBloodQualification(int regisId, [FromBody] RecordBloodQualification request)
        {
            var bloodProcedure = await _service.UpdateBloodQualificationAsync(regisId, request);

            if (bloodProcedure == null)
                return BadRequest(new
                {
                    Message = "Failed to record blood qualification."
                });

            return Ok(new
            {
                Message = "Blood qualification recorded successfully."
            });
        }
    }
}
