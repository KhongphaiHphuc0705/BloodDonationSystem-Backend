using Application.DTO.HealthProcedureDTO;
using Domain.Entities;

namespace Application.Service.HealthProcedureServ
{
    public interface IHealthProcedureService
    {
        Task<HealthProcedure?> RecordHealthProcedureAsync(int id, HealthProcedureRequest request);
    }
}
