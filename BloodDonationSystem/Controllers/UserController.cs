using Application.DTO;
using Application.Service.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController (IUserService _userService) : ControllerBase
    {
        [HttpPut("deactive/{userId}")]
        public async Task<IActionResult> DeactiveUser(Guid userId)
        {
            var result = await _userService.DeactiveUserAsync(userId);
            if (!result)
            {
                return BadRequest("User may not exist or is already deactivated.");
            }
            return Ok("Deactive successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add-staff")]
        public async Task<IActionResult> AddStaffAsync([FromBody] UserDTO request)
        {
            if (request == null)
            {
                return BadRequest("Invalid user data.");
            }
            var user = await _userService.AddStaffAsync(request);
            if (user == null)
            {
                return BadRequest("User already exists with the provided phone number.");
            }
            return Ok(new 
            { 
                Message = "Staff added successfully.", 
                User = user 
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers([FromQuery]int pageNumber = 1,[FromQuery] int pageSize = 10)
        {
            var users = await _userService.GetAllUserAsync(pageNumber, pageSize);
            if (users == null || !users.Items.Any())
            {
                return NotFound("No users found.");
            }
            return Ok(users);
        }
    }
}
