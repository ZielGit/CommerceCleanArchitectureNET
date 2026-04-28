using CommerceCleanArchitectureNET.Application.Common;
using CommerceCleanArchitectureNET.Application.DTOs;
using CommerceCleanArchitectureNET.Application.Interfaces;
using CommerceCleanArchitectureNET.Domain.Repositories;

namespace CommerceCleanArchitectureNET.Application.UseCases.Users.LoginUser
{
    public class LoginUserUseCase : ILoginUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;

        public LoginUserUseCase(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<Result<AuthResponseDto>> ExecuteAsync(LoginUserDto dto, CancellationToken ct = default)
        {
            try
            {
                var emailNormalized = dto.Email.Trim().ToLowerInvariant();
                var user = await _userRepository.GetByEmailAsync(emailNormalized, ct);

                if (user is null || !_passwordHasher.Verify(dto.Password, user.PasswordHash))
                    return Result<AuthResponseDto>.Failure("Invalid email or password");

                if (!user.IsActive)
                    return Result<AuthResponseDto>.Failure("User account is inactive");

                var token = _tokenGenerator.GenerateToken(
                    user.Id.ToString(),
                    user.Email,
                    [user.Role]);

                var userDto = new UserDto(
                    user.Id,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    user.Role,
                    user.CreatedAt);

                return Result<AuthResponseDto>.Success(new AuthResponseDto(token, userDto));
            }
            catch (Exception ex)
            {
                return Result<AuthResponseDto>.Failure($"Error during login: {ex.Message}");
            }
        }
    }
}
