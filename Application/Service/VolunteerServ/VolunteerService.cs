using Application.DTO.BloodRegistrationDTO;
using Domain.Entities;
using Infrastructure.Repository.Users;
using Infrastructure.Repository.VolunteerRepo;
using Microsoft.AspNetCore.Http;
<<<<<<< HEAD
=======
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
>>>>>>> 7813e3d6e8429ba0d2072fc7e67b73930be3fabd

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
<<<<<<< HEAD
            {
                user.LastDonation = request.LastDonation;
                await _repoUser.UpdateUserProfileAsync(user);
            }
=======
                user.LastDonation = request.LastDonation;
>>>>>>> 7813e3d6e8429ba0d2072fc7e67b73930be3fabd

            var volunteer = new Volunteer
            {
                CreateAt = DateTime.Now,
                StartVolunteerDate = request.StartVolunteerDate,
                EndVolunteerDate = request.EndVolunteerDate,
                IsExpired = false,
                MemberId = creatorId
            };
<<<<<<< HEAD

=======
>>>>>>> 7813e3d6e8429ba0d2072fc7e67b73930be3fabd
            return await _repoVolun.AddAsync(volunteer);
        }
    }
}
