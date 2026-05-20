using CommerceCleanArchitectureNET.Application.DTOs;
using CommerceCleanArchitectureNET.Application.UseCases.Users.GetCurrentUser;
using CommerceCleanArchitectureNET.Application.UseCases.Users.LoginUser;
using CommerceCleanArchitectureNET.Application.UseCases.Users.LogoutUser;
using CommerceCleanArchitectureNET.Application.UseCases.Users.RegisterUser;
using CommerceCleanArchitectureNET.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CommerceCleanArchitectureNET.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IRegisterUserUseCase _registerUser;
        private readonly ILoginUserUseCase _loginUser;
        private readonly IGetCurrentUserUseCase _getCurrentUser;
        private readonly ILogoutUserUseCase _logoutUser;

        public AuthController(
            IRegisterUserUseCase registerUser,
            ILoginUserUseCase loginUser,
            IGetCurrentUserUseCase getCurrentUser,
            ILogoutUserUseCase logoutUser)
        {
            _registerUser = registerUser;
            _loginUser = loginUser;
            _getCurrentUser = getCurrentUser;
            _logoutUser = logoutUser;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto, CancellationToken ct)
        {
            var result = await _registerUser.ExecuteAsync(dto, ct);

            if (!result.IsSuccess)
                return BadRequest(new ErrorResponse(result.Error!));

            return CreatedAtAction(nameof(Register), result.Value);
        }

        /// <summary>
        /// Login with email and password
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto, CancellationToken ct)
        {
            var result = await _loginUser.ExecuteAsync(dto, ct);

            if (!result.IsSuccess)
                return Unauthorized(new ErrorResponse(result.Error!));

            return Ok(result.Value);
        }

        /// <summary>
        /// Get the current authenticated user's profile
        /// </summary>
        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Me(CancellationToken ct)
        {
            var userId = GetUserIdFromClaims();
            if (userId is null)
                return Unauthorized(new ErrorResponse("Invalid token"));

            var result = await _getCurrentUser.ExecuteAsync(userId.Value, ct);

            if (!result.IsSuccess)
                return NotFound(new ErrorResponse(result.Error!));

            return Ok(result.Value);
        }

        /// <summary>
        /// Logout the current authenticated user
        /// </summary>
        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Logout(CancellationToken ct)
        {
            var userId = GetUserIdFromClaims();
            if (userId is null)
                return Unauthorized(new ErrorResponse("Invalid token"));

            var result = await _logoutUser.ExecuteAsync(userId.Value, ct);

            if (!result.IsSuccess)
                return BadRequest(new ErrorResponse(result.Error!));

            return NoContent();
        }

        private Guid? GetUserIdFromClaims()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? User.FindFirst("sub")?.Value;

            return Guid.TryParse(claim, out var id) ? id : null;
        }
    }
}
