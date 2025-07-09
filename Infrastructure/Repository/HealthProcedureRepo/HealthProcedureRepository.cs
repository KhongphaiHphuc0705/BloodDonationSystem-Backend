using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Helper;
using Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.HealthProcedureRepo
{
    public class HealthProcedureRepository : GenericRepository<HealthProcedure>, IHealthProcedureRepository
    {
        public HealthProcedureRepository(BloodDonationSystemContext context) : base(context)
        {
        }

        // Hàm list ra hàng đợi để lấy máu
        public async Task<PaginatedResult<HealthProcedure>> GetHealthProceduresByPaged(int id, int pageNumber, int pageSize)
        {
            var healthProceduresCount = await _dbSet
                .Include(hp => hp.BloodRegistration)
                    .ThenInclude(br => br.Member)
                        .ThenInclude(mem => mem.BloodType)
                .Include(hp => hp.BloodRegistration)
                    .ThenInclude(br => br.Event)
                .Where(hp => hp.BloodRegistration.EventId == id && 
                            hp.BloodRegistration.IsApproved == true && 
                            hp.BloodRegistration.BloodProcedureId == null)
                .ToListAsync();

            var healthProcedures = healthProceduresCount
                .OrderBy(hp => hp.PerformedAt)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToList();

            var pagedHealthProcedure = new PaginatedResult<HealthProcedure>
            {
                Items = healthProcedures,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = healthProceduresCount.Count
            };
            return pagedHealthProcedure;
        }

        public async Task<HealthProcedure?> GetIncludeByIdAsync(int id)
        {
            return await _dbSet
                .Include(hp => hp.BloodRegistration)
                .FirstOrDefaultAsync(hp => hp.Id == id);
        }
    }
}
