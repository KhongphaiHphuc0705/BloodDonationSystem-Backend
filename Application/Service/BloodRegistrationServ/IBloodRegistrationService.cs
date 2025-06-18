using Application.DTO.BloodRegistration;
using Application.DTO.BloodRegistrationDTO;
using Domain.Entities;


namespace Application.Service.BloodRegistrationServ
{
    public interface IBloodRegistrationService
    {
        Task<BloodRegistration?> RegisterDonation(int id, BloodRegistrationRequest request);
<<<<<<< HEAD
        Task<BloodRegistration?> RejectBloodRegistration(int bloodRegisId);
        Task<BloodRegistration?> CancelOwnRegistration(int id);
=======
        Task<BloodRegistration?> RejectRegistration(int bloodRegisId);
        Task<BloodRegistration?> CancelOwnRegistration(int bloodRegisId);
>>>>>>> 7813e3d6e8429ba0d2072fc7e67b73930be3fabd
    }
}
