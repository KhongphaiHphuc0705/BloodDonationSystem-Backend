using Application.DTO.BloodRegistration;
using Application.DTO.BloodRegistrationDTO;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.BloodRegistrationServ
{
    public interface IBloodRegistrationService
    {
        Task<Domain.Entities.BloodRegistration?> RegisterDonation(BloodRegistrationRequest request);
        Task<Domain.Entities.BloodRegistration?> RejectRegistration(int bloodRegisId);
        Task<Domain.Entities.BloodRegistration?> CancelOwnRegistration(int bloodRegisId);
        Task<Volunteer?> RegisterVolunteerDonation(RegisterVolunteerDonation request);
    }
}
