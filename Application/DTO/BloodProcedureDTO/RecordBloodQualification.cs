using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.BloodProcedureDTO
{
    public class RecordBloodQualification
    {
        public bool IsQualified { get; set; }
        public int BloodTypeId { get; set; }
        public BloodComponent BloodComponent { get; set; }
    }
}
