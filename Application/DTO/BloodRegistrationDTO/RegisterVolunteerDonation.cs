using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.BloodRegistrationDTO
{
    public class RegisterVolunteerDonation
    {
        public DateTime LastDonation { get; set; }
        public DateTime StartVolunteerDate { get; set; }
        public DateTime EndVolunteerDate { get; set; }
    }
}
