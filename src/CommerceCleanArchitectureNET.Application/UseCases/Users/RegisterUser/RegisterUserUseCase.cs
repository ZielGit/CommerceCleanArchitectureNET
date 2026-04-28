using CommerceCleanArchitectureNET.Application.Common;
using CommerceCleanArchitectureNET.Application.DTOs;
using CommerceCleanArchitectureNET.Application.Interfaces;
using CommerceCleanArchitectureNET.Domain.Entities;
using CommerceCleanArchitectureNET.Domain.Exceptions;
using CommerceCleanArchitectureNET.Domain.Repositories;

namespace CommerceCleanArchitectureNET.Application.UseCases.Users.RegisterUser
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserUseCase(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<UserDto>> ExecuteAsync(RegisterUserDto dto, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                    return Result<UserDto>.Failure("Password must be at least 6 characters");

                var emailNormalized = dto.Email.Trim().ToLowerInvariant();

                if (await _userRepository.ExistsByEmailAsync(emailNormalized, ct))
                    return Result<UserDto>.Failure("A user with this email already exists");

                var passwordHash = _passwordHasher.Hash(dto.Password);
                var user = new User(emailNormalized, passwordHash, dto.FirstName, dto.LastName);

                await _userRepository.AddAsync(user, ct);
                await _unitOfWork.CommitAsync(ct);

                return Result<UserDto>.Success(MapToDto(user));
            }
            catch (DomainException ex)
            {
                return Result<UserDto>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                return Result<UserDto>.Failure($"Error registering user: {ex.Message}");
            }
        }

        private static UserDto MapToDto(User user) => new(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.Role,
            user.CreatedAt
        );
    }
}
