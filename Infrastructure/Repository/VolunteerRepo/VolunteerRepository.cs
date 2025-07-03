using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Helper;
using Infrastructure.Repository.Base;
using Infrastructure.Repository.Facilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.VolunteerRepo
{
    public class VolunteerRepository : GenericRepository<Volunteer>, IVolunteerRepository
    {
        private readonly IFacilityRepository _repoFacility;

        public VolunteerRepository(BloodDonationSystemContext context, IFacilityRepository repoFacility) : base(context)
        {
            _repoFacility = repoFacility;
        }

        public async Task<List<Volunteer>> GetIncludePagedAsync(int pageNumber, int pageSize)
        {
            var volunteers = await _dbSet
                .Include(v => v.Member)
                //.Where(v => GeographyHelper.CalculateDistanceKm(facility.Latitude, facility.Longitude, v.Member.Latitude, v.Member.Longitude) <= 5)
                //.OrderBy(v => GeographyHelper.CalculateDistanceKm(facility.Latitude, facility.Longitude, v.Member.Latitude, v.Member.Longitude))
                //    .ThenBy(v => v.CreateAt)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return volunteers;
        }
    }
}
