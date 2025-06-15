using Application.DTO.UserDTO;
using Domain.Entities;
using Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.Users
{
    public interface IUserService
    {
    
        Task<PaginatedResult<ListUserDTO>> GetAllUserAsync(int pageNumber, int pageSize);

        Task<bool> DeactiveUserAsync(Guid userId);
        Task<User> AddStaffAsync (UserDTO request);
    }
}
