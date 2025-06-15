using Application.DTO.BloodProcedureDTO;
using Application.Service.EmailServ;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Repository.BloodInventoryRepo;
using Infrastructure.Repository.BloodProcedureRepo;
using Infrastructure.Repository.BloodRegistrationRepo;
using Infrastructure.Repository.Events;
using Infrastructure.Repository.Facilities;
using Infrastructure.Repository.HealthProcedureRepo;
using Infrastructure.Repository.UserRepo;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.BloodProcedureServ
{
    public class BloodProcedureService : IBloodProcedureService
    {
        private readonly IBloodProcedureRepository _repo;
        private readonly IBloodRegistrationRepository _repoRegis;
        private readonly IHealthProcedureRepository _repoHealth;
        private readonly IBloodInventoryRepository _repoInven;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEmailService _servEmail;

        public BloodProcedureService(IBloodProcedureRepository repo, IBloodRegistrationRepository repoRegis, 
            IHealthProcedureRepository repoHealth, IBloodInventoryRepository repoInven, 
            IHttpContextAccessor contextAccessor, IEmailService servEmail)
        {
            _repo = repo;
            _repoRegis = repoRegis;
            _repoHealth = repoHealth;
            _repoInven = repoInven;
            _contextAccessor = contextAccessor;
            _servEmail = servEmail;
        }

        public async Task<BloodProcedure?> RecordBloodCollectionAsync(BloodCollectionRequest request)
        {
            var bloodRegistration = await _repoRegis.GetByIdAsync(request.BloodRegistrationId);
            if (bloodRegistration == null)
                return null;

            var healthProcedure = await _repoHealth.GetByIdAsync(bloodRegistration.HealthId);
            if (healthProcedure == null || healthProcedure.IsHealth == false)
                return null;

            var userId = _contextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid creatorId))
            {
                throw new UnauthorizedAccessException("User not found or invalid");
            }

            var bloodCollection = new BloodProcedure
            {
                Volume = request.Volume,
                PerformedAt = DateTime.Now,
                Description = request.Description,
                PerformedBy = creatorId
            };

            var bloodCollectionAdded = await _repo.AddAsync(bloodCollection);
            bloodRegistration.BloodProcedureId = bloodCollectionAdded.Id;
            await _repoRegis.UpdateAsync(bloodRegistration);

            // Gửi mail thông báo cho người hiến máu
            await _servEmail.SendEmailBloodCollectionAsync(bloodRegistration);

            return bloodCollectionAdded;
        }

        public async Task<BloodProcedure?> UpdateBloodQualificationAsync(int regisId, RecordBloodQualification request)
        {
            var bloodRegistration = await _repoRegis.GetByIdAsync(regisId);
            if (bloodRegistration == null)
                return null;

            var bloodProcedure = await _repo.GetByIdAsync(bloodRegistration.BloodProcedureId);
            if (bloodProcedure == null)
                return null;

            var userId = _contextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid creatorId))
            {
                throw new UnauthorizedAccessException("User not found or invalid");
            }

            bloodProcedure.IsQualified = request.IsQualified;
            bloodProcedure.BloodTypeId = request.BloodTypeId;
            bloodProcedure.BloodComponent = request.BloodComponent;
            bloodProcedure.PerformedAt = DateTime.Now;
            bloodProcedure.PerformedBy = creatorId;
            await _repo.UpdateAsync(bloodProcedure);  // Update thêm thông tin cho BloodProcedure

            if (bloodProcedure.IsQualified == false)
                return bloodProcedure;
            await AddNewBloodUnit(bloodRegistration);  // Thêm đơn vị máu mới vào kho nếu đủ điều kiện

            return bloodProcedure;
        }

        private async Task<BloodInventory?> AddNewBloodUnit(BloodRegistration bloodRegistration)
        {
            var bloodProcedure = await _repo.GetByIdAsync(bloodRegistration.BloodProcedureId);
            if (bloodProcedure == null) return null;

            var bloodInventory = new BloodInventory
            {
                Volume = bloodProcedure.Volume,
                CreateAt = DateTime.Now,
                IsAvailable = true,
                BloodTypeId = (int) bloodProcedure.BloodTypeId,
                BloodComponent = (BloodComponent) bloodProcedure.BloodComponent,
                RegistrationId = bloodRegistration.Id
            };

            // Set ngày hết hạn dựa trên Blood Component
            if (bloodProcedure.BloodComponent == BloodComponent.WholeBlood || bloodProcedure.BloodComponent == BloodComponent.RedBloodCells)
            {
                bloodInventory.ExpiredDate = bloodProcedure.PerformedAt.AddDays(42);
            }

            if (bloodProcedure.BloodComponent == BloodComponent.Plasma)
            {
                bloodInventory.ExpiredDate = bloodProcedure.PerformedAt.AddDays(14);
            }

            if (bloodProcedure.BloodComponent == BloodComponent.Platelets)
            {
                bloodInventory.ExpiredDate = bloodProcedure.PerformedAt.AddDays(5);
            }
                
            return await _repoInven.AddAsync(bloodInventory);
        }
    }
}
