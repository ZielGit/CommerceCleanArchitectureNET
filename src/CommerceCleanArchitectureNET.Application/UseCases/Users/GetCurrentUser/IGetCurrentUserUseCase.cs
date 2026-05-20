using CommerceCleanArchitectureNET.Application.Common;
using CommerceCleanArchitectureNET.Application.DTOs;

namespace CommerceCleanArchitectureNET.Application.UseCases.Users.GetCurrentUser
{
    public interface IGetCurrentUserUseCase
    {
        Task<Result<UserDto>> ExecuteAsync(Guid userId, CancellationToken ct = default);
    }
}
