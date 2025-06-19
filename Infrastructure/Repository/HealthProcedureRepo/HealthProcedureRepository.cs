using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repository.Base;

namespace Infrastructure.Repository.HealthProcedureRepo
{
    public class HealthProcedureRepository : GenericRepository<HealthProcedure>, IHealthProcedureRepository
    {
        public HealthProcedureRepository(BloodDonationSystemContext context) : base(context)
        {

        }
    }
}
