﻿using Domain.Entities;
using Infrastructure.Helper;
using Infrastructure.Repository.Base;

namespace Infrastructure.Repository.VolunteerRepo
{
    public interface IVolunteerRepository : IGenericRepository<Volunteer>
    {
        Task<PaginatedResult<Volunteer>> GetPagedAsync(int pageNumber, int pageSize);
        Task<bool> UpdateAvailableDateAsync(int id, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Volunteer>?> GetVolunteerByMemberIdAsync(Guid memberId);
        Task<int> EndVolunteerDateExpired();
    }
}