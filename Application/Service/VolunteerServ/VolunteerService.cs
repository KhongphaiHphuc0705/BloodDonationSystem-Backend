using Application.DTO.BloodRegistrationDTO;
using Domain.Entities;
using Infrastructure.Repository.Users;
using Infrastructure.Repository.VolunteerRepo;
using Microsoft.AspNetCore.Http;

namespace Application.Service.VolunteerServ
{
    public class VolunteerService(IVolunteerRepository _repoVolun, IHttpContextAccessor _contextAccessor, 
        IUserRepository _repoUser) : IVolunteerService
    {
        public async Task<Volunteer?> RegisterVolunteerDonation(RegisterVolunteerDonation request)
        {
            var userId = _contextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid creatorId))
                throw new UnauthorizedAccessException("User not found or invalid");

            var user = await _repoUser.GetUserByIdAsync(creatorId);
            if (user != null && user.LastDonation == null)
            {
                user.LastDonation = request.LastDonation;
                await _repoUser.UpdateUserProfileAsync(user);
            }

            var volunteer = new Volunteer
            {
                CreateAt = DateTime.Now,
                StartVolunteerDate = request.StartVolunteerDate,
                EndVolunteerDate = request.EndVolunteerDate,
                IsExpired = false,
                MemberId = creatorId
            };

            return await _repoVolun.AddAsync(volunteer);
        }
    }
}
