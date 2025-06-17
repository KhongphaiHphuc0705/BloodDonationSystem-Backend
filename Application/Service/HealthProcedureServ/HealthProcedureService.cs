using Application.DTO.HealthProcedureDTO;
using Domain.Entities;
using Infrastructure.Repository.BloodRegistrationRepo;
using Infrastructure.Repository.HealthProcedureRepo;
using Microsoft.AspNetCore.Http;

namespace Application.Service.HealthProcedureServ
{
    public class HealthProcedureService : IHealthProcedureService
    {
        private readonly IHealthProcedureRepository _repo;
        private readonly IBloodRegistrationRepository _repoRegis;
        private readonly IHttpContextAccessor _contextAccessor;

        public HealthProcedureService(IHealthProcedureRepository repo, IBloodRegistrationRepository repoRegis,
            IHttpContextAccessor contextAccessor)
        {
            _repo = repo;
            _repoRegis = repoRegis; 
            _contextAccessor = contextAccessor;
        }

        public async Task<HealthProcedure?> RecordHealthProcedureAsync(HealthProcedureRequest request)
        {
            var bloodRegistration = await _repoRegis.GetByIdAsync(request.BloodRegistrationId);

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

            bloodRegistration.HealthId = healthProcedureAdded.Id;
            await _repoRegis.UpdateAsync(bloodRegistration);

            return healthProcedureAdded;
        }
    }
}
