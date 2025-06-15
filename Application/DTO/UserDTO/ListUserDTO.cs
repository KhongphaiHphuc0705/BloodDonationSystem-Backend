using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.UserDTO
{
    public class ListUserDTO
    {
        public string Name { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }
        public DateOnly? Dob { get; set; }
        public Role Role { get; set; }
    }
}
