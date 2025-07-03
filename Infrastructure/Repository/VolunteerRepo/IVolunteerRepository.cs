using Domain.Entities;
using Infrastructure.Helper;
using Infrastructure.Repository.Base;

namespace Infrastructure.Repository.VolunteerRepo
{
    public interface IVolunteerRepository : IGenericRepository<Volunteer>
    {
        Task<List<Volunteer>> GetIncludePagedAsync(int pageNumber, int pageSize);
    }
}
