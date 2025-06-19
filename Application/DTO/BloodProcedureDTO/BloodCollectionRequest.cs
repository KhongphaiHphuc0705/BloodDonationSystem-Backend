namespace Application.DTO.BloodProcedureDTO
{
    public class BloodCollectionRequest
    {
        public float Volume { get; set; } 
        public string? Description { get; set; } = null;
    }
}
