using Application.DTO.HealthProcedureDTO;
using Domain.Entities;
using Infrastructure.Repository.BloodRegistrationRepo;
using Infrastructure.Repository.HealthProcedureRepo;
using Microsoft.AspNetCore.Http;

namespace Application.Service.HealthProcedureServ
{
    public class HealthProcedureService(IHealthProcedureRepository _repo, IBloodRegistrationRepository _repoRegis,
        IHttpContextAccessor _contextAccessor) : IHealthProcedureService
    {
        public async Task<HealthProcedure?> RecordHealthProcedureAsync(int id, HealthProcedureRequest request)
        {
            var bloodRegistration = await _repoRegis.GetByIdAsync(id);
            if (bloodRegistration == null || bloodRegistration.IsApproved == false)
                return null;

            var userId = _contextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid creatorId))
            {
                throw new UnauthorizedAccessException("User not found or invalid");
            }

            var healthProcedure = new HealthProcedure
            {
                Pressure = request.Pressure,
                Temperature = request.Temperature,
                Hb = request.Hb,
                HBV = request.HBV,
                Weight = request.Weight,
                Height = request.Height,
                IsHealth = request.IsHealth,
                PerformedAt = DateTime.Now,
                Description = request.Description,
                PerformedBy = creatorId
            };
            var healthProcedureAdded = await _repo.AddAsync(healthProcedure);

            if (request.IsHealth == true)
                bloodRegistration.IsApproved = true;
            else
                bloodRegistration.IsApproved = false;
            bloodRegistration.UpdateAt = DateTime.Now;
            bloodRegistration.StaffId = creatorId;
            bloodRegistration.HealthId = healthProcedureAdded.Id;
            await _repoRegis.UpdateAsync(bloodRegistration);

            return healthProcedureAdded;
        }
    }
}
