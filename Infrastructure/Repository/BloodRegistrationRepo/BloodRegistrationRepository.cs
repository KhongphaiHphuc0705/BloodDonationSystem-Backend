using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Helper;
using Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.BloodRegistrationRepo
{
    public class BloodRegistrationRepository : GenericRepository<BloodRegistration>, IBloodRegistrationRepository
    {
        public BloodRegistrationRepository(BloodDonationSystemContext context) : base(context)
        {
        }

        public async Task<List<BloodRegistration>> GetBloodRegistrationHistoryAsync(Guid userId)
        {
            return await _context.BloodRegistrations
                                        .Include(br => br.Event)
                                        .ThenInclude(e => e.Facility)
                                        .Include(br => br.HealthProcedure)
                                        .OrderByDescending(e => e.CreateAt)
                                        .Where(br => br.MemberId == userId && br.VolunteerId == null)
                                        .ToListAsync();
        }

        public async Task<List<BloodRegistration>> GetDonationHistoryAsync(Guid userId)
        {
            return await _context.BloodRegistrations
                .Include(br => br.Event)
                .ThenInclude(e => e.Facility)
                .Include(br => br.BloodProcedure)
                .Include(br => br.HealthProcedure)
                .Include(br => br.BloodInventory)
                .Where(br => br.MemberId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<BloodRegistration>> GetByEventAsync(int eventId)
        {
            return await _dbSet
                        .Where(br => br.EventId == eventId)
                        .ToListAsync();           
        }

        public async Task<PaginatedResult<BloodRegistration>> GetPagedAsync(int eventId, int pageNumber, int pageSize)
        {
            var bloodRegistrationsCount = await _dbSet
                                    .Include(br => br.Event)
                                    .Where(br => br.EventId == eventId && br.IsApproved == null)
                                    .ToListAsync();

            var bloodRegistrations = bloodRegistrationsCount
                                    .OrderBy(e => e.CreateAt)
                                    .Skip(pageSize * (pageNumber - 1))
                                    .Take(pageSize)
                                    .ToList();

            var pagedResult = new PaginatedResult<BloodRegistration>
            {
                Items = bloodRegistrations,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = bloodRegistrationsCount.Count
            };
            return pagedResult;
        }

        public async Task<List<BloodRegistration>> GetVolunteerRegistrationHistoryAsync(Guid userId)
        {
            return await _context.BloodRegistrations
                                        .Include(br => br.Event)
                                        .ThenInclude(e => e.Facility)
                                        .Include(br => br.Volunteer)
                                        .OrderByDescending(e => e.CreateAt)
                                        .Where(br => br.Volunteer.MemberId == userId)
                                        .ToListAsync();
        }
    }
}
