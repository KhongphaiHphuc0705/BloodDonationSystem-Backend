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
        public string Address { get; set; }
        public DateTime LastDonation { get; set; }
        public DateTime StartVolunteerDate { get; set; }
        public DateTime EndVolunteerDate { get; set; }
        public int BloodTypeId { get; set; }
        public string Phone { get; set; }
        public string Gmail { get; set; }
    }
}
