using Application.DTO.BloodProcedureDTO;
using Application.Service.EmailServ;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Repository.BloodInventoryRepo;
using Infrastructure.Repository.BloodProcedureRepo;
using Infrastructure.Repository.BloodRegistrationRepo;
using Infrastructure.Repository.HealthProcedureRepo;
using Microsoft.AspNetCore.Http;


namespace Application.Service.BloodProcedureServ
{
    public class BloodProcedureService(IBloodProcedureRepository _repo, IBloodRegistrationRepository _repoRegis,
        IHealthProcedureRepository _repoHealth, IBloodInventoryRepository _repoInven,
        IHttpContextAccessor _contextAccessor, IEmailService _servEmail) : IBloodProcedureService
    {
        public async Task<BloodProcedure?> RecordBloodCollectionAsync(int id, BloodCollectionRequest request)
        {
            var bloodRegistration = await _repoRegis.GetByIdAsync(id);
            if (bloodRegistration == null || bloodRegistration.IsApproved == false)
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
            bloodRegistration.UpdateAt = DateTime.Now;
            bloodRegistration.StaffId = creatorId;
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

            var existingInventory = await _repoInven.GetByBloodRegisId(bloodRegistration.Id);
            if (existingInventory != null && !existingInventory.IsAvailable)  // Máu đã !IsAvailable thì không thể cập nhật
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

            bloodRegistration.UpdateAt = DateTime.Now;
            bloodRegistration.StaffId = creatorId;
            if (request.IsQualified)
                bloodRegistration.IsApproved = true;
            else 
                bloodRegistration.IsApproved = false;
            await _repoRegis.UpdateAsync(bloodRegistration);  // Cập nhật thông tin đơn đăng ký hiến máu

            // Kiểm tra kho máu để update cho trường hợp đánh lộn
            if (existingInventory == null) {
                if (request.IsQualified)
                    await AddNewBloodUnit(bloodRegistration);  // Thêm đơn vị máu mới vào kho nếu đủ điều kiện
                return bloodProcedure;
            }
            if (request.IsQualified == false)
            {
                existingInventory.IsAvailable = false;
                existingInventory.RemoveBy = creatorId;
                existingInventory.UpdateAt = DateTime.Now;
            }
            existingInventory.BloodTypeId = request.BloodTypeId;
            existingInventory.BloodComponent = request.BloodComponent;
            await _repoInven.UpdateAsync(existingInventory);

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
