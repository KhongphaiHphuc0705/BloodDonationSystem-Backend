using Application.DTO.BloodHistoryDTO;
using Domain.Entities;
using Infrastructure.Repository.BloodRegistrationRepo;
using Infrastructure.Repository.VolunteerRepo;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.BloodHistoryServ
{
    public class BloodHistoryService(IBloodRegistrationRepository _bloodRegistration,
                                     IHttpContextAccessor _contextAccessor) : IBloodHistoryService
    {
        public async Task<List<UnifiedBloodHistory>> GetBloodRegistraionHistoryAsync()
        {
            var user = _contextAccessor.HttpContext?.User.FindFirst("UserId").Value;
            if (string.IsNullOrEmpty(user) || !Guid.TryParse(user, out Guid userId))
            {
                throw new UnauthorizedAccessException("User not found or invalid");
            }

            var bloodRegistrations = await _bloodRegistration.GetBloodRegistrationHistoryAsync(userId);

            var volunteerRegistrations = await _bloodRegistration.GetVolunteerRegistrationHistoryAsync(userId);

            var result = new List<UnifiedBloodHistory>();

            if(bloodRegistrations != null)
            {
                var donationHistory = bloodRegistrations.Select(b => new UnifiedBloodHistory
                {
                    Id = b.Id,
                    Type = "Donation",
                    FacilityName = b.Event.Facility.Name,
                    EventName = b.Event.Title,
                    EventDate = b.Event.EventTime,
                    Longitude = b.Event.Facility.Longitude,
                    Latitude = b.Event.Facility.Latitude,
                    RegisterDate = DateOnly.FromDateTime(b.CreateAt)
                });
                result.AddRange(donationHistory);
            }

            if (volunteerRegistrations != null)
            {
                var volunteerHistory = volunteerRegistrations.Select(v => new UnifiedBloodHistory
                {
                    Id = v.Id,
                    Type = "Volunteer",
                    FacilityName = v.Event.Facility.Name,
                    Longitude = v.Event.Facility.Longitude,
                    Latitude = v.Event.Facility.Latitude,
                    RegisterDate = DateOnly.FromDateTime(v.CreateAt),
                    StartDate = DateOnly.FromDateTime(v.Volunteer.StartVolunteerDate),
                    EndDate = DateOnly.FromDateTime(v.Volunteer.EndVolunteerDate),
                    IsExpired = v.Volunteer.IsExpired
                });
                result.AddRange(volunteerHistory);
            }
            return result.ToList();
        }
    }
}
