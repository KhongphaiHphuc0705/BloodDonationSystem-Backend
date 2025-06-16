using Domain.Entities;
using Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Users
{
    public interface IUserRepository
    {
        Task<PaginatedResult<User>> GetAllUserAsync(int pageNumber, int pageSize);
        Task<User?> GetUserByIdAsync(Guid id);

        Task<int> DeactiveUserAsync(Guid id);
    }
}
