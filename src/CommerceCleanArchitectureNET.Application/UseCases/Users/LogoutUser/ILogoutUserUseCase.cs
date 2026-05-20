using CommerceCleanArchitectureNET.Application.Common;

namespace CommerceCleanArchitectureNET.Application.UseCases.Users.LogoutUser
{
    public interface ILogoutUserUseCase
    {
        Task<Result<bool>> ExecuteAsync(Guid userId, CancellationToken ct = default);
    }
}
