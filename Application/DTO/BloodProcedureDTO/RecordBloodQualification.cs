using Domain.Enums;

namespace Application.DTO.BloodProcedureDTO
{
    public class RecordBloodQualification
    {
        public bool IsQualified { get; set; }
        public int BloodTypeId { get; set; }
        public BloodComponent BloodComponent { get; set; }
    }
}
