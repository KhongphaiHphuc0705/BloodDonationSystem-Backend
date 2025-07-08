using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Helper;
using Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.BloodProcedureRepo
{
    public class BloodProcedureRepository : GenericRepository<BloodProcedure>, IBloodProcedureRepository
    {
        public BloodProcedureRepository(BloodDonationSystemContext context) : base(context)
        {
        }

        // Lấy list những đơn đăng ký mà chưa kiểm tra chất lượng máu
        public async Task<PaginatedResult<BloodProcedure>> GetBloodCollectionsByPagedAsync(int eventId, int pageNumber, int pageSize)
        {
            var bloodCollectionsByEvent = await _dbSet
                .Include(bc => bc.BloodRegistration)
                    .ThenInclude(br => br.Event)
                .Include(bc => bc.BloodRegistration)
                    .ThenInclude(br => br.Member)
                .Where(bc => bc.BloodRegistration.EventId == eventId &&
                                bc.IsQualified == null)
                .ToListAsync();

            var bloodCollections = bloodCollectionsByEvent
                .OrderBy(bc => bc.PerformedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var pagedResult = new PaginatedResult<BloodProcedure>
            {
                Items = bloodCollections,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = bloodCollectionsByEvent.Count()
            };
            return pagedResult;
        }
    }
}