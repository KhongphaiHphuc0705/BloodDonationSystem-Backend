using Application.DTO.UserDTO;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Helper;
using Infrastructure.Repository.Auth;
using Infrastructure.Repository.Blood;
using Infrastructure.Repository.Users;
using Microsoft.AspNetCore.Http;

namespace Application.Service.Users
{
    public class UserService(IUserRepository _userRepository,
                             IAuthRepository _authRepository,
                             IHttpContextAccessor _contextAccessor,
                             IBloodRepository _bloodRepository) : IUserService
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
                Status = AccountStatus.Active,
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
                Name = $"{u.LastName} {u.FirstName}",
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

        public async Task<ProfileDTO?> GetUserByIdAsync(Guid userId)
        {
            var id = _contextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (id == null || !Guid.TryParse(id, out Guid parsedUserId) || parsedUserId != userId)
            {
                // Log or handle the case where the user ID is invalid or does not match
                return null; // Unauthorized access or invalid user ID
            }

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                // Log or handle the case where the user was not found
                return null; // User not found
            }

            var bloodType = await _bloodRepository.GetBloodTypeByIdAsync(user.BloodTypeId);

            return new ProfileDTO
            {
                Name = $"{user.LastName} {user.FirstName}",
                Phone = user.Phone,
                Gmail = user.Gmail,
                Gender = user.Gender,
                Dob = user.Dob,
                BloodType = bloodType.Type
            };
        }

        public async Task<ProfileDTO> UpdateUserProfileAsync(Guid userId, UserDTO updateUser)
        {
            var id = _contextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (id == null || !Guid.TryParse(id, out Guid parsedUserId) || parsedUserId != userId)
            {
                // Log or handle the case where the user ID is invalid or does not match
                return null; // Unauthorized access or invalid user ID
            }

            var existingUser = await _userRepository.GetUserByIdAsync(userId);
           // var bloodType = await _bloodRepository.GetBloodTypeByNameAsync(updateUser.BloodTypeId);

            existingUser.FirstName = updateUser.FirstName;
            existingUser.LastName = updateUser.LastName;
            existingUser.Phone = updateUser.Phone;
            existingUser.Gmail = updateUser.Gmail;
            existingUser.Gender = updateUser.Gender;
            existingUser.Dob = updateUser.Dob;
            existingUser.BloodTypeId = updateUser.BloodTypeId;

            var bloodType = await _bloodRepository.GetBloodTypeByIdAsync(updateUser.BloodTypeId);

            var updatedUser = await _userRepository.UpdateUserProfileAsync(existingUser);

            return new ProfileDTO
            {
                Name = $"{existingUser.LastName} {existingUser.FirstName}",
                Phone = existingUser.Phone,
                Gmail = existingUser.Gmail,
                Gender = existingUser.Gender,
                Dob = existingUser.Dob,
                BloodType = bloodType.Type
            };
        }
    }
}