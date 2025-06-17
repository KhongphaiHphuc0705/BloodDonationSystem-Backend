using Application.DTO.BloodRegistration;
using Application.DTO.BloodRegistrationDTO;
using Domain.Entities;
using Infrastructure.Repository.BloodRegistrationRepo;
using Infrastructure.Repository.Events;
using Infrastructure.Repository.VolunteerRepo;
using Microsoft.AspNetCore.Http;


namespace Application.Service.BloodRegistrationServ
{
    public class BloodRegistrationService : IBloodRegistrationService
    {
        private readonly IBloodRegistrationRepository _repository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IVolunteerRepository _repoVolun;
        private readonly IEventRepository _repoEvent;

        public BloodRegistrationService(IBloodRegistrationRepository repository, IHttpContextAccessor contextAccessor, 
            IVolunteerRepository repoVolun, IEventRepository repoEvent)
        {
            _repository = repository;
            _contextAccessor = contextAccessor;
            _repoVolun = repoVolun;
            _repoEvent = repoEvent;
        }

        public async Task<BloodRegistration?> RegisterDonation(BloodRegistrationRequest request)
        {
            var userId = _contextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid creatorId))
            {
                throw new UnauthorizedAccessException("User not found or invalid");
            }

            var eventDetails = await _repoEvent.GetEventByIdAsync(request.EventId);
            if (eventDetails == null)
                return null;

            var registration = new BloodRegistration
            {
                CreateAt = DateTime.Now,
                MemberId = creatorId,
                EventId = request.EventId
            };

            await _repository.AddAsync(registration);
            return registration;
        }

        public async Task<BloodRegistration?> RejectRegistration(int bloodRegisId)
        {
            var bloodRegistration = await _repository.GetByIdAsync(bloodRegisId);
            if (bloodRegistration == null || bloodRegistration.IsApproved == false)
                return null;

            var userId = _contextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid creatorId))
            {
                throw new UnauthorizedAccessException("User not found or invalid");
            }

            bloodRegistration.IsApproved = false;
            bloodRegistration.UpdateAt = DateTime.Now;
            bloodRegistration.StaffId = creatorId;

            await _repository.UpdateAsync(bloodRegistration);
            return bloodRegistration;
        }

        public async Task<BloodRegistration?> CancelOwnRegistration(int bloodRegisId)
        {
            var bloodRegistration = await _repository.GetByIdAsync(bloodRegisId);
            // Check đơn có tồn tại, bị hủy, hay bị từ chối, hoặc đã khám hay chưa
            if (bloodRegistration == null || 
                bloodRegistration.IsApproved == false ||
                bloodRegistration.HealthId != null)
                return null;

            var userId = _contextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid creatorId))
            {
                throw new UnauthorizedAccessException("User not found or invalid");
            }

            // Xác thực người dùng hiện tại có đang là chủ của đơn đăng ký này hay không
            if (bloodRegistration.MemberId != creatorId)
                return null;

            bloodRegistration.IsApproved = false;
            bloodRegistration.UpdateAt = DateTime.Now;

            await _repository.UpdateAsync(bloodRegistration);
            return bloodRegistration;
        }

        public async Task<Volunteer?> RegisterVolunteerDonation(RegisterVolunteerDonation request)
        {
            var userId = _contextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid creatorId))
                throw new UnauthorizedAccessException("User not found or invalid");

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