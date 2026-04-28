using CommerceCleanArchitectureNET.Application.Common;
using CommerceCleanArchitectureNET.Application.DTOs;

namespace CommerceCleanArchitectureNET.Application.UseCases.Users.RegisterUser
{
    public interface IRegisterUserUseCase
    {
        Task<Result<UserDto>> ExecuteAsync(RegisterUserDto dto, CancellationToken ct = default);
    }
}
