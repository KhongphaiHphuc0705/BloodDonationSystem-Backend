using Application.DTO.BloodRegistration;
using Domain.Entities;


namespace Application.Service.BloodRegistrationServ
{
    public interface IBloodRegistrationService
    {
        Task<BloodRegistration?> RegisterDonation(int id, BloodRegistrationRequest request);
        Task<BloodRegistration?> RejectRegistration(int bloodRegisId);
        Task<BloodRegistration?> CancelOwnRegistration(int bloodRegisId);
    }
}
