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
        Task<BloodRegistration?> RegisterDonation(BloodRegistrationRequest request);
        Task<BloodRegistration?> EvaluateRegistration(int bloodRegisId, EvaluateBloodRegistration evaluation);
        Task<BloodRegistration?> CancelOwnRegistration(int bloodRegisId);
        Task<Volunteer?> RegisterVolunteerDonation(RegisterVolunteerDonation request);
    }
}
