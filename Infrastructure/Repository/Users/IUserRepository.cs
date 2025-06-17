﻿using Domain.Entities;
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
        Task<int> CountAllAsync();
        Task<List<User>> GetAllUserAsync(int pageNumber, int pageSize);
        Task<User?> GetUserByIdAsync(Guid id);

        Task<User> AssignUserRole(User user);
        Task<User> UpdateUserProfileAsync(User updateUser);

        Task<int> DeactiveUserAsync(Guid id);
    }
}
