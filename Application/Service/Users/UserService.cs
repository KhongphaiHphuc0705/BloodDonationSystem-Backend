using Application.DTO.UserDTO;
using Domain.Entities;
using Infrastructure.Helper;
using Infrastructure.Repository.Auth;
using Infrastructure.Repository.Users;
using Microsoft.AspNetCore.Http;

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

        public async Task<PaginatedResult<ListUserDTO>> GetAllUserAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _userRepository.CountAllAsync();
            var users = await _userRepository.GetAllUserAsync(pageNumber, pageSize);

            var userDtos = users.Select(u => new ListUserDTO
            {
                Name = $"{u.FirstName} {u.LastName}",
                Email = u.Gmail,
                Status = u.IsActived,
                Dob = u.Dob,

            }).ToList();

            return new PaginatedResult<ListUserDTO>
            {
                Items = userDtos,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}