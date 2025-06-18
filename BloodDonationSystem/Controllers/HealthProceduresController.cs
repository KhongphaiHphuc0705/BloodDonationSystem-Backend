using Application.DTO.HealthProcedureDTO;
using Application.Service.HealthProcedureServ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTO;

namespace BloodDonationSystem.Controllers
{
    [ApiController]
    public class HealthProceduresController(IHealthProcedureService _service) : ControllerBase
    {
        [Authorize(Roles = "Staff")]
        [HttpPost("api/blood-registrations/{id}/health-procedures")]
        public async Task<IActionResult> RecordHealthProcedure(int id, [FromBody] HealthProcedureRequest request)
        {
            var healthProcedure = await _service.RecordHealthProcedureAsync(id, request);

            if (healthProcedure == null)
            {
                return BadRequest(new ApiResponse<HealthProcedureRequest>()
                {
                    IsSuccess = false,
                    Message = "Health procedure recorded unsuccessfully." 
                });
            }

            return Ok(new ApiResponse<HealthProcedureRequest>()
            {

                IsSuccess = true,
                Message = "Health procedure recorded successfully.",
                Data = request
            });
        }
    }
}
