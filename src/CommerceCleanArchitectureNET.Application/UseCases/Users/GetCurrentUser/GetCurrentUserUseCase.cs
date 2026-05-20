using CommerceCleanArchitectureNET.Application.Common;
using CommerceCleanArchitectureNET.Application.DTOs;
using CommerceCleanArchitectureNET.Domain.Repositories;

namespace CommerceCleanArchitectureNET.Application.UseCases.Users.GetCurrentUser
{
    public class GetCurrentUserUseCase : IGetCurrentUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public GetCurrentUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserDto>> ExecuteAsync(Guid userId, CancellationToken ct = default)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId, ct);

                if (user is null)
                    return Result<UserDto>.Failure("User not found");

                if (!user.IsActive)
                    return Result<UserDto>.Failure("User account is inactive");

                return Result<UserDto>.Success(new UserDto(
                    user.Id,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    user.Role,
                    user.CreatedAt));
            }
            catch (Exception ex)
            {
                return Result<UserDto>.Failure($"Error retrieving user: {ex.Message}");
            }
        }
    }
}
