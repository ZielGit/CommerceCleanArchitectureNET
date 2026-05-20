using CommerceCleanArchitectureNET.Application.Common;
using CommerceCleanArchitectureNET.Domain.Repositories;

namespace CommerceCleanArchitectureNET.Application.UseCases.Users.LogoutUser
{
    public class LogoutUserUseCase : ILogoutUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public LogoutUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<bool>> ExecuteAsync(Guid userId, CancellationToken ct = default)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId, ct);

                if (user is null)
                    return Result<bool>.Failure("User not found");

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error during logout: {ex.Message}");
            }
        }
    }
}
