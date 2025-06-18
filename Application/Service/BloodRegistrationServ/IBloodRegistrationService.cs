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
        Task<BloodRegistration?> RegisterDonation(int id, BloodRegistrationRequest request);
        Task<BloodRegistration?> RejectBloodRegistration(int bloodRegisId);
        Task<BloodRegistration?> CancelOwnRegistration(int id);
    }
}
