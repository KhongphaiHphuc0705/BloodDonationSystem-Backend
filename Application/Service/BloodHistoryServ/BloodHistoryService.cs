using Application.DTO.BloodHistoryDTO;
using Domain.Entities;
using Infrastructure.Repository.BloodRegistrationRepo;
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
        public async Task<List<EventBloodHistory>> GetBloodRegistraionHistoryAsync()
        {
            var user = _contextAccessor.HttpContext?.User.FindFirst("UserId").Value;
            if (string.IsNullOrEmpty(user) || !Guid.TryParse(user, out Guid userId))
            {
                throw new UnauthorizedAccessException("User not found or invalid");
            }

            var bloodRegistrations = await _bloodRegistration.GetBloodRegistrationHistoryAsync(userId);

            return bloodRegistrations.Select(bloodRegistration => new EventBloodHistory
            {
                Id = bloodRegistration.Id,
                EventName = bloodRegistration.Event.Title,
                EventDate = bloodRegistration.Event.EventTime,
                Longitude = bloodRegistration.Event.Facility.Longitude,
                Latitude = bloodRegistration.Event.Facility.Latitude,
                RegisterDate = DateOnly.FromDateTime(bloodRegistration.CreateAt),
            }).ToList();
        }
    }
}
