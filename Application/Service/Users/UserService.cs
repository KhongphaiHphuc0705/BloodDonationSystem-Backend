using Application.DTO;
using Domain.Entities;
using Infrastructure.Helper;
using Infrastructure.Repository.Auth;
using Infrastructure.Repository.Users;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.Users
{
    public class UserService(IUserRepository _userRepository,
                             IAuthRepository _authRepository,
                             IHttpContextAccessor _contextAccessor) : IUserService
    {
        public async Task<User> AddStaffAsync(UserDTO request)
        {
            if (await _authRepository.UserExistsByPhoneAsync(request.Phone))
            {
                // Log or handle the case where the user already exists
                return null; // User already exists
            }

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = request.Gender,
                Dob = request.Dob,
                Phone = request.Phone,
                Gmail = request.Gmail,
                BloodTypeId = request.BloodTypeId,
                CreateAt = DateTime.UtcNow,
                IsActived = true,
                RoleId = 2
            };
            await _authRepository.RegisterAsync(user);
            return user; // Return the added staff user
        }

        public async Task<bool> DeactiveUserAsync(Guid userId)
        {
            var deactiveUser = await _userRepository.DeactiveUserAsync(userId);
            if (deactiveUser <= 0)
            {
                // Log or handle the case where no user was deactivated
                return false;
            }

            return deactiveUser > 0;
        }

        public async Task<PaginatedResult<User>> GetAllUserAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("Page number and page size must be greater than zero.");
            }

            return await _userRepository.GetAllUserAsync(pageNumber, pageSize);
        }
    }
}
