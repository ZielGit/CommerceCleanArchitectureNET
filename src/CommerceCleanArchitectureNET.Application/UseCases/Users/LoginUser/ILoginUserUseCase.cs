using CommerceCleanArchitectureNET.Application.Common;
using CommerceCleanArchitectureNET.Application.DTOs;

namespace CommerceCleanArchitectureNET.Application.UseCases.Users.LoginUser
{
    public interface ILoginUserUseCase
    {
        Task<Result<AuthResponseDto>> ExecuteAsync(LoginUserDto dto, CancellationToken ct = default);
    }
}
